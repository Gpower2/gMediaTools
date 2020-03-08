using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace gMediaTools.Services
{
    public class AviSynthScriptService
    {
        private readonly AviSynthSourceFactory _aviSynthSourceFactory = new AviSynthSourceFactory();

        public string CreateAviSynthScript(MediaAnalyzeInfo mediaInfo)
        {
            if (mediaInfo == null)
            {
                throw new ArgumentNullException(nameof(mediaInfo));
            }
            if (string.IsNullOrWhiteSpace(mediaInfo.Filename))
            {
                throw new ArgumentException("No filename was provided!", nameof(mediaInfo.Filename));
            }

            string path = Path.GetDirectoryName(mediaInfo.Filename);

            // Set the initial AVS script filename
            string avsScriptFilename = $"{mediaInfo.Filename}.avs";

            // Check if this script already exists and create a new one
            int alreadyExistingFilecounter = 0;
            while (File.Exists(avsScriptFilename))
            {
                alreadyExistingFilecounter++;
                avsScriptFilename = $"{mediaInfo.Filename}.{alreadyExistingFilecounter}.avs";
            }

            StringBuilder avsScriptBuilder = new StringBuilder();

            // Decide on the Source filter
            //=============================
            string fileContainerFormat = mediaInfo.FileContainerFormat.Trim().ToLower();
            // Get the Source Service
            IAviSynthSourceService sourceService = _aviSynthSourceFactory.GetAviSynthSourceService(fileContainerFormat);

            avsScriptBuilder.AppendLine(sourceService.GetAviSynthSource(mediaInfo.Filename));

            // Decide what to do with VFR
            //=============================
            if (mediaInfo.VideoInfo.FrameRateMode == VideoFrameRateMode.VFR)
            {
                // Special handling for VFR
                // TODO
            }

            // Decide if we need resize
            //=============================
            if (mediaInfo.TargetVideoWidth != mediaInfo.VideoInfo.Width
                || mediaInfo.TargetVideoHeight != mediaInfo.VideoInfo.Height)
            {
                avsScriptBuilder.AppendLine($"Lanczos4resize({mediaInfo.TargetVideoWidth}, {mediaInfo.TargetVideoHeight})");
            }

            // Decide if we need colorspace conversion
            //=============================
            if (!mediaInfo.VideoInfo.ColorSpace.Trim().ToLower().Equals("yv12"))
            {
                avsScriptBuilder.AppendLine("ConvertToYV12()");
            }

            // Write the file
            using (StreamWriter sw = new StreamWriter(avsScriptFilename, false, Encoding.GetEncoding("el-GR")))
            {
                sw.Write(avsScriptBuilder.ToString());
            }

            return avsScriptFilename;
        } 
    }
}

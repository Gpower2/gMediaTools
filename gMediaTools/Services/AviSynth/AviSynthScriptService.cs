using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using gMediaTools.Extensions;
using gMediaTools.Factories;
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Services.AviSynth.VideoSource;

namespace gMediaTools.Services.AviSynth
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

            // Get the AVS script filename
            string avsScriptFilename = $"{mediaInfo.Filename}.avs".GetNewFileName();

            StringBuilder avsScriptBuilder = new StringBuilder();

            // Decide on the Source filter
            //=============================
            string fileContainerFormat = mediaInfo.FileContainerFormat.Trim().ToLower();
            // Get the Source Service
            IAviSynthVideoSourceService sourceService = _aviSynthSourceFactory.GetAviSynthSourceService(fileContainerFormat);

            avsScriptBuilder.AppendLine(sourceService.GetAviSynthVideoSource(mediaInfo.Filename));

            // Decide what to do with VFR
            //=============================
            if (mediaInfo.VideoInfo.FrameRateMode == VideoFrameRateMode.VFR)
            {
                // TODO: Special handling for VFR
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
            using (StreamWriter sw = new StreamWriter(avsScriptFilename, false, Encoding.GetEncoding(1253)))
            {
                sw.Write(avsScriptBuilder.ToString());
            }

            return avsScriptFilename;
        } 
    }
}

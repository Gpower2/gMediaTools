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
            string fileContainerFormat = mediaInfo.FileContainerFormat.Trim().ToLower();
            string fileExtension = mediaInfo.FileExtension.Trim().ToLower();


            // Decide if we need resize
            if (mediaInfo.TargetVideoWidth != mediaInfo.VideoInfo.Width
                || mediaInfo.TargetVideoHeight != mediaInfo.VideoInfo.Height)
            {
                avsScriptBuilder.AppendLine($"Lanczos4resize({mediaInfo.TargetVideoWidth}, {mediaInfo.TargetVideoHeight})");
            }

            // Decide if we need colorspace conversion
            if (!mediaInfo.VideoInfo.ColorSpace.Trim().ToLower().Equals("yv12"))
            {
                avsScriptBuilder.AppendLine("ConvertToYV12()");
            }

            using (StreamWriter sw = new StreamWriter(avsScriptFilename, false, Encoding.ASCII))
            {
                sw.Write(avsScriptBuilder.ToString());
            }

            return avsScriptFilename;
        } 
    }
}

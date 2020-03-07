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


            // Decide if we need resize


            // Decide if we need colorspace conversion



            return avsScriptFilename;
        } 
    }
}

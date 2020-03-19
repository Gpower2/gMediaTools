using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;
using gMediaTools.Services.AviSynth;
using gMediaTools.Services.AviSynth.VideoSource;

namespace gMediaTools.Services.TimeCodes
{
    public class TimeCodesProviderService
    {
        private readonly AviSynthFileService _aviSynthFileService = ServiceFactory.GetService<AviSynthFileService>();

        public string GetTimecodesFileName(string mediaFileName)
        {
            if (string.IsNullOrWhiteSpace(mediaFileName))
            {
                throw new ArgumentException("No filename was provided!", nameof(mediaFileName));
            }

            // Get the cache filename
            string cacheFileName = $"{mediaFileName}.ffindex".GetNewFileName();

            // Get the timecodes filename
            string timeCodesFileName = $"{mediaFileName}.tcodes.txt".GetNewFileName();

            // Get the AviSynth script
            string scriptFileName = CreateAviSynthTimecodesScript(mediaFileName, cacheFileName, timeCodesFileName);

            // Open the AviSynth Script to generate the timecodes
            using (_aviSynthFileService.OpenAviSynthScriptFile(scriptFileName))
            {
            }

            // Delete temporary files
            File.Delete(cacheFileName);
            File.Delete(scriptFileName);

            // Return the timecodes file
            return timeCodesFileName;
        }

        public string CreateAviSynthTimecodesScript(string mediaFileName, string cacheFileName, string timecodesFileName)
        {
            if (string.IsNullOrWhiteSpace(mediaFileName))
            {
                throw new ArgumentException("No filename was provided!", nameof(mediaFileName));
            }

            // Get the AVS script filename
            string avsScriptFilename = $"{mediaFileName}.tc.avs".GetNewFileName();

            StringBuilder avsScriptBuilder = new StringBuilder();

            // Use FFMS2 Source filter to get the timecodes
            //=============================
            // Get the Source Service
            AviSynthFfms2VideoSourceService sourceService = ServiceFactory.GetService<AviSynthFfms2VideoSourceService>();

            avsScriptBuilder.AppendLine(sourceService.GetAviSynthVideoSource(mediaFileName, cacheFileName, timecodesFileName));

            // Write the file
            using (StreamWriter sw = new StreamWriter(avsScriptFilename, false, Encoding.GetEncoding(1253)))
            {
                sw.Write(avsScriptBuilder.ToString());
            }

            return avsScriptFilename;
        }
    }
}

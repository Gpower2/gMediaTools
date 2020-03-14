using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;
using gMediaTools.Services.AviSynth;

namespace gMediaTools.Services.TimeCodes
{
    public class TimeCodesProviderService
    {
        private readonly AviSynthScriptService _aviSynthScriptService = new AviSynthScriptService();

        private readonly AviSynthFileService _aviSynthFileService = new AviSynthFileService();

        public string GetTimecodesFileName(string mediaFileName)
        {
            if (string.IsNullOrWhiteSpace(mediaFileName))
            {
                throw new ArgumentException("No filename was provided!", nameof(mediaFileName));
            }

            // Get the timecodes filename
            string timeCodesFilename = $"{mediaFileName}.tcodes.txt".GetNewFileName();

            // Get the AviSynth script
            string scriptFileName = _aviSynthScriptService.CreateAviSynthTimecodesScript(mediaFileName, timeCodesFilename);

            // Open the AviSynth Script to generate the timecodes
            using (_aviSynthFileService.OpenAviSynthScriptFile(scriptFileName))
            {
                return timeCodesFilename;
            }
        }
    }
}

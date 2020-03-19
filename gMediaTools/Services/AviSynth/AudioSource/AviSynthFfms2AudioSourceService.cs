using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthFfms2AudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(string fileName, int trackNumber, bool overWriteScriptFile)
        {
            // Find cache file
            string cacheFileName = $"{fileName}.ffindex";
            if (!overWriteScriptFile)
            {
                cacheFileName = cacheFileName.GetNewFileName();
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"FFAudioSource(source = \"{fileName}\", ");
            sb.Append($"track = {trackNumber}, cache = true, cachefile = \"{cacheFileName}\", ");
            sb.Append($"adjustdelay = -1)");

            return sb.ToString();
        }
    }
}

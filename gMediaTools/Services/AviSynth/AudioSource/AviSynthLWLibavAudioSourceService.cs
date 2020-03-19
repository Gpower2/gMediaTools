using gMediaTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthLWLibavAudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(string fileName, int trackNumber, bool overWriteScriptFile)
        {
            // Find cache file
            string cacheFileName = $"{fileName}.lwi";
            if (!overWriteScriptFile)
            {
                cacheFileName = cacheFileName.GetNewFileName();
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"LWLibavAudioSource(source = \"{fileName}\", ");
            sb.Append($"stream_index = {trackNumber}, cache = true, cachefile = \"{cacheFileName}\", ");
            sb.Append($"av_sync = false, rate = 0)");

            return sb.ToString();
        }
    }
}

using System.Text;
using gMediaTools.Extensions;
using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthLWLibavAudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(MediaAnalyzeInfo mediaAnalyzeInfo, string fileName, int trackNumber, bool overWriteScriptFile)
        {
            // Find cache file
            string cacheFileName = $"{fileName}.lwi";
            if (!overWriteScriptFile)
            {
                cacheFileName = cacheFileName.GetNewFileName();
            }
            mediaAnalyzeInfo.TempFiles.Add(cacheFileName);

            StringBuilder sb = new StringBuilder();

            sb.Append($"LWLibavAudioSource(source = \"{fileName}\", ");
            sb.Append($"stream_index = {trackNumber}, cache = true, cachefile = \"{cacheFileName}\", ");
            sb.Append($"av_sync = false, rate = 0)");

            return sb.ToString();
        }
    }
}

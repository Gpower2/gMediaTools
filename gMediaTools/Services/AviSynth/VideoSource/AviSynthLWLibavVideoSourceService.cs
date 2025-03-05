using System.Text;
using gMediaTools.Extensions;
using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthLWLibavVideoSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(MediaAnalyzeInfo mediaAnalyzeInfo, string fileName, bool overWriteScriptFile)
        {
            // Find cache file
            string cacheFileName = $"{fileName}.lwi";
            if (!overWriteScriptFile)
            {
                cacheFileName = cacheFileName.GetNewFileName();
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"LWLibavVideoSource(source = \"{fileName}\", ");
            sb.Append($"stream_index = -1, cache = true, cachefile = \"{cacheFileName}\", ");
            sb.Append($"fpsnum = 0, fpsden = 1, repeat = false, threads = 0, seek_mode = 0)");

            return sb.ToString();
        }
    }
}

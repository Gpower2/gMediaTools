using System.Text;
using gMediaTools.Extensions;
using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthFfms2AudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(MediaAnalyzeInfo mediaAnalyzeInfo, string fileName, int trackNumber, bool overWriteScriptFile)
        {
            // Find cache file
            string cacheFileName = $"{fileName}.ffindex";
            if (!overWriteScriptFile)
            {
                cacheFileName = cacheFileName.GetNewFileName();
            }
            mediaAnalyzeInfo.TempFiles.Add(cacheFileName);

            StringBuilder sb = new StringBuilder();

            sb.Append($"FFAudioSource(source = \"{fileName}\", ");
            sb.Append($"track = {trackNumber}, cache = true, cachefile = \"{cacheFileName}\", ");
            sb.Append($"adjustdelay = -1)");

            return sb.ToString();
        }
    }
}

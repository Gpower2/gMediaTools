using System.Text;
using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthLSMASHAudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(MediaAnalyzeInfo mediaAnalyzeInfo, string fileName, int trackNumber, bool overWriteScriptFile)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"LSMASHAudioSource(source = \"{fileName}\", ");
            sb.Append($"track = {trackNumber}, rate = 0)");

            return sb.ToString();
        }
    }
}

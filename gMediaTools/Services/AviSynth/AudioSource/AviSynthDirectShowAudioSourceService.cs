using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthDirectShowAudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(MediaAnalyzeInfo mediaAnalyzeInfo, string fileName, int trackNumber, bool overWriteScriptFile)
        {
            return $"DirectShowSource(\"{fileName}\", seek = true, video = false, audio = true, convertfps = false)";
        }
    }
}

using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public interface IAviSynthAudioSourceService
    {
        string GetAviSynthAudioSource(MediaAnalyzeInfo mediaAnalyzeInfo, string fileName, int trackNumber, bool overWriteScriptFile);
    }
}

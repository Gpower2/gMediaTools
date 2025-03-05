using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public interface IAviSynthVideoSourceService
    {
        string GetAviSynthVideoSource(MediaAnalyzeInfo mediaAnalyzeInfo, string fileName, bool overWriteScriptFile);
    }
}

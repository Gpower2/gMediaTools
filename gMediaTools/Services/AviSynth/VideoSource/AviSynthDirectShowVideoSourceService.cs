using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthDirectShowVideoSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(MediaAnalyzeInfo mediaAnalyzeInfo, string filename, bool overWriteScriptFile)
        {
            return $"DirectShowSource(\"{filename}\", seek = true, video = true, audio = false, convertfps = false)";
        }
    }
}

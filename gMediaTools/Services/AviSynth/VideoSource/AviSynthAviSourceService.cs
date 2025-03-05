using System;
using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthAviSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(MediaAnalyzeInfo mediaAnalyzeInfo, string filename, bool overWriteScriptFile)
        {
            return $"AviSource(\"{filename}\"){Environment.NewLine}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthAviSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(string filename, bool overWriteScriptFile)
        {
            return $"AviSource(\"{filename}\"){Environment.NewLine}";
        }
    }
}

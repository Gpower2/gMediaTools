using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthDirectShowSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(string filename)
        {
            return $"DirectShowSource(\"{filename}\"){Environment.NewLine}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public interface IAviSynthVideoSourceService
    {
        string GetAviSynthVideoSource(string filename, bool overWriteScriptFile);
    }
}

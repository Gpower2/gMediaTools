using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthLSMASHVideoSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(string fileName, bool overWriteScriptFile)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"LSMASHVideoSource(source = \"{fileName}\", ");
            sb.Append($"track = 0, threads = 0, seek_mode = 0, ");
            sb.Append($"fpsnum = 0, fpsden = 1)");

            return sb.ToString();
        }
    }
}

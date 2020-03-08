using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services
{
    public class AviSynthDirectShowSourceService : IAviSynthSourceService
    {
        public string GetAviSynthSource(string filename)
        {
            return $"DirectShowSource(\"{filename}\"){Environment.NewLine}";
        }
    }
}

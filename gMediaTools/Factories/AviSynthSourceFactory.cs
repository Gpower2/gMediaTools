using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Services;
using gMediaTools.Services.AviSynth;
using gMediaTools.Services.AviSynth.VideoSource;

namespace gMediaTools.Factories
{
    public class AviSynthSourceFactory
    {
        public IAviSynthVideoSourceService GetAviSynthSourceService(string fileContainerFormat)
        {
            string container = fileContainerFormat.Trim().ToLower();

            if (container.Equals("matroska"))
            {
                // MKV => FFMS2
                return ServiceFactory.GetService<AviSynthFfms2VideoSourceService>();
            }
            else if (container.Equals("windows media"))
            {
                // WMV => FFMS2
                return ServiceFactory.GetService<AviSynthFfms2VideoSourceService>();
            }
            else if (container.Equals("mpeg-4"))
            {
                // MP4 or MOV => FFMS2
                return ServiceFactory.GetService<AviSynthFfms2VideoSourceService>();
            }
            else if (container.Equals("avi"))
            {
                // AVI => AviSource
                return ServiceFactory.GetService<AviSynthAviSourceService>();
            }
            else if (container.Equals("flash video"))
            {
                // FLV => FFMS2
                return ServiceFactory.GetService<AviSynthFfms2VideoSourceService>();
            }
            else
            {
                // Could not identify container/format
                // Let's play it safe and use DirectShowSource
                return ServiceFactory.GetService<AviSynthDirectShowVideoSourceService>();
            }
        }
    }
}

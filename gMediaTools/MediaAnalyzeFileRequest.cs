using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools
{
    public class MediaAnalyzeFileRequest
    {
        public string MediaFile { get; set; }

        public double BitratePercentageThreshold { get; set; }

        public double GainPercentageThreshold { get; set; }

        public int MaxAllowedWidth { get; set; }

        public int MaxAllowedHeight { get; set; }
    }
}

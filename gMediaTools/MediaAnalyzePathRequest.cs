using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools
{
    public class MediaAnalyzePathRequest
    {
        public string MediaDirectoryName { get; set; }

        public double BitratePercentageThreshold { get; set; }

        public double GainPercentageThreshold { get; set; }
    }
}

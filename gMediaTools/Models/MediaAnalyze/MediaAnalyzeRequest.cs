using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.MediaAnalyze
{
    public abstract class MediaAnalyzeRequest
    {
        public double BitratePercentageThreshold { get; set; }

        public double GainPercentageThreshold { get; set; }

        public int MaxAllowedWidth { get; set; }

        public int MaxAllowedHeight { get; set; }

        /// <summary>
        /// In bps
        /// </summary>
        public int MinAllowedBitrate { get; set; }
    }
}

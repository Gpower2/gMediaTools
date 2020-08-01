using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Models.MediaAnalyze;

namespace gMediaTools.Models
{
    public class FormStateInfo
    {
        public string InputFolder { get; set; }

        public int BitratePercentageThreshold { get; set; } = 10;

        public int GainPercentageThreshold { get; set; } = 20;

        public int MaxAllowedWidth { get; set; } = 1280;

        public int MaxAllowedHeight { get; set; } = 720;

        public int MinAllowedBitrate { get; set; } = 700;

        public IEnumerable<MediaAnalyzeInfo> MediaAnalyzeInfos { get; set; } = new List<MediaAnalyzeInfo>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models
{
    public class VideoFrameSection
    {
        public int StartFrameNumber { get; set; }

        public int EndFrameNumber { get; set; }

        public string Name { get; set; }

        public List<VideoFrameInfo> FramesToDelete { get; } = new List<VideoFrameInfo>();

        public List<VideoFrameInfo> FramesToDuplicate { get; } = new List<VideoFrameInfo>();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.Muxer
{
    public class MkvMergeMuxer : IMuxer
    {
        public string MuxerFileName { get; }

        public string[] SupportedExtensions { get; } = new string[] { "mkv" };

        public MkvMergeMuxer(string muxerFileName)
        {
            MuxerFileName = muxerFileName ?? throw new ArgumentNullException(nameof(muxerFileName));
        }
    }
}

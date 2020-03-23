using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.Encoder
{
    public interface IAudioEncoderSettings
    {
        int ChannelMask { get; }

        string FileExtension { get; }
    }
}

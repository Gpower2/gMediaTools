using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.Encoder
{
    public class DefaultAudioEncoderSettings : IAudioEncoderSettings
    {
        public int ChannelMask { get; }

        public string FileExtension { get; }

        public DefaultAudioEncoderSettings(int channelMask, string fileExtension)
        {
            ChannelMask = channelMask;
            FileExtension = fileExtension ?? throw new ArgumentNullException(nameof(fileExtension));
        }

    }
}

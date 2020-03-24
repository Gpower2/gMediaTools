using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.Muxer
{
    public class DefaultMuxerSettings : IMuxerSettings
    {
        public string VideoSourceFileName { get; }

        public string AudioSourceFileName { get; }

        public string FileExtension { get; }

        public DefaultMuxerSettings(string videoSourceFileName, string audioSourceFileName, string fileExtension)
        {
            VideoSourceFileName = videoSourceFileName ?? throw new ArgumentNullException(nameof(videoSourceFileName));
            AudioSourceFileName = audioSourceFileName ?? throw new ArgumentNullException(nameof(audioSourceFileName));
            FileExtension = fileExtension ?? throw new ArgumentNullException(nameof(fileExtension));
        }
    }
}

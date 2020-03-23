using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.Encoder
{
    public class NeroAacAudioEncoder : IAudioEncoder
    {
        public string EncoderFileName { get; }
        public string ExecutableArguments { get; } = "-br 159000 -ignorelength -if - -of \"{0}\"";
        public bool WriteHeader { get; } = true;
        public AudioHeaderType HeaderType { get; } = AudioHeaderType.WAV;
        public bool IsLossless { get; } = false;

        public string[] SupportedFileExtensions { get; } = new string[] { "m4a" };

        public NeroAacAudioEncoder(string encoderFilename)
        {
            EncoderFileName = encoderFilename ?? throw new ArgumentNullException(nameof(encoderFilename));
        }
    }
}

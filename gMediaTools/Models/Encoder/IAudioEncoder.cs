using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.Encoder
{
    public enum AudioHeaderType
    {
        WAV = 0,
        W64 = 1,
        RF64 = 2
    }

    public interface IAudioEncoder
    {
        string EncoderFileName { get; }

        string ExecutableArguments { get; }

        bool WriteHeader { get; }

        AudioHeaderType HeaderType { get; }

        bool IsLossless { get; }

        string[] SupportedFileExtensions { get; }
    }
}

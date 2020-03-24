using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.Muxer
{
    public interface IMuxerSettings
    {
        string VideoSourceFileName { get; }

        string AudioSourceFileName { get; }

        string FileExtension { get; }
    }
}

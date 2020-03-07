using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools
{
    public class MediaAnalyzeActions
    {
        public Action<string> SetCurrentFileAction { get; set; }

        public Action<string> LogErrorAction { get; set; }

        public Action<int, int> UpdateProgressAction { get; set; }

        public Action<MediaAnalyzeInfo> HandleMediaForReencodeAction { get; set; }
    }
}

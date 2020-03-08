using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools
{
    public class MediaAnalyzePathRequest: MediaAnalyzeRequest
    {
        public string MediaDirectoryName { get; set; }

        public MediaAnalyzePathRequest()
        {

        }

        public MediaAnalyzePathRequest(string mediaDirectoryName, MediaAnalyzeRequest baseRequest)
        {
            MediaDirectoryName = mediaDirectoryName;

            BitratePercentageThreshold = baseRequest.BitratePercentageThreshold;

            GainPercentageThreshold = baseRequest.GainPercentageThreshold;

            MaxAllowedWidth = baseRequest.MaxAllowedWidth;

            MaxAllowedHeight = baseRequest.MaxAllowedHeight;

            MinAllowedBitrate = baseRequest.MinAllowedBitrate;
        }
    }
}

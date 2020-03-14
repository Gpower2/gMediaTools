using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.MediaAnalyze
{
    public class MediaAnalyzeFileRequest : MediaAnalyzeRequest
    {
        public string MediaFile { get; set; }

        public MediaAnalyzeFileRequest()
        {

        }

        public MediaAnalyzeFileRequest(string mediaFilename, MediaAnalyzeRequest baseRequest)
        {
            MediaFile = mediaFilename;

            BitratePercentageThreshold = baseRequest.BitratePercentageThreshold;

            GainPercentageThreshold = baseRequest.GainPercentageThreshold;

            MaxAllowedWidth = baseRequest.MaxAllowedWidth;

            MaxAllowedHeight = baseRequest.MaxAllowedHeight;

            MinAllowedBitrate = baseRequest.MinAllowedBitrate;
        }
    }
}

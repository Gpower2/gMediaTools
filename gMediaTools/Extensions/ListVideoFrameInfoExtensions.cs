using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Models;

namespace gMediaTools.Extensions
{
    public static class ListVideoFrameInfoExtensions
    {
        public static bool IsCFR(this List<VideoFrameInfo> videoFrameList)
        {
            // We don't care for more than 3 digits accuracy in ms duration
            return (videoFrameList.GroupBy(x => decimal.Round(x.Duration, 3, MidpointRounding.AwayFromZero)).Count() == 1);
        }
    }
}

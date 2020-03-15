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
        private static readonly List<decimal> _cfrFrameRates = new List<decimal>()
            {
                15.0m,
                24000.0m / 1001.0m,
                25.0m,
                30000.0m / 1001.0m,
                30.0m,
                48000.0m / 1001.0m,
                50.0m,
                60000.0m / 1001.0m,
                60.0m
            };

        public static bool IsCFR(this List<VideoFrameInfo> videoFrameList)
        {
            // We don't care for more than 3 digits accuracy in ms duration
            return (videoFrameList.GroupBy(x => decimal.Round(x.Duration, 3, MidpointRounding.AwayFromZero)).Count() == 1);
        }

        public static decimal GetNearestCfrFrameRate(this List<VideoFrameInfo> videoFrameList)
        {
            // Group all the frame reates present in our media file
            var groupedFrameRates = videoFrameList
                .GroupBy(f =>
                    decimal.Round(f.FrameRate, 3, MidpointRounding.AwayFromZero),
                    (keyFrameRate, groupFrameRates) => new
                    {
                        FrameRate = keyFrameRate,
                        Count = groupFrameRates.Count(),
                    }
                );

            // Get the frame rate present in most frames
            decimal mostUsedFrameRate = groupedFrameRates
                .OrderByDescending(g => g.Count)
                .FirstOrDefault().FrameRate;

            // Get the closest CFR framerate
            var targetVideoFrameRate =
                _cfrFrameRates.Select(
                    c => new { Diff = Math.Abs(c - mostUsedFrameRate), FrameRate = c }
                ).Distinct()
                .OrderBy(k => k.Diff)
                .FirstOrDefault().FrameRate;

            return targetVideoFrameRate;
        }

    }
}

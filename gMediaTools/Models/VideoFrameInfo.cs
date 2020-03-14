using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models
{
    public class VideoFrameInfo
    {
        /// <summary>
        /// The specific frame number in the video
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The start time of the frame in ms
        /// </summary>
        public decimal StartTime { get; set; }

        /// <summary>
        /// The end time of the frame in ms
        /// </summary>
        public decimal EndTime
        {
            get
            {
                return StartTime + Duration;
            }
            set
            {
                if (value >= Duration)
                {
                    StartTime = value - Duration;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid Frame End Time! End time must be greater than the frame duration.");
                }
            }
        }

        /// <summary>
        /// The frame duration of the current frame in ms
        /// </summary>
        public decimal Duration { get; set; }

        /// <summary>
        /// The frame rate that corresponds to the current frame in frame/ms
        /// </summary>
        public decimal FrameRate
        {
            get
            {
                if (Duration == 0)
                {
                    return 0.0m;
                }
                return 1000.0m / Duration;
            }
            set
            {
                if (value > 0.0m)
                {
                    Duration = 1000.0m / value;
                }
                else if (value == 0.0m)
                {
                    Duration = 0.0m;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid Frame rate! Only positive or zero values allowed!");
                }
            }
        }

        /// <summary>
        /// The difference of the current frame from the previous one in the corresponding video in %
        /// </summary>
        public decimal DifferencePercentageFromPreviousFrame { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools
{
    public enum VideoFrameRateMode
    {
        CFR,
        VFR
    }

    public class MediaAnalyzeVideoInfo
    {
        /// <summary>
        /// In bytes
        /// </summary>
        public long Size { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// In bps
        /// </summary>
        public int Bitrate { get; set; }

        public string CodecID { get; set; }

        public VideoFrameRateMode FrameRateMode { get; set; }
    }

    public class MediaAnalyzeAudioInfo
    {
        /// <summary>
        /// In bytes
        /// </summary>
        public long Size { get; set; }

        public string Codec { get; set; }

        /// <summary>
        /// In bps
        /// </summary>
        public int Bitrate { get; set; }

        public int Channels { get; set; }
    }

    public class MediaAnalyzeInfo
    {
        public string Filename { get; set; }

        /// <summary>
        /// In bytes
        /// </summary>
        public long Size { get; set; }

        public bool NeedsVideoReencode { get; set; }

        /// <summary>
        /// In bytes
        /// </summary>
        public int TargetVideoBitrate { get; set; }

        public bool NeedsAudioReencode { get; set; }

        /// <summary>
        /// In bytes
        /// </summary>
        public int TargetAudioBitrate { get; set; }

        /// <summary>
        /// In bytes
        /// </summary>
        public long TargetSize { 
            get 
            {
                long targetVideoSize = 0;
                if(VideoInfo != null)
                {
                    if (NeedsVideoReencode && TargetVideoBitrate > 0 && VideoInfo.Bitrate > 0)
                    {
                        targetVideoSize = Convert.ToInt64((double)(VideoInfo.Size * TargetVideoBitrate) / (double)VideoInfo.Bitrate);
                    }
                    else
                    {
                        targetVideoSize = VideoInfo.Size;
                    }
                }

                long targetAudioSize = 0;
                if (AudioInfo != null)
                {
                    if (NeedsAudioReencode && TargetAudioBitrate > 0 && AudioInfo.Bitrate > 0)
                    {
                        targetAudioSize = Convert.ToInt64((double)(AudioInfo.Size * TargetAudioBitrate) / (double)AudioInfo.Bitrate);
                    }
                    else
                    {
                        targetAudioSize = AudioInfo.Size;
                    }
                }

                long containerOverhead = Size - (VideoInfo?.Size ?? 0) - (AudioInfo?.Size ?? 0);

                return targetVideoSize + targetAudioSize + containerOverhead;
            }
        }

        public MediaAnalyzeVideoInfo VideoInfo { get; set; }

        public MediaAnalyzeAudioInfo AudioInfo { get; set; }
    }
}

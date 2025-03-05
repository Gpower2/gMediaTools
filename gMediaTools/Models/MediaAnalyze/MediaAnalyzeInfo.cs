using System;
using System.Collections.Generic;

namespace gMediaTools.Models.MediaAnalyze
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

        public int OriginalWidth { get; set; }

        public int OriginalHeight { get; set; }


        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// In bps
        /// </summary>
        public int Bitrate { get; set; }

        public string CodecID { get; set; }

        /// <summary>
        /// In seconds
        /// </summary>
        public long Length { get; set; }

        public VideoFrameRateMode FrameRateMode { get; set; }

        public string ColorSpace { get; set; }

        public string ChromaSubsampling { get; set; }

        public string Rotation { get; set; }

        public string ScanType { get; set; }

        public string BitDepth { get; set; }
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

        /// <summary>
        /// In seconds
        /// </summary>
        public long Length { get; set; }
    }

    public class MediaAnalyzeInfo
    {
        public string Filename { get; set; }

        public string FileExtension { get; set; }

        public string FileContainerFormat { get; set; }

        /// <summary>
        /// In bytes
        /// </summary>
        public long Size { get; set; }

        public bool NeedsVideoReencode { get; set; }

        public int TargetVideoWidth { get; set; }

        public int TargetVideoHeight { get; set; }

        /// <summary>
        /// In bps
        /// </summary>
        public int TargetVideoBitrate { get; set; }

        public bool NeedsAudioReencode { get; set; }

        /// <summary>
        /// In bps
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

        public double BitrateInKbps
        {
            get 
            {
                return Math.Round(((double)(VideoInfo?.Bitrate ?? 0)) / 1000.0, 3);
            } 
        }

        public double TargetVideoBitrateInKbps
        {
            get
            {
                return Math.Round(((double)TargetVideoBitrate) / 1000.0, 3);
            }
        }

        public double TargetAudioBitrateInKbps
        {
            get
            {
                return Math.Round(((double)TargetAudioBitrate) / 1000.0, 3);
            }
        }

        public double BitrateGainPercentage
        {
            get
            {
                if ((VideoInfo?.Bitrate ?? 0) == 0)
                {
                    return 0;
                }

                return Math.Round(((double)(TargetVideoBitrate - (VideoInfo?.Bitrate ?? 0)) / (double)(VideoInfo?.Bitrate ?? 0)) * 100.0, 2);
            }
        }

        public string PreviewText
        {
            get
            {
                string logText = $"{VideoInfo?.Width}x{VideoInfo?.Height} => {TargetVideoWidth}x{TargetVideoHeight} : {VideoInfo?.CodecID} : {BitrateInKbps:#####0.000} kbps => {TargetVideoBitrateInKbps:#####0.000} kbps ({BitrateGainPercentage}%) ({TimeSpan.FromMilliseconds((int)(AudioInfo?.Length ?? 0))}) {Filename}";

                return logText;
            }
        }

        public MediaAnalyzeVideoInfo VideoInfo { get; set; }

        public MediaAnalyzeAudioInfo AudioInfo { get; set; }

        // Instantiate with a capacity of 10 since usually we don't have more temp files
        public List<string> TempFiles { get; } = new List<string>(10);

        public string FinalOutputFile { get; set; }

        public override string ToString()
        {
            return PreviewText;
        }
    }
}

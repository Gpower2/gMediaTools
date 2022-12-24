using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;
using gMediaTools.Factories;
using gMediaTools.Models.AviSynth;
using gMediaTools.Models.CurveFitting;
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Models.MediaInfo;
using gMediaTools.Services.AviSynth;

namespace gMediaTools.Services.MediaAnalyzer
{
    public class MediaAnalyzerService
    {
        private static readonly string[] _mediaExtensions = new string[] { "mkv", "mp4", "m4v", "mov", "avi", "mpg", "mpeg", "flv", "wmv", "asf", "rm", "ts" };

        private int _reEncodeFiles = 0;
        private int _totalFiles = 0;

        public void AnalyzePath(
            MediaAnalyzePathRequest request,
            CurveFittingSettings curveFittingSettings, 
            MediaAnalyzeActions actions
        )
        {
            // Sanity checks
            if (request.MaxAllowedHeight > request.MaxAllowedWidth)
            {
                throw new ArgumentException(nameof(request), $"{nameof(request.MaxAllowedWidth)} must be always greater than {nameof(request.MaxAllowedHeight)}!");
            }
            
            // Reset counters
            _reEncodeFiles = 0;
            _totalFiles = 0;

            // Get the Data for calculating the CurveFittingFunction
            var data = curveFittingSettings.Data.ToDictionary(
                k => (double)k.Width * k.Height, 
                v => (double)v.Bitrate / (double)(v.Width * v.Height)
            );

            // Get the CurveFitting Function
            var targetFunction = new CurveFittingFactory()
                .GetCurveFittingService(curveFittingSettings.CurveFittingType)
                .GetCurveFittingFunction(data);

            // Call the internal recursive method
            AnalyzePathInternal(request, targetFunction, actions);
        }

        private void AnalyzePathInternal(
            MediaAnalyzePathRequest request,
            Func<double, double> targetFunction,
            MediaAnalyzeActions actions
        )
        {
            // Sanity checks
            if (request.MaxAllowedHeight > request.MaxAllowedWidth)
            {
                throw new ArgumentException(nameof(request), $"{nameof(request.MaxAllowedWidth)} must be always greater than {nameof(request.MaxAllowedHeight)}!");
            }

            var files = Directory.GetFiles(request.MediaDirectoryName);

            var mediaFiles = files
                .Where(f => _mediaExtensions
                    .Any(m => 
                    m.Equals(
                        Path.GetExtension(f).Length > 1 
                        ? Path.GetExtension(f).Substring(1).ToLower()
                        : String.Empty)))
                .OrderBy(f => f)
                .ToList();

            if (mediaFiles.Any())
            {
                foreach (var mediaFile in mediaFiles)
                {
                    _totalFiles++;
                    actions.UpdateProgressAction(_reEncodeFiles, _totalFiles);
                    AnalyzeVideoFileInternal(
                        new MediaAnalyzeFileRequest(mediaFile, request),
                        targetFunction,
                        actions
                    );
                }
            }

            var subDirs = Directory.GetDirectories(request.MediaDirectoryName);

            if (subDirs.Any())
            {
                foreach (var subDir in subDirs.OrderBy(d => d).ToList())
                {
                    AnalyzePathInternal(
                        new MediaAnalyzePathRequest(subDir, request),
                        targetFunction,
                        actions
                    );
                }
            }
        }

        public void AnalyzeVideoFile(
            MediaAnalyzeFileRequest request,
            CurveFittingSettings curveFittingSettings,
            MediaAnalyzeActions actions
        )
        {
            // Sanity checks
            if (request.MaxAllowedHeight > request.MaxAllowedWidth)
            {
                throw new ArgumentException(nameof(request), $"{nameof(request.MaxAllowedWidth)} must be always greater than {nameof(request.MaxAllowedHeight)}!");
            }

            // Get the Data for calculating the CurveFittingFunction
            var data = curveFittingSettings.Data.ToDictionary(
                k => (double)k.Width * k.Height,
                v => (double)v.Bitrate / (double)(v.Width * v.Height)
            );

            // Get the CurveFitting Function
            var targetFunction = new CurveFittingFactory()
                .GetCurveFittingService(curveFittingSettings.CurveFittingType)
                .GetCurveFittingFunction(data);

            // Call the internal recursive method
            AnalyzeVideoFileInternal(request, targetFunction, actions);
        }

        private void AnalyzeVideoFileInternal(
            MediaAnalyzeFileRequest request,
            Func<double, double> targetFunction,
            MediaAnalyzeActions actions
        )
        {
            // Sanity checks
            if (request.MaxAllowedHeight > request.MaxAllowedWidth)
            {
                throw new ArgumentException(nameof(request), $"{nameof(request.MaxAllowedWidth)} must be always greater than {nameof(request.MaxAllowedHeight)}!");
            }

            actions.SetCurrentFileAction(request.MediaFile);

            using (gMediaInfo mi = new gMediaInfo(request.MediaFile))
            {
                if (mi == null)
                {
                    actions.LogErrorAction($"ERROR! {request.MediaFile}");
                    return;
                }

                MediaAnalyzeInfo result = new MediaAnalyzeInfo
                {
                    Filename = request.MediaFile,
                    Size = new FileInfo(request.MediaFile).Length,
                    NeedsVideoReencode = false,
                    NeedsAudioReencode = false
                };

                // Get first General track
                var generalTrack = mi?.GeneralTracks?.FirstOrDefault();
                if (generalTrack == null)
                {
                    actions.LogErrorAction($"ERROR! {request.MediaFile}");
                    return;
                }

                // Get more info
                result.FileExtension = generalTrack.FileExtension;
                result.FileContainerFormat = generalTrack.Format;

                // Get first video track
                var videoTrack = mi?.VideoTracks?.FirstOrDefault();
                if (videoTrack == null)
                {
                    actions.LogErrorAction($"ERROR! {request.MediaFile}");
                    return;
                }

                // Check if we have valid video track info
                if (int.TryParse(videoTrack.Width, out int width)
                   && int.TryParse(videoTrack.Height, out int height)
                   && (
                        int.TryParse(videoTrack.BitRate, out int bitrate)
                        || (videoTrack.BitRate.Contains("/") && int.TryParse(videoTrack.BitRate.Substring(videoTrack.BitRate.IndexOf("/") + 1), out bitrate))
                        || int.TryParse(videoTrack.BitRateNominal, out bitrate)
                        || (videoTrack.BitRateNominal.Contains("/") && int.TryParse(videoTrack.BitRateNominal.Substring(videoTrack.BitRate.IndexOf("/") + 1), out bitrate))
                   )
                )
                {
                    MediaAnalyzeVideoInfo videoResult = new MediaAnalyzeVideoInfo();

                    // Keep the original dimensions
                    int originalWidth = width;
                    int originalHeight = height;

                    // Check for pixel aspect ratio
                    if (videoTrack.PixelAspectRatio.TryParseDecimal(out decimal pixelAspectRatio))
                    {
                        // Check if pixel aspect ration is not equal to 1
                        if (pixelAspectRatio > 1.0m)
                        {
                            // Recalculate width based on the pixel aspect ration
                            width = Convert.ToInt32(Math.Ceiling(width * pixelAspectRatio));
                        }
                    }

                    videoResult.OriginalWidth = originalWidth;
                    videoResult.OriginalHeight = originalHeight;

                    videoResult.Width = width;
                    videoResult.Height = height;
                    videoResult.Bitrate = bitrate;
                    videoResult.CodecID = videoTrack.CodecID;
                    videoResult.Size = long.TryParse(videoTrack.StreamSize, out long streamSize) ? streamSize : default;
                    videoResult.Length = long.TryParse(videoTrack.Duration, out long videoDuration) ? videoDuration : default;
                    // Get the video FrameRate Mode
                    videoResult.FrameRateMode = videoTrack.FrameRateMode.ToLower().Equals("vfr") ? VideoFrameRateMode.VFR : VideoFrameRateMode.CFR;

                    videoResult.ChromaSubsampling = videoTrack.ChromaSubsampling;
                    videoResult.ColorSpace = videoTrack.ColorSpace;

                    videoResult.Rotation = videoTrack.Rotation;

                    videoResult.ScanType = videoTrack.ScanType;
                    videoResult.BitDepth = videoTrack.BitDepth;

                    result.VideoInfo = videoResult;
                }
                else
                {
                    actions.LogErrorAction($"ERROR! {videoTrack.Width}x{videoTrack.Height} : {videoTrack.BitRate} : {videoTrack.CodecID} : {request.MediaFile}");
                    return;
                }

                // Get audio tracks
                var audioTracks = mi?.AudioTracks;

                // Get audio track
                var audioTrack = audioTracks?.FirstOrDefault();

                // Check if we have valid audio track info
                if (audioTrack != null 
                    && int.TryParse(audioTrack.Channels, out int audioChannels)
                    && (int.TryParse(audioTrack.BitRate, out int audioBitrate)
                    || int.TryParse(audioTrack.BitRateNominal, out audioBitrate)))
                {
                    MediaAnalyzeAudioInfo audioResult = new MediaAnalyzeAudioInfo();
                    audioResult.Channels = audioChannels;
                    audioResult.Bitrate = audioBitrate;
                    audioResult.Codec = audioTrack.FormatString;
                    audioResult.Size = long.TryParse(audioTrack.StreamSize, out long audioSize) ? audioSize : default;
                    audioResult.Length = long.TryParse(audioTrack.Duration, out long audioDuration) ? audioDuration : default;

                    result.AudioInfo = audioResult;
                }

                // Check if we need to re encode the video track
                bool isCandidateForVideoReencode = IsCandidateForVideoReencode(width, height, bitrate, request.MaxAllowedWidth, request.MaxAllowedHeight, request.BitratePercentageThreshold, targetFunction, out int targetBitrate, out int targetWidth, out int targetHeight);

                if (isCandidateForVideoReencode)
                {
                    // We only care for lower target bitrate and bitrate greater than the min allowed bitrate!
                    if (targetBitrate < bitrate
                        && targetBitrate > request.MinAllowedBitrate)
                    {
                        // Check if the gain percentage is worth the reencode
                        double gainPercentage = Math.Abs(((double)(targetBitrate - bitrate) / (double)bitrate) * 100.0);
                        if (gainPercentage >= request.GainPercentageThreshold)
                        {
                            // Set that Video needs reencode
                            result.NeedsVideoReencode = true;
                            result.TargetVideoBitrate = targetBitrate;
                            result.TargetVideoWidth = targetWidth;
                            result.TargetVideoHeight = targetHeight;

                            _reEncodeFiles++;

                            actions.UpdateProgressAction(_reEncodeFiles, _totalFiles);
                            actions.HandleMediaForReencodeAction(result);

                            // Check for no audio, or more than one audio
                            if (audioTracks == null || !audioTracks.Any() || audioTracks.Count() > 1)
                            {
                                // No audio tracks, nothing to do here
                                return;
                            }

                            // Sanity check, we should have an AudioInfo
                            if (result?.AudioInfo == null)
                            {
                                // No audio info found, nothing to do here
                                return;
                            }

                            // Check if we need to re encode the audio track
                            bool isWindowsMedia = result.FileContainerFormat.ToLower().Trim().Equals("windows media");
                            bool isCandidateForAudioReencode = IsCandidateForAudioReencode(isWindowsMedia, result.AudioInfo.Channels, result.AudioInfo.Bitrate, out int targetAudioBitrate);

                            if (isCandidateForAudioReencode)
                            {
                                result.NeedsAudioReencode = true;
                                result.TargetAudioBitrate = targetAudioBitrate;
                            }
                        }
                    }
                }
            }
        }

        private bool IsCandidateForVideoReencode(int width, int height, int bitrate, int maxWidth, int maxHeight, double percentageThreshold, Func<double, double> targetFunction, out int targetBitrate, out int targetWidth, out int targetHeight)
        {
            bool needResize = false;

            long pixels = width * height;

            // Initialize out variables
            // and recalculate to closest mod 2 value
            targetWidth = width.ClosestModValue(2);
            targetHeight = height.ClosestModValue(2);

            // First check if we need to resize it
            long maxPixels = maxWidth * maxHeight;
            if (pixels > maxPixels)
            {
                // We need to resize
                needResize = true;

                // a * b > a' * b'
                // a > a' * b' / b => b > b'
                // b > b' * a' / a => a > a'

                // Find the long dimension (width or height)
                if (height > width)
                {
                    // probably rotated video, use maxWidth as maxHeight
                    if (height > maxWidth)
                    {
                        // Height exeeds allowed value
                        targetHeight = maxWidth;
                        targetWidth = Convert.ToInt32(targetHeight * (double)width / height);

                        // Check if the target width still exceeds allowed value
                        if (targetWidth > maxHeight)
                        {
                            // Calculate based on max allowed width
                            targetWidth = maxHeight;
                            targetHeight = Convert.ToInt32(targetWidth * (double)height / width);
                        }
                    }
                    else
                    {
                        // Width exceeds allowed value
                        targetWidth = maxHeight;
                        targetHeight = Convert.ToInt32(targetWidth * (double)height / width);

                        // Check if the target height still exceeds allowed value
                        if (targetHeight > maxWidth)
                        {
                            // Calculate based on max allowed height
                            targetHeight = maxWidth;
                            targetWidth = Convert.ToInt32(targetHeight * (double)width / height);
                        }
                    }
                }
                else
                {
                    // normal video
                    // probably rotated video, use maxWidth as maxHeight
                    if (height > maxHeight)
                    {
                        // Height exeeds allowed value
                        targetHeight = maxHeight;
                        targetWidth = Convert.ToInt32(targetHeight * (double)width / height);

                        // Check if the target width still exceeds allowed value
                        if (targetWidth > maxWidth)
                        {
                            // Calculate based on max allowed width
                            targetWidth = maxWidth;
                            targetHeight = Convert.ToInt32(targetWidth * (double)height / width);
                        }
                    }
                    else
                    {
                        // Width exceeds allowed value
                        targetWidth = maxWidth;
                        targetHeight = Convert.ToInt32(targetWidth * (double)height / width);

                        // Check if the target height still exceeds allowed value
                        if (targetHeight > maxHeight)
                        {
                            // Calculate based on max allowed height
                            targetHeight = maxHeight;
                            targetWidth = Convert.ToInt32(targetHeight * (double)width / height);
                        }
                    }
                }

                // Recaluclate to closest mod 2 value
                targetWidth = targetWidth.ClosestModValue(2);
                targetHeight = targetHeight.ClosestModValue(2);

                // Recalculate pixels
                pixels = targetWidth * targetHeight;
            }

            // Sanity Check!
            if (pixels > maxPixels)
            {
                throw new Exception($"Something went wrong calculating new video resolution!{Environment.NewLine}original resolution : {width}x{height}{Environment.NewLine}new resolution : {targetWidth}x{targetHeight}{Environment.NewLine}max allowed resolution : {maxWidth}x{maxHeight}");
            }

            var targetRatio = targetFunction(pixels);

            targetBitrate = Convert.ToInt32(pixels * targetRatio);

            double minPercentage = 1.0 - (percentageThreshold / 100.0);
            double maxPercentage = 1.0 + (percentageThreshold / 100.0);

            return needResize || (bitrate < minPercentage * targetBitrate || bitrate > maxPercentage * targetBitrate);
        }

        private bool IsCandidateForAudioReencode(bool isWindowsMedia, int audioChannels, int audioBitrate, out int targetAudioBitrate)
        {            
            if (audioChannels == 1)
            {
                targetAudioBitrate = 64000;
                return isWindowsMedia || audioBitrate > targetAudioBitrate * 1.1;
            }
            else if (audioChannels == 2)
            {
                targetAudioBitrate = 128000;
                return isWindowsMedia || audioBitrate > targetAudioBitrate * 1.1;
            }
            else if(audioChannels > 2)
            {
                targetAudioBitrate = 384000;
                return isWindowsMedia || audioBitrate > targetAudioBitrate * 1.1;
            }
            else
            {
                // 0 channels, nothing to do
                targetAudioBitrate = 0;
                return false;
            }
        }
    }
}

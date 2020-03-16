using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private static readonly string[] _mediaExtensions = new string[] { "mkv", "mp4", "mov", "avi", "mpg", "mpeg", "flv", "wmv", "asf" };

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

            var mediaFiles = files.Where(f => _mediaExtensions.Any(m => m.Equals(Path.GetExtension(f).Substring(1).ToLower()))).ToList();
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
                foreach (var subDir in subDirs)
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
                   && int.TryParse(videoTrack.BitRate, out int bitrate))
                {
                    MediaAnalyzeVideoInfo videoResult = new MediaAnalyzeVideoInfo();
                    videoResult.Width = width;
                    videoResult.Height = height;
                    videoResult.Bitrate = bitrate;
                    videoResult.CodecID = videoTrack.CodecID;
                    videoResult.Size = long.TryParse(videoTrack.StreamSize, out long streamSize) ? streamSize : default;
                    // Get the video FrameRate Mode
                    videoResult.FrameRateMode = videoTrack.FrameRateMode.ToLower().Equals("vfr") ? VideoFrameRateMode.VFR : VideoFrameRateMode.CFR;

                    videoResult.ChromaSubsampling = videoTrack.ChromaSubsampling;
                    videoResult.ColorSpace = videoTrack.ColorSpace;

                    result.VideoInfo = videoResult;
                }
                else
                {
                    actions.LogErrorAction($"ERROR! {width}x{videoTrack.Height} : {videoTrack.BitRate} : {videoTrack.CodecID} : {request.MediaFile}");
                    return;
                }

                // Get first audio track
                var audioTrack = mi?.AudioTracks?.FirstOrDefault();
                if (audioTrack != null)
                {
                    // Check if we have valid audio track info
                    if (int.TryParse(audioTrack.Channels, out int channels)
                        && int.TryParse(audioTrack.BitRate, out int audioBitrate))
                    {
                        MediaAnalyzeAudioInfo audioResult = new MediaAnalyzeAudioInfo();
                        audioResult.Channels = channels;
                        audioResult.Bitrate = audioBitrate;
                        audioResult.Codec = audioTrack.FormatString;
                        audioResult.Size = long.TryParse(audioTrack.StreamSize, out long audioSize) ? audioSize : default;

                        result.AudioInfo = audioResult;
                    }
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

                            ServiceFactory.GetService<AviSynthScriptService>().CreateAviSynthScript(result);
                        }
                    }
                }

                // TODO: Check if we need to re encode the audio track
            }
        }

        private bool IsCandidateForVideoReencode(int width, int height, int bitrate, int maxWidth, int maxHeight, double percentageThreshold, Func<double, double> targetFunction, out int targetBitrate, out int targetWidth, out int targetHeight)
        {
            bool needResize = false;

            long pixels = width * height;

            // Initialize out variables
            targetWidth = width;
            targetHeight = height;

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
                    }
                    else
                    {
                        // Width exceeds allowed value
                        targetWidth = maxHeight;
                        targetHeight = Convert.ToInt32(targetWidth * (double)height / width);
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
                    }
                    else
                    {
                        // Width exceeds allowed value
                        targetWidth = maxWidth;
                        targetHeight = Convert.ToInt32(targetWidth * (double)height / width);
                    }
                }

                // Recalculate pixels
                pixels = targetWidth * targetHeight;
            }

            // Sanity Check!
            if (pixels > maxPixels)
            {
                throw new Exception($"Something went wrong calculating new video resolution! original resolution : {width}x{height} new resolution : {targetWidth}x{targetHeight} max allowed resolution : {maxWidth}x{maxHeight}");
            }

            var targetRatio = targetFunction(pixels);

            targetBitrate = Convert.ToInt32(pixels * targetRatio);

            double minPercentage = 1.0 - (percentageThreshold / 100.0);
            double maxPercentage = 1.0 + (percentageThreshold / 100.0);

            return needResize || (bitrate < minPercentage * targetBitrate || bitrate > maxPercentage * targetBitrate);
        }
    }
}

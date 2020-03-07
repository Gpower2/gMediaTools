﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services
{
    public class MediaAnalyzerService
    {
        private static readonly string[] _mediaExtensions = new string[] { "mkv", "mp4", "mov", "avi", "mpg", "mpeg", "flv", "wmv" };

        private int _reEncodeFiles = 0;
        private int _totalFiles = 0;

        public void AnalyzePath(
            MediaAnalyzePathRequest request,
            CurveFittingSettings curveFittingSettings, 
            MediaAnalyzeActions actions
        )
        {
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
            var files = Directory.GetFiles(request.MediaDirectoryName);

            var mediaFiles = files.Where(f => _mediaExtensions.Any(m => m.Equals(Path.GetExtension(f).Substring(1).ToLower()))).ToList();
            if (mediaFiles.Any())
            {
                foreach (var mediaFile in mediaFiles)
                {
                    _totalFiles++;
                    actions.UpdateProgressAction(_reEncodeFiles, _totalFiles);
                    AnalyzeVideoFileInternal(
                        new MediaAnalyzeFileRequest 
                        { 
                             MediaFile = mediaFile,
                             BitratePercentageThreshold = request.BitratePercentageThreshold,
                             GainPercentageThreshold = request.GainPercentageThreshold
                        },
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
                        new MediaAnalyzePathRequest 
                        { 
                            MediaDirectoryName = subDir,
                            BitratePercentageThreshold = request.BitratePercentageThreshold,
                            GainPercentageThreshold = request.GainPercentageThreshold
                        },
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
            actions.SetCurrentFileAction(request.MediaFile);

            using (MediaInfo.gMediaInfo mi = new MediaInfo.gMediaInfo(request.MediaFile))
            {
                var videoTrack = mi?.VideoTracks?.FirstOrDefault();
                if (videoTrack == null)
                {
                    actions.LogLineAction($"ERROR! {request.MediaFile}");
                }

                if (int.TryParse(videoTrack.Width, out int width)
                   && int.TryParse(videoTrack.Height, out int height)
                   && int.TryParse(videoTrack.BitRate, out int bitrate))
                {
                    if (NeedsReencode(width, height, bitrate, request.BitratePercentageThreshold, targetFunction, out int targetBitrate))
                    {
                        if (targetBitrate < bitrate)
                        {
                            // Check if the gain percentage is worth the reencode
                            double gainPercentage = Math.Abs(((double)(targetBitrate - bitrate) / (double)bitrate) * 100.0);
                            if (gainPercentage >= request.GainPercentageThreshold)
                            {
                                _reEncodeFiles++;
                                actions.UpdateProgressAction(_reEncodeFiles, _totalFiles);
                                actions.LogLineAction($"{width}x{height} : {videoTrack.CodecID} : {Math.Round(((double)bitrate) / 1000.0, 3):#####0.000} => {Math.Round(((double)targetBitrate) / 1000.0, 3):#####0.000} ({Math.Round(((double)(targetBitrate - bitrate) / (double)bitrate) * 100.0, 2)}%) {request.MediaFile}");
                            }
                        }
                    }
                }
                else
                {
                    actions.LogLineAction($"ERROR! {width}x{videoTrack.Height} : {videoTrack.BitRate} : {videoTrack.CodecID} : {request.MediaFile}");
                }
            }
        }

        private bool NeedsReencode(int width, int height, int bitrate, double percentageThreshold, Func<double, double> targetFunction, out int targetBitrate)
        {
            long pixels = width * height;

            var targetRatio = targetFunction(pixels);

            targetBitrate = Convert.ToInt32(pixels * targetRatio);

            double minPercentage = 1.0 - (percentageThreshold / 100.0);
            double maxPercentage = 1.0 + (percentageThreshold / 100.0);

            return (bitrate < minPercentage * targetBitrate || bitrate > maxPercentage * targetBitrate);
        }
    }
}

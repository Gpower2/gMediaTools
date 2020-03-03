using System;
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
            CurveFittingSettings curveFittingSettings, 
            string currentDir, 
            double bitratePercentageThreshold, 
            double gainPercentageThreshold,
            Action<string> setCurrentFileAction,
            Action<string> logLineAction,
            Action<int, int> updateProgressAction
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
            AnalyzePathInternal(currentDir, bitratePercentageThreshold, gainPercentageThreshold, targetFunction, setCurrentFileAction, logLineAction, updateProgressAction);
        }

        private void AnalyzePathInternal(
            string currentDir, 
            double bitratePercentageThreshold, 
            double gainPercentageThreshold, 
            Func<double, double> targetFunction,
            Action<string> setCurrentFileAction,
            Action<string> logLineAction,
            Action<int, int> updateProgressAction)
        {
            var files = Directory.GetFiles(currentDir);
            var mediaFiles = files.Where(f => _mediaExtensions.Any(m => m.Equals(Path.GetExtension(f).Substring(1).ToLower()))).ToList();
            if (mediaFiles.Any())
            {
                foreach (var mediaFile in mediaFiles)
                {
                    _totalFiles++;
                    updateProgressAction(_reEncodeFiles, _totalFiles);
                    AnalyzeVideoFile(mediaFile, bitratePercentageThreshold, gainPercentageThreshold, targetFunction, setCurrentFileAction, logLineAction, updateProgressAction);
                }
            }

            var subDirs = Directory.GetDirectories(currentDir);

            if (subDirs.Any())
            {
                foreach (var subDir in subDirs)
                {
                    AnalyzePathInternal(subDir, bitratePercentageThreshold, gainPercentageThreshold, targetFunction, setCurrentFileAction, logLineAction, updateProgressAction);
                }
            }
        }

        public void AnalyzeVideoFile(string mediaFilename, double bitratePercentageThreshold, double gainPercentageThreshold, Func<double, double> targetFunction,
            Action<string> setCurrentFileAction,
            Action<string> logLineAction,
            Action<int, int> updateProgressAction)
        {
            setCurrentFileAction(mediaFilename);
            using (MediaInfo.gMediaInfo mi = new MediaInfo.gMediaInfo(mediaFilename))
            {
                var videoTrack = mi.Video.FirstOrDefault();
                if (videoTrack == null)
                {
                    return;
                }
                string bitrate = videoTrack.BitRate;
                string width = videoTrack.Width;
                string height = videoTrack.Height;
                string frameRate = videoTrack.FrameRate;
                string videoCodec = videoTrack.CodecID;

                if (Int32.TryParse(width, out int widthInt)
                   && Int32.TryParse(height, out int heightInt)
                   && Int32.TryParse(bitrate, out int bitrateInt))
                {
                    if (NeedsReencode(Convert.ToInt32(widthInt), Convert.ToInt32(heightInt), Convert.ToInt32(bitrateInt), bitratePercentageThreshold, targetFunction, out int targetBitrate))
                    {
                        if (targetBitrate < bitrateInt)
                        {
                            // Check if the gain percentage is worth the reencode
                            double gainPercentage = Math.Abs(((double)(targetBitrate - bitrateInt) / (double)bitrateInt) * 100.0);
                            if (gainPercentage >= gainPercentageThreshold)
                            {
                                _reEncodeFiles++;
                                updateProgressAction(_reEncodeFiles, _totalFiles);
                                logLineAction($"{width}x{height} : {videoCodec} : {Math.Round(((double)bitrateInt) / 1000.0, 3):#####0.000} => {Math.Round(((double)targetBitrate) / 1000.0, 3):#####0.000} ({Math.Round(((double)(targetBitrate - bitrateInt) / (double)bitrateInt) * 100.0, 2)}%) {mediaFilename}");
                            }
                        }
                    }
                }
                else
                {
                    logLineAction($"ERROR! {width}x{height} : {bitrate} : {videoCodec} : {mediaFilename}");
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

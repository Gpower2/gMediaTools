using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gMediaTools.Services;

namespace gMediaTools
{
    public partial class FrmMain : BaseForm
    {
        private string[] _mediaExtensions = new string[] { "mkv", "mp4", "mov", "avi", "mpg", "mpeg", "flv", "wmv" };
        private int _reEncodeFiles = 0;
        private int _totalFiles = 0;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnScanMediaFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtInputFolder.Text))
                {
                    throw new Exception("Empty path!");
                }
                _reEncodeFiles = 0;
                _totalFiles = 0;

                string rootPath = txtInputFolder.Text;
                if (!Directory.Exists(txtInputFolder.Text))
                {
                    rootPath = Path.GetDirectoryName(txtInputFolder.Text);
                }

                txtLog.Clear();

                AnalyzePath(rootPath, 10, 20);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void AnalyzePath(string currentDir, double bitratePercentageThreshold, double gainPercentageThreshold, Func<double, double> targetFunction = null)
        {
            var files = Directory.GetFiles(currentDir);
            var mediaFiles = files.Where(f => _mediaExtensions.Any(m => m.Equals(Path.GetExtension(f).Substring(1).ToLower()))).ToList();
            if (mediaFiles.Any())
            {
                // Get the Function for the target bitrate, if not provided
                if (targetFunction == null)
                {
                    // Get the Data for calculating the CurveFittingFunction
                    var initData = new Dictionary<double, double> {
                        { 640*480, 1200 },
                        { 848*480, 1500 },
                        { 1280*720, 2500 },
                        { 1920*1080, 4000 }
                    };

                    var data = initData.ToDictionary(k => k.Key, v => v.Value * 1000 / v.Key);

                    // Get the CurveFitting Function
                    targetFunction = new CurveFittingFactory().GetCurveFittingService(CurveFittingType.Logarithmic)
                        .GetCurveFittingFunction(data);
                }

                foreach (var mediaFile in mediaFiles)
                {
                    _totalFiles++;
                    Log(_reEncodeFiles, _totalFiles);
                    MediaAnalyze(mediaFile, bitratePercentageThreshold, gainPercentageThreshold, targetFunction);
                    Application.DoEvents();
                }
            }

            var subDirs = Directory.GetDirectories(currentDir);

            if (subDirs.Any())
            {
                foreach (var subDir in subDirs)
                {
                    AnalyzePath(subDir, bitratePercentageThreshold, gainPercentageThreshold, targetFunction);
                }
            }
        }

        private void SetCurrentFile(string currentFile)
        {
            this.Invoke((MethodInvoker)(() => { this.txtCurrentFile.Text = currentFile; }));
        }
        
        private void Log(string logText)
        {
            this.Invoke((MethodInvoker)(() => { this.txtLog.AppendText(logText + Environment.NewLine); }));
        }

        private void Log(int reencodeFiles, int totalFiles)
        {
            this.Invoke((MethodInvoker)(() => { this.txtFilesProgress.Text = $"{reencodeFiles}/{totalFiles} ({Math.Round((double)reencodeFiles / (double)totalFiles * 100.0, 2)}%)"; }));
        }

        private void MediaAnalyze(string mediaFilename, double bitratePercentageThreshold, double gainPercentageThreshold, Func<double, double> targetFunction)
        {
            SetCurrentFile(mediaFilename);
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
                                Log(_reEncodeFiles, _totalFiles);
                                Log($"{width}x{height} : {videoCodec} : {Math.Round(((double)bitrateInt) / 1000.0, 3):#####0.000} => {Math.Round(((double)targetBitrate) / 1000.0, 3):#####0.000} ({Math.Round(((double)(targetBitrate - bitrateInt) / (double)bitrateInt) * 100.0, 2)}%) {mediaFilename}");
                            }
                        }
                    }
                }
                else
                {
                    Log($"ERROR! {width}x{height} : {bitrate} : {videoCodec} : {mediaFilename}");
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

        private void txtInputFolder_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                // check if the drop data is actually a file or folder
                if (e != null && e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    // Get the Dropped Data
                    String[] s = (String[])e.Data.GetData(DataFormats.FileDrop, false);
                    // Check if we have valid Data and that the specified File Data actually exists
                    if (s != null && s.Length > 0 && (Directory.Exists(s[0]) || File.Exists(s[0])))
                    {
                        e.Effect = DragDropEffects.All;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ShowExceptionMessage(ex);
            }
        }

        private void txtInputFolder_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                // check if the drop data is actually a file or folder
                if (e != null && e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    String[] s = (String[])e.Data.GetData(DataFormats.FileDrop, false);
                    // Check if we have valid Data and that the specified File Data actually exists
                    if (s != null && s.Length > 0 && (Directory.Exists(s[0])|| File.Exists(s[0])))
                    {
                        string finalText = s[0];
                        if (File.Exists(s[0]))
                        {
                            finalText = Path.GetDirectoryName(finalText);
                        }
                        (sender as Control).Text = finalText;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ShowExceptionMessage(ex);
            }
        }
    }
}

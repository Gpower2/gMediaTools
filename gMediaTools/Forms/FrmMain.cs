﻿using System;
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
using gMediaTools.Models.Encoder;
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Models.Muxer;
using gMediaTools.Services;
using gMediaTools.Services.CurveFitting;
using gMediaTools.Services.Encoder;
using gMediaTools.Services.MediaAnalyzer;
using gMediaTools.Services.Muxer;

namespace gMediaTools.Forms
{
    public partial class FrmMain : BaseForm
    {
        private readonly CurveFittingRepository _curveFittingRepo = new CurveFittingRepository();

        private readonly MediaAnalyzerService _mediaAnalyzerService = new MediaAnalyzerService();

        public FrmMain()
        {
            InitializeComponent();

            // Default values
            txtBitratePercentageThreshold.Int32Value = 10;
            txtGainPercentageThreshold.Int32Value = 20;
            txtMaxAllowedWidth.Int32Value = 1280;
            txtMaxAllowedHeight.Int32Value = 720;
            txtMinAllowedBitrate.Int32Value = 700;
        }

        private void BtnScanMediaFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtInputFolder.Text))
                {
                    throw new Exception("Empty path!");
                }

                string rootPath = txtInputFolder.Text;
                if (!Directory.Exists(txtInputFolder.Text))
                {
                    if (!File.Exists(txtInputFolder.Text))
                    {
                        throw new Exception($"Invalid directory '{txtInputFolder.Text}'!");
                    }

                    rootPath = Path.GetDirectoryName(txtInputFolder.Text);
                }

                txtLog.Clear();
                lstMediaInfoItems.Items.Clear();

                // Get the CurveFittingSettings for calculating the CurveFittingFunction
                var curveSettings = _curveFittingRepo.GetCurveFittingSettings();

                List<MediaAnalyzeInfo> mediaToReencode = new List<MediaAnalyzeInfo>();

                _mediaAnalyzerService.AnalyzePath(
                    new MediaAnalyzePathRequest
                    {
                        MediaDirectoryName = rootPath,
                        BitratePercentageThreshold = txtBitratePercentageThreshold.Int32Value,
                        GainPercentageThreshold = txtGainPercentageThreshold.Int32Value,
                        MaxAllowedWidth = txtMaxAllowedWidth.Int32Value,
                        MaxAllowedHeight = txtMaxAllowedHeight.Int32Value,
                        MinAllowedBitrate = txtMinAllowedBitrate.Int32Value * 1000
                    },
                    curveSettings,
                    new MediaAnalyzeActions
                    {
                        SetCurrentFileAction = (string currentFile) =>
                           {
                               this.Invoke((MethodInvoker)(() => { this.txtCurrentFile.Text = currentFile; }));
                               Application.DoEvents();
                           },
                        LogErrorAction = (string logText) =>
                             {
                                 this.Invoke((MethodInvoker)(() => { this.txtLog.AppendText(logText + Environment.NewLine); }));
                                 Application.DoEvents();
                             },
                        UpdateProgressAction = (int reencodeFiles, int totalFiles) =>
                             {
                                 this.Invoke((MethodInvoker)(() => { this.txtFilesProgress.Text = $"{reencodeFiles}/{totalFiles} ({Math.Round((double)reencodeFiles / (double)totalFiles * 100.0, 2)}%)"; }));
                                 Application.DoEvents();
                             },
                        HandleMediaForReencodeAction = (MediaAnalyzeInfo info) =>
                        {
                            mediaToReencode.Add(info);

                            this.Invoke((MethodInvoker)(() => { this.txtLog.AppendText(info.PreviewText + Environment.NewLine); }));
                            Application.DoEvents();
                        }
                    }
                );

                lstMediaInfoItems.Items.AddRange(mediaToReencode.ToArray());
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
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
                ShowExceptionMessage(ex);
            }
        }

        private void btnResolutionBitrate_Click(object sender, EventArgs e)
        {
            FrmResolutionBitrateEditor frm = new FrmResolutionBitrateEditor();
            frm.ShowDialog(this);
        }

        private string GetMediaInfoAnalysis(MediaAnalyzeInfo mediaInfo)
        {
            if(mediaInfo == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{nameof(mediaInfo.Filename)}: {mediaInfo.Filename}");
            sb.AppendLine("######################");
            sb.AppendLine($"Resolution: {mediaInfo.VideoInfo?.Width} x {mediaInfo.VideoInfo?.Height}");
            sb.AppendLine($"{nameof(mediaInfo.VideoInfo.Bitrate)}: {mediaInfo.VideoInfo?.Bitrate}");
            sb.AppendLine($"{nameof(mediaInfo.VideoInfo.Length)}: {new TimeSpan(0, 0, 0, 0, (int)mediaInfo.VideoInfo?.Length)}");
            sb.AppendLine($"{nameof(mediaInfo.VideoInfo.CodecID)}: {mediaInfo.VideoInfo?.CodecID}");
            sb.AppendLine($"{nameof(mediaInfo.VideoInfo.FrameRateMode)}: {mediaInfo.VideoInfo?.FrameRateMode}");
            sb.AppendLine("######################");
            sb.AppendLine($"{nameof(mediaInfo.NeedsVideoReencode)}: {mediaInfo.NeedsVideoReencode}");
            sb.AppendLine($"TargetResolution: {mediaInfo.TargetVideoWidth} x {mediaInfo.TargetVideoHeight}");
            sb.AppendLine($"{nameof(mediaInfo.TargetVideoBitrate)}: {mediaInfo.TargetVideoBitrate}");
            if (mediaInfo.AudioInfo != null)
            {
                sb.AppendLine("######################");
                sb.AppendLine($"{nameof(mediaInfo.NeedsAudioReencode)}: {mediaInfo.NeedsAudioReencode}");
                sb.AppendLine($"{nameof(mediaInfo.TargetAudioBitrate)}: {mediaInfo.TargetAudioBitrate}");
                sb.AppendLine($"{nameof(mediaInfo.AudioInfo.Length)}: {new TimeSpan(0, 0, 0, 0, (int)mediaInfo.AudioInfo?.Length)}");
            }
            sb.AppendLine("######################");
            sb.AppendLine($"{nameof(mediaInfo.Size)}: {Math.Round((double)mediaInfo.Size / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} MB");
            sb.AppendLine($"{nameof(mediaInfo.TargetSize)}: {Math.Round((double)mediaInfo.TargetSize / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} MB");

            return sb.ToString();
        }

        private void lstMediaInfoItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    txtMediaInfo.Clear();
                    return;
                }

                var mediaInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;

                if (mediaInfo == null)
                {
                    txtMediaInfo.Clear();
                    return;
                }

                txtMediaInfo.Text = GetMediaInfoAnalysis(mediaInfo);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void EncodeLog(string log)
        {
            Debug.WriteLine(log);
            this.Invoke(new Action(() => txtEncodeLog.Text = log));
        }

        private void EncodeProgress(string progress)
        {
            Debug.WriteLine(progress);
            this.Invoke(new Action(() => txtEncodeLogProgress.Text = progress));
        }

        private async Task<string> EncodeMediaInfoAsync(MediaAnalyzeInfo mediaInfo)
        {
            if (mediaInfo == null)
            {
                txtMediaInfo.Clear();
                return "";
            }

            // Sanity check
            if (!mediaInfo.NeedsVideoReencode && !mediaInfo.NeedsAudioReencode)
            {
                return "";
            }

            // Log
            txtEncodeLog.Text = $"Start Encoding: {mediaInfo.Filename}";

            txtMediaInfo.Text = GetMediaInfoAnalysis(mediaInfo);

            string videoOutputFileName = "";

            // Encode video
            if (mediaInfo.NeedsVideoReencode)
            {
                txtEncodeLog.Text = $"Video Encoding: {mediaInfo.Filename}...";

                X264VideoEncoderService videoEncoderService = ServiceFactory.GetService<X264VideoEncoderService>();

                int res = await Task.Run(() => videoEncoderService.Encode(
                    mediaInfo, 
                    @"E:\Programs\MeGUI\tools\x264\x264.exe",
                    new Action<string>((log) => EncodeLog(log)),
                    new Action<string>((progress) => EncodeProgress(progress)),
                    out videoOutputFileName)
                );

                if (res != 0)
                {
                    txtEncodeLog.Text = $"Video Encoder failed for {mediaInfo.Filename}! Exit code : {res}";
                    return "";
                }
            }

            string audioOutputFileName = "";

            // Encode Audio
            if (mediaInfo.NeedsAudioReencode)
            {
                txtEncodeLog.Text = $"Audio Encoding: {mediaInfo.Filename}...";

                AudioEncoderService audioEncoderService = ServiceFactory.GetService<AudioEncoderService>();

                NeroAacAudioEncoder audioEncoder = new NeroAacAudioEncoder(@"E:\Programs\MeGUI\tools\eac3to\neroAacEnc.exe");

                DefaultAudioEncoderSettings audioEncoderSettings = new DefaultAudioEncoderSettings(-1, "m4a");

                int res = await Task.Run(() => audioEncoderService.Encode(
                    mediaInfo, 
                    audioEncoder, 
                    audioEncoderSettings,
                    new Action<string>((log) => EncodeLog(log)),
                    new Action<string>((progress) => EncodeProgress(progress)),
                    out audioOutputFileName)
                );

                if (res != 0)
                {
                    txtEncodeLog.Text = $"Audio Encoder failed for {mediaInfo.Filename}! Exit code : {res}";
                    return "";
                }
            }

            // Mux final video!
            txtEncodeLog.Text = $"Muxing: {mediaInfo.Filename}...";

            DefaultMuxerSettings muxerSettings = new DefaultMuxerSettings(
                videoOutputFileName,
                mediaInfo.NeedsAudioReencode ? audioOutputFileName : mediaInfo.Filename,
                "mkv"
            );

            MkvMergeMuxer muxer = new MkvMergeMuxer(@"C:\Program Files\MKVToolNix\mkvmerge.exe");

            MkvMergeMuxerService mkvMergeMuxerService = new MkvMergeMuxerService();

            string muxedFilename = "";
            await Task.Run(() => mkvMergeMuxerService.Mux(
                muxer, 
                muxerSettings,
                new Action<string>((log) => EncodeLog(log)),
                new Action<string>((progress) => EncodeProgress(progress)),
                out muxedFilename)
            );

            return muxedFilename;
        }

        private async void btnEncode_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    txtMediaInfo.Clear();
                    return;
                }

                var mediaInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;

                string muxedFilename = await EncodeMediaInfoAsync(mediaInfo);

                // Log
                txtEncodeLog.Text = $"Muxed {mediaInfo.Filename} => {muxedFilename}";
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private async void btnEncodeAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.Items.Count == 0)
                {
                    txtMediaInfo.Clear();
                    return;
                }

                lstMediaInfoItems.Enabled = false;
                btnEncode.Enabled = false;
                btnEncodeAll.Enabled = false;
                BtnScanMediaFiles.Enabled = false;

                int i = 0;

                foreach (var item in lstMediaInfoItems.Items)
                {
                    i++;
                    txtEncodeProgress.Text = $"{i}/{lstMediaInfoItems.Items.Count}";

                    var mediaInfo = item as MediaAnalyzeInfo;

                    string muxedFilename = await EncodeMediaInfoAsync(mediaInfo);

                    // Log
                    txtEncodeLog.Text = $"Muxed {mediaInfo.Filename} => {muxedFilename}";
                }

                lstMediaInfoItems.Enabled = true;
                btnEncode.Enabled = true;
                btnEncodeAll.Enabled = true;
                BtnScanMediaFiles.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);

                lstMediaInfoItems.Enabled = true;
                btnEncode.Enabled = true;
                btnEncodeAll.Enabled = true;
                BtnScanMediaFiles.Enabled = true;
            }
        }
    }
}

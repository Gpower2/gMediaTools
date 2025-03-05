using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gMediaTools.Extensions;
using gMediaTools.Models;
using gMediaTools.Models.Encoder;
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Models.Muxer;
using gMediaTools.Services;
using gMediaTools.Services.CurveFitting;
using gMediaTools.Services.Encoder;
using gMediaTools.Services.FormState;
using gMediaTools.Services.MediaAnalyzer;
using gMediaTools.Services.Muxer;

namespace gMediaTools.Forms
{
    public partial class FrmMain : BaseForm
    {
        private readonly CurveFittingRepository _curveFittingRepo = new CurveFittingRepository();

        private readonly MediaAnalyzerService _mediaAnalyzerService = new MediaAnalyzerService();

        private readonly FormStateRepository _formStateRepository = new FormStateRepository();

        private bool _ignoreEvents = false;

        public FrmMain()
        {
            try
            {
                _ignoreEvents = true;

                InitializeComponent();

                // Get Form State
                var formStateInfo = _formStateRepository.GetFormStateInfo();

                SetFormState(formStateInfo);

                _ignoreEvents = false;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);

                _ignoreEvents = false;
            }
        }

        private void SetFormState(FormStateInfo formStateInfo)
        {
            txtInputFolder.Text = formStateInfo.InputFolder;

            txtBitratePercentageThreshold.Int32Value = formStateInfo.BitratePercentageThreshold;
            txtGainPercentageThreshold.Int32Value = formStateInfo.GainPercentageThreshold;
            txtMaxAllowedWidth.Int32Value = formStateInfo.MaxAllowedWidth;
            txtMaxAllowedHeight.Int32Value = formStateInfo.MaxAllowedHeight;
            txtMinAllowedBitrate.Int32Value = formStateInfo.MinAllowedBitrate;

            lstMediaInfoItems.Items.AddRange(formStateInfo.MediaAnalyzeInfos.ToArray());
            UpdateListCounter();
        }

        private FormStateInfo GetCurrentFormStateInfo()
        {
            return new FormStateInfo
            {
                InputFolder = txtInputFolder.Text,

                BitratePercentageThreshold = txtBitratePercentageThreshold.Int32Value,
                GainPercentageThreshold = txtGainPercentageThreshold.Int32Value,
                MaxAllowedWidth = txtMaxAllowedWidth.Int32Value,
                MaxAllowedHeight = txtMaxAllowedHeight.Int32Value,
                MinAllowedBitrate = txtMinAllowedBitrate.Int32Value,

                MediaAnalyzeInfos = lstMediaInfoItems.Items.Cast<MediaAnalyzeInfo>()
            };
        }

        private void UpdateCurrentFormState()
        {
            if (!_ignoreEvents)
            {
                _formStateRepository.SaveFormStateInfo(GetCurrentFormStateInfo());
            }
            UpdateListCounter();
        }

        private void UpdateListCounter()
        {
            grpItems.Text = $"Items ({lstMediaInfoItems.Items.Count})";
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
                UpdateCurrentFormState();

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
                UpdateCurrentFormState();
            }
            catch (Exception ex)
            {
                UpdateCurrentFormState();
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
                sb.AppendLine($"{nameof(mediaInfo.AudioInfo.Length)}: {new TimeSpan(0, 0, 0, 0, (int)(mediaInfo.AudioInfo?.Length ?? 0))}");
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
                    txtOutputFilename.Clear();
                    lstFiles.Items.Clear();
                    return;
                }

                var mediaInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;

                if (mediaInfo == null)
                {
                    txtMediaInfo.Clear();
                    txtOutputFilename.Clear();
                    lstFiles.Items.Clear();
                    return;
                }

                txtMediaInfo.Text = GetMediaInfoAnalysis(mediaInfo);
                txtOutputFilename.Text = mediaInfo.FinalOutputFile;
                lstFiles.Items.Clear();
                lstFiles.Items.AddRange(mediaInfo.TempFiles.ToArray());
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void EncodeLog(string log)
        {
            Debug.WriteLine(log);
            this.Invoke(new Action(() => txtEncodeLog.Text = $"{DateTime.Now:[yyyy-MM-dd HH:mm:ss.fff]} {log}"));
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
                txtOutputFilename.Clear();
                lstFiles.Items.Clear();
                return "";
            }

            // Sanity check
            if (!mediaInfo.NeedsVideoReencode && !mediaInfo.NeedsAudioReencode)
            {
                return "";
            }

            // Log
            EncodeLog($"Start Encoding: {mediaInfo.Filename}");

            txtMediaInfo.Text = GetMediaInfoAnalysis(mediaInfo);
            txtOutputFilename.Text = mediaInfo.FinalOutputFile;
            lstFiles.Items.Clear();
            lstFiles.Items.AddRange(mediaInfo.TempFiles.ToArray());

            string videoOutputFileName = "";

            // Encode video
            if (mediaInfo.NeedsVideoReencode)
            {
                EncodeLog($"Video Encoding: {mediaInfo.Filename}...");

                X264VideoEncoderService videoEncoderService = ServiceFactory.GetService<X264VideoEncoderService>();

                int res = await Task.Run(() => videoEncoderService.Encode(
                    mediaInfo, 
                    @"E:\Programs\MeGUI\tools\x264\x264.exe",
                    new Action<string>((log) => EncodeLog(log)),
                    new Action<string>((progress) => EncodeProgress(progress)),
                    out videoOutputFileName)
                );
                mediaInfo.TempFiles.Add(videoOutputFileName);

                if (res != 0)
                {
                    EncodeLog($"Video Encoder failed for {mediaInfo.Filename}! Exit code : {res}");
                    UpdateCurrentFormState();
                    return "";
                }
            }

            string audioOutputFileName = "";

            // Encode Audio
            if (mediaInfo.NeedsAudioReencode)
            {
                EncodeLog($"Audio Encoding: {mediaInfo.Filename}...");

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
                mediaInfo.TempFiles.Add(audioOutputFileName);

                if (res != 0)
                {
                    EncodeLog($"Audio Encoder failed for {mediaInfo.Filename}! Exit code : {res}");
                    UpdateCurrentFormState();
                    return "";
                }
            }

            // Mux final video!
            EncodeLog($"Muxing: {mediaInfo.Filename}...");

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
            mediaInfo.FinalOutputFile = muxedFilename;

            UpdateCurrentFormState();

            return muxedFilename;
        }

        private async void btnEncode_Click(object sender, EventArgs e)
        {
            try
            {
                _requestedAbort = false;

                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    txtMediaInfo.Clear();
                    txtOutputFilename.Clear();
                    lstFiles.Items.Clear();
                    return;
                }

                lstMediaInfoItems.Enabled = false;
                btnItemRemove.Enabled = false;
                btnEncode.Enabled = false;
                btnEncodeAll.Enabled = false;
                BtnScanMediaFiles.Enabled = false;
                btnAbort.Enabled = true;

                var mediaInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;

                this.Cursor = Cursors.WaitCursor;
                string muxedFilename = await EncodeMediaInfoAsync(mediaInfo);

                // Log
                EncodeLog($"Muxed {mediaInfo.Filename} => {muxedFilename}");

                lstMediaInfoItems.Enabled = true;
                btnItemRemove.Enabled = true;
                btnEncode.Enabled = true;
                btnEncodeAll.Enabled = true;
                BtnScanMediaFiles.Enabled = true;
                btnAbort.Enabled = false;

                lstMediaInfoItems.SuspendLayout();
                lstMediaInfoItems.SelectedIndex = -1;
                lstMediaInfoItems.ResumeLayout();
                lstMediaInfoItems.SelectedItem = mediaInfo;

                this.Cursor = Cursors.Default;
                ShowInformationMessage("Encoding completed!");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                ShowExceptionMessage(ex);

                lstMediaInfoItems.Enabled = true;
                btnItemRemove.Enabled = true;
                btnEncode.Enabled = true;
                btnEncodeAll.Enabled = true;
                BtnScanMediaFiles.Enabled = true;
                btnAbort.Enabled = false;
            }
        }

        private bool _requestedAbort = false;

        private async void btnEncodeAll_Click(object sender, EventArgs e)
        {
            try
            {
                _requestedAbort = false;

                if (lstMediaInfoItems.Items.Count == 0)
                {
                    txtMediaInfo.Clear();
                    txtOutputFilename.Clear();
                    lstFiles.Items.Clear();
                    return;
                }

                //lstMediaInfoItems.Enabled = false;
                btnItemRemove.Enabled = false;
                btnEncode.Enabled = false;
                btnEncodeAll.Enabled = false;
                BtnScanMediaFiles.Enabled = false;
                btnAbort.Enabled = true;

                int i = 0;

                var mediaInfoList = lstMediaInfoItems.Items.Cast<MediaAnalyzeInfo>().ToList();

                foreach (var mediaInfo in mediaInfoList)
                {
                    if (_requestedAbort)
                    {
                        break;
                    }

                    i++;
                    txtEncodeProgress.Text = $"{i}/{mediaInfoList.Count}";

                    try
                    {
                        string muxedFilename = await EncodeMediaInfoAsync(mediaInfo);

                        // Log
                        EncodeLog($"Muxed {mediaInfo.Filename} => {muxedFilename}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        
                        // Log
                        EncodeLog($"Exception occured with {mediaInfo.Filename} => {ex.Message}");
                    }
                }

                //lstMediaInfoItems.Enabled = true;
                btnItemRemove.Enabled = true;
                btnEncode.Enabled = true;
                btnEncodeAll.Enabled = true;
                BtnScanMediaFiles.Enabled = true;
                btnAbort.Enabled = false;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);

                lstMediaInfoItems.Enabled = true;
                btnItemRemove.Enabled = true;
                btnEncode.Enabled = true;
                btnEncodeAll.Enabled = true;
                BtnScanMediaFiles.Enabled = true;
                btnAbort.Enabled = false;
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    return;
                }

                var mediaInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;

                if (Directory.Exists(Path.GetDirectoryName(mediaInfo.Filename)))
                {
                    if (File.Exists(mediaInfo.Filename))
                    {
                        Process.Start("explorer.exe", String.Format("/select, \"{0}\"", mediaInfo.Filename));
                    }
                    else
                    {
                        Process.Start("explorer.exe", Path.GetFullPath(mediaInfo.Filename));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            try
            {
                _requestedAbort = true;
                btnAbort.Enabled = false;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    return;
                }

                int selectedIndex = lstMediaInfoItems.SelectedIndex;
                lstMediaInfoItems.Items.Remove(lstMediaInfoItems.SelectedItem);

                if (selectedIndex > lstMediaInfoItems.Items.Count - 1)
                {
                    selectedIndex = lstMediaInfoItems.Items.Count - 1;
                }
                lstMediaInfoItems.SelectedIndex = selectedIndex;

                UpdateCurrentFormState();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void UserInputChanged(object sender, EventArgs e)
        {
            UpdateCurrentFormState();
        }

        private void btnRemoveDeleted_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var deletedFiles = lstMediaInfoItems.Items.Cast<MediaAnalyzeInfo>().Where(m => !File.Exists(m.Filename)).ToList();

                foreach (var deletedFile in deletedFiles)
                {
                    lstMediaInfoItems.Items.Remove(deletedFile);
                    UpdateCurrentFormState();
                }

                // For each media info, check if the temp files or the output file exists
                foreach (var mediaInfo in lstMediaInfoItems.Items.Cast<MediaAnalyzeInfo>())
                {
                    var deletedTempFiles = mediaInfo.TempFiles.Where(f => !File.Exists(f)).ToList();
                    foreach (var deletedTempFile in deletedTempFiles)
                    {
                        mediaInfo.TempFiles.Remove(deletedTempFile);
                    }
                    if (!File.Exists(mediaInfo.FinalOutputFile))
                    {
                        mediaInfo.FinalOutputFile = "";
                    }
                    UpdateCurrentFormState();
                }

                // Update the state even if no files were removed
                UpdateCurrentFormState();

                this.Cursor = Cursors.Default;
                ShowInformationMessage($"{deletedFiles.Count} files removed!");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                ShowExceptionMessage(ex);
            }
        }

        private void btnItempUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1) return;

                if (lstMediaInfoItems.SelectedIndex == 0) return;

                lstMediaInfoItems.MoveSelectedItemUp();

                UpdateCurrentFormState();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void btnItemDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1) return;

                if (lstMediaInfoItems.SelectedIndex == lstMediaInfoItems.Items.Count - 1) return;

                lstMediaInfoItems.MoveSelectedItemDown();

                UpdateCurrentFormState();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void btnOpenSourceFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    return;
                }
                MediaAnalyzeInfo mediaAnalyzeInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;
                if (mediaAnalyzeInfo == null)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                if (File.Exists(mediaAnalyzeInfo.Filename))
                {
                    Process.Start(mediaAnalyzeInfo.Filename);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                ShowExceptionMessage(ex);
            }
        }

        private void btnOpenOutputFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    return;
                }
                MediaAnalyzeInfo mediaAnalyzeInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;
                if (mediaAnalyzeInfo == null)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                if (File.Exists(mediaAnalyzeInfo.FinalOutputFile))
                {
                    Process.Start(mediaAnalyzeInfo.FinalOutputFile);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                ShowExceptionMessage(ex);
            }
        }

        private void btnDeleteTempFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    return;
                }
                MediaAnalyzeInfo mediaAnalyzeInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;
                if (mediaAnalyzeInfo == null)
                {
                    return;
                }

                // Ask the user if he is really sure to delete the temp files
                if (ShowQuestion("Are you sure you want to delete the temp files?", "Are you sure?", false) != DialogResult.Yes)
                {
                    return;
                }

                // Delete the Temp Files of the selected MediaAnalyzeInfo from the list
                this.Cursor = Cursors.WaitCursor;
                long successCount = 0;
                foreach (string tempFile in mediaAnalyzeInfo.TempFiles.ToList())
                {
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                        successCount++;
                    }
                    mediaAnalyzeInfo.TempFiles.Remove(tempFile);
                }
                UpdateCurrentFormState();

                lstMediaInfoItems.SuspendLayout();
                lstMediaInfoItems.SelectedIndex = -1;
                lstMediaInfoItems.ResumeLayout();
                lstMediaInfoItems.SelectedItem = mediaAnalyzeInfo;

                this.Cursor = Cursors.Default;
                ShowInformationMessage($"{successCount} Temp files deleted!");
            }
            catch (Exception ex)
            {
                UpdateCurrentFormState();
                this.Cursor = Cursors.Default;
                ShowExceptionMessage(ex);
            }
        }

        private void btnDeleteOutputFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMediaInfoItems.SelectedIndex == -1)
                {
                    return;
                }
                MediaAnalyzeInfo mediaAnalyzeInfo = lstMediaInfoItems.SelectedItem as MediaAnalyzeInfo;
                if (mediaAnalyzeInfo == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(mediaAnalyzeInfo.FinalOutputFile))
                {
                    return;
                }

                // Ask the user if he is really sure to delete the temp files
                if (ShowQuestion("Are you sure you want to delete the Output file?", "Are you sure?", false) != DialogResult.Yes)
                {
                    return;
                }

                // Delete the final output file of the selected MediaAnalyzeInfo from the list
                this.Cursor = Cursors.WaitCursor;
                long successCount = 0;
                if (File.Exists(mediaAnalyzeInfo.FinalOutputFile))
                {
                    File.Delete(mediaAnalyzeInfo.FinalOutputFile);
                    mediaAnalyzeInfo.FinalOutputFile = "";
                    successCount++;
                }
                UpdateCurrentFormState();

                lstMediaInfoItems.SuspendLayout();
                lstMediaInfoItems.SelectedIndex = -1;
                lstMediaInfoItems.ResumeLayout();
                lstMediaInfoItems.SelectedItem = mediaAnalyzeInfo;

                this.Cursor = Cursors.Default;
                ShowInformationMessage($"{successCount} Output files deleted!");
            }
            catch (Exception ex)
            {
                UpdateCurrentFormState();
                this.Cursor = Cursors.Default;
                ShowExceptionMessage(ex);
            }
        }

        private void btnDeleteAllCompletedFiles_Click(object sender, EventArgs e)
        {
            try
            {
                // Ask the user if he is really sure to delete the temp files
                if (ShowQuestion("Are you sure you want to delete all temp and source files?", "Are you sure?", false) != DialogResult.Yes)
                {
                    return;
                }

                // Check all media info items, and for those that have final output file,
                // delete the temp files, delete the source file, and remove the item from the list
                this.Cursor = Cursors.WaitCursor;
                List<MediaAnalyzeInfo> mediaAnalyzeInfos = lstMediaInfoItems.Items.Cast<MediaAnalyzeInfo>().ToList();
                long successCount = 0;
                long failCount = 0;
                this.Cursor = Cursors.WaitCursor;
                foreach (MediaAnalyzeInfo mediaAnalyzeInfo in mediaAnalyzeInfos)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(mediaAnalyzeInfo.FinalOutputFile))
                        {
                            continue;
                        }

                        this.Invoke((MethodInvoker)(() => { this.txtEncodeLog.Text = $"Deleting {mediaAnalyzeInfo.Filename} files..." ; }));
                        Application.DoEvents();

                        // Delete the Temp Files of the selected MediaAnalyzeInfo from the list
                        foreach (string tempFile in mediaAnalyzeInfo.TempFiles.ToList())
                        {
                            if (File.Exists(tempFile))
                            {
                                this.Invoke((MethodInvoker)(() => { this.txtEncodeLogProgress.Text = $"Deleting {tempFile}..."; }));

                                File.Delete(tempFile);
                            }
                            mediaAnalyzeInfo.TempFiles.Remove(tempFile);
                        }

                        // Delete the source file of the selected MediaAnalyzeInfo from the list
                        if (File.Exists(mediaAnalyzeInfo.Filename))
                        {
                            this.Invoke((MethodInvoker)(() => { this.txtEncodeLogProgress.Text = $"Deleting {mediaAnalyzeInfo.Filename}..."; }));

                            File.Delete(mediaAnalyzeInfo.Filename);
                            lstMediaInfoItems.Items.Remove(mediaAnalyzeInfo);
                        }
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        failCount++;
                    }
                }
                UpdateCurrentFormState();

                lstMediaInfoItems.SelectedIndex = -1;

                this.Cursor = Cursors.Default;

                if (failCount > 0)
                {
                    ShowWarningMessage($"{successCount} files deleted, {failCount} failed!");
                }
                else
                {
                    ShowInformationMessage($"{successCount} files deleted!");
                }
            }
            catch (Exception ex)
            {
                UpdateCurrentFormState();
                this.Cursor = Cursors.Default;
                ShowExceptionMessage(ex);
            }
        }
    }
}

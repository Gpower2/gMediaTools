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
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Services;
using gMediaTools.Services.CurveFitting;
using gMediaTools.Services.MediaAnalyzer;

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

        private void btnResolutionBitrate_Click(object sender, EventArgs e)
        {
            FrmResolutionBitrateEditor frm = new FrmResolutionBitrateEditor();
            frm.ShowDialog(this);
        }
    }
}

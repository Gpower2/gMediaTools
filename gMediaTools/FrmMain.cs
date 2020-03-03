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
        private readonly CurveFittingRepository _curveFittingRepo = new CurveFittingRepository();

        private readonly MediaAnalyzerService _mediaAnalyzerService = new MediaAnalyzerService();

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

                _mediaAnalyzerService.AnalyzePath(
                    new MediaAnalyzePathRequest
                    {
                        MediaDirectoryName = rootPath,
                        BitratePercentageThreshold = 10,
                        GainPercentageThreshold = 20
                    },
                    curveSettings,
                    new MediaAnalyzeActions
                    {
                        SetCurrentFileAction = (string currentFile) =>
                           {
                               this.Invoke((MethodInvoker)(() => { this.txtCurrentFile.Text = currentFile; }));
                               Application.DoEvents();
                           },
                        LogLineAction = (string logText) =>
                             {
                                 this.Invoke((MethodInvoker)(() => { this.txtLog.AppendText(logText + Environment.NewLine); }));
                                 Application.DoEvents();
                             },
                        UpdateProgressAction = (int reencodeFiles, int totalFiles) =>
                             {
                                 this.Invoke((MethodInvoker)(() => { this.txtFilesProgress.Text = $"{reencodeFiles}/{totalFiles} ({Math.Round((double)reencodeFiles / (double)totalFiles * 100.0, 2)}%)"; }));
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

namespace gMediaTools.Forms
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnScanMediaFiles = new System.Windows.Forms.Button();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtCurrentFile = new System.Windows.Forms.TextBox();
            this.txtFilesProgress = new System.Windows.Forms.TextBox();
            this.btnResolutionBitrate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnScanMediaFiles
            // 
            this.BtnScanMediaFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnScanMediaFiles.Location = new System.Drawing.Point(841, 42);
            this.BtnScanMediaFiles.Name = "BtnScanMediaFiles";
            this.BtnScanMediaFiles.Size = new System.Drawing.Size(80, 45);
            this.BtnScanMediaFiles.TabIndex = 0;
            this.BtnScanMediaFiles.Text = "Scan";
            this.BtnScanMediaFiles.UseVisualStyleBackColor = true;
            this.BtnScanMediaFiles.Click += new System.EventHandler(this.BtnScanMediaFiles_Click);
            // 
            // txtInputFolder
            // 
            this.txtInputFolder.AllowDrop = true;
            this.txtInputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFolder.Location = new System.Drawing.Point(15, 12);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.Size = new System.Drawing.Size(906, 23);
            this.txtInputFolder.TabIndex = 1;
            this.txtInputFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInputFolder_DragDrop);
            this.txtInputFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInputFolder_DragEnter);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtLog.Location = new System.Drawing.Point(15, 120);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(906, 387);
            this.txtLog.TabIndex = 2;
            this.txtLog.WordWrap = false;
            // 
            // txtCurrentFile
            // 
            this.txtCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentFile.Location = new System.Drawing.Point(15, 94);
            this.txtCurrentFile.Name = "txtCurrentFile";
            this.txtCurrentFile.ReadOnly = true;
            this.txtCurrentFile.Size = new System.Drawing.Size(906, 23);
            this.txtCurrentFile.TabIndex = 3;
            // 
            // txtFilesProgress
            // 
            this.txtFilesProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilesProgress.Location = new System.Drawing.Point(15, 64);
            this.txtFilesProgress.Name = "txtFilesProgress";
            this.txtFilesProgress.ReadOnly = true;
            this.txtFilesProgress.Size = new System.Drawing.Size(734, 23);
            this.txtFilesProgress.TabIndex = 4;
            // 
            // btnResolutionBitrate
            // 
            this.btnResolutionBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResolutionBitrate.Location = new System.Drawing.Point(755, 42);
            this.btnResolutionBitrate.Name = "btnResolutionBitrate";
            this.btnResolutionBitrate.Size = new System.Drawing.Size(80, 45);
            this.btnResolutionBitrate.TabIndex = 5;
            this.btnResolutionBitrate.Text = "Resolution-Bitrate...";
            this.btnResolutionBitrate.UseVisualStyleBackColor = true;
            this.btnResolutionBitrate.Click += new System.EventHandler(this.btnResolutionBitrate_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.btnResolutionBitrate);
            this.Controls.Add(this.txtFilesProgress);
            this.Controls.Add(this.txtCurrentFile);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtInputFolder);
            this.Controls.Add(this.BtnScanMediaFiles);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Name = "FrmMain";
            this.Text = "gMediaTools";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnScanMediaFiles;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtCurrentFile;
        private System.Windows.Forms.TextBox txtFilesProgress;
        private System.Windows.Forms.Button btnResolutionBitrate;
    }
}


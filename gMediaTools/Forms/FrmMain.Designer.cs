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
            this.txtBitratePercentageThreshold = new gMediaTools.GTextBox();
            this.txtGainPercentageThreshold = new gMediaTools.GTextBox();
            this.txtMaxAllowedWidth = new gMediaTools.GTextBox();
            this.txtMaxAllowedHeight = new gMediaTools.GTextBox();
            this.txtMinAllowedBitrate = new gMediaTools.GTextBox();
            this.lblBitratePercentageThreshold = new System.Windows.Forms.Label();
            this.lblGainPercentageThreshold = new System.Windows.Forms.Label();
            this.lblMaxAllowedWidth = new System.Windows.Forms.Label();
            this.lblMaxAllowedHeight = new System.Windows.Forms.Label();
            this.lblMinAllowedBitrate = new System.Windows.Forms.Label();
            this.lblInputFolder = new System.Windows.Forms.Label();
            this.lstMediaInfoItems = new System.Windows.Forms.ListBox();
            this.txtMediaInfo = new System.Windows.Forms.RichTextBox();
            this.btnEncode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnScanMediaFiles
            // 
            this.BtnScanMediaFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnScanMediaFiles.Location = new System.Drawing.Point(892, 54);
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
            this.txtInputFolder.Location = new System.Drawing.Point(16, 26);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.Size = new System.Drawing.Size(956, 23);
            this.txtInputFolder.TabIndex = 1;
            this.txtInputFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInputFolder_DragDrop);
            this.txtInputFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInputFolder_DragEnter);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtLog.Location = new System.Drawing.Point(15, 167);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(957, 132);
            this.txtLog.TabIndex = 2;
            this.txtLog.WordWrap = false;
            // 
            // txtCurrentFile
            // 
            this.txtCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentFile.Location = new System.Drawing.Point(15, 138);
            this.txtCurrentFile.Name = "txtCurrentFile";
            this.txtCurrentFile.ReadOnly = true;
            this.txtCurrentFile.Size = new System.Drawing.Size(957, 23);
            this.txtCurrentFile.TabIndex = 3;
            // 
            // txtFilesProgress
            // 
            this.txtFilesProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilesProgress.Location = new System.Drawing.Point(15, 108);
            this.txtFilesProgress.Name = "txtFilesProgress";
            this.txtFilesProgress.ReadOnly = true;
            this.txtFilesProgress.Size = new System.Drawing.Size(957, 23);
            this.txtFilesProgress.TabIndex = 4;
            // 
            // btnResolutionBitrate
            // 
            this.btnResolutionBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResolutionBitrate.Location = new System.Drawing.Point(806, 54);
            this.btnResolutionBitrate.Name = "btnResolutionBitrate";
            this.btnResolutionBitrate.Size = new System.Drawing.Size(80, 45);
            this.btnResolutionBitrate.TabIndex = 5;
            this.btnResolutionBitrate.Text = "Resolution-Bitrate...";
            this.btnResolutionBitrate.UseVisualStyleBackColor = true;
            this.btnResolutionBitrate.Click += new System.EventHandler(this.btnResolutionBitrate_Click);
            // 
            // txtBitratePercentageThreshold
            // 
            this.txtBitratePercentageThreshold.DataObject = null;
            this.txtBitratePercentageThreshold.Decimals = 0;
            this.txtBitratePercentageThreshold.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtBitratePercentageThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtBitratePercentageThreshold.Int32Value = 0;
            this.txtBitratePercentageThreshold.Int64Value = ((long)(0));
            this.txtBitratePercentageThreshold.Location = new System.Drawing.Point(16, 76);
            this.txtBitratePercentageThreshold.MaxLength = 3;
            this.txtBitratePercentageThreshold.Name = "txtBitratePercentageThreshold";
            this.txtBitratePercentageThreshold.Size = new System.Drawing.Size(170, 23);
            this.txtBitratePercentageThreshold.TabIndex = 6;
            this.txtBitratePercentageThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBitratePercentageThreshold.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // txtGainPercentageThreshold
            // 
            this.txtGainPercentageThreshold.DataObject = null;
            this.txtGainPercentageThreshold.Decimals = 0;
            this.txtGainPercentageThreshold.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtGainPercentageThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtGainPercentageThreshold.Int32Value = 0;
            this.txtGainPercentageThreshold.Int64Value = ((long)(0));
            this.txtGainPercentageThreshold.Location = new System.Drawing.Point(197, 76);
            this.txtGainPercentageThreshold.MaxLength = 3;
            this.txtGainPercentageThreshold.Name = "txtGainPercentageThreshold";
            this.txtGainPercentageThreshold.Size = new System.Drawing.Size(163, 23);
            this.txtGainPercentageThreshold.TabIndex = 7;
            this.txtGainPercentageThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGainPercentageThreshold.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // txtMaxAllowedWidth
            // 
            this.txtMaxAllowedWidth.DataObject = null;
            this.txtMaxAllowedWidth.Decimals = 0;
            this.txtMaxAllowedWidth.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtMaxAllowedWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtMaxAllowedWidth.Int32Value = 0;
            this.txtMaxAllowedWidth.Int64Value = ((long)(0));
            this.txtMaxAllowedWidth.Location = new System.Drawing.Point(389, 76);
            this.txtMaxAllowedWidth.MaxLength = 4;
            this.txtMaxAllowedWidth.Name = "txtMaxAllowedWidth";
            this.txtMaxAllowedWidth.Size = new System.Drawing.Size(105, 23);
            this.txtMaxAllowedWidth.TabIndex = 8;
            this.txtMaxAllowedWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMaxAllowedWidth.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // txtMaxAllowedHeight
            // 
            this.txtMaxAllowedHeight.DataObject = null;
            this.txtMaxAllowedHeight.Decimals = 0;
            this.txtMaxAllowedHeight.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtMaxAllowedHeight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtMaxAllowedHeight.Int32Value = 0;
            this.txtMaxAllowedHeight.Int64Value = ((long)(0));
            this.txtMaxAllowedHeight.Location = new System.Drawing.Point(508, 76);
            this.txtMaxAllowedHeight.MaxLength = 4;
            this.txtMaxAllowedHeight.Name = "txtMaxAllowedHeight";
            this.txtMaxAllowedHeight.Size = new System.Drawing.Size(109, 23);
            this.txtMaxAllowedHeight.TabIndex = 9;
            this.txtMaxAllowedHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMaxAllowedHeight.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // txtMinAllowedBitrate
            // 
            this.txtMinAllowedBitrate.DataObject = null;
            this.txtMinAllowedBitrate.Decimals = 0;
            this.txtMinAllowedBitrate.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtMinAllowedBitrate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtMinAllowedBitrate.Int32Value = 0;
            this.txtMinAllowedBitrate.Int64Value = ((long)(0));
            this.txtMinAllowedBitrate.Location = new System.Drawing.Point(643, 76);
            this.txtMinAllowedBitrate.MaxLength = 6;
            this.txtMinAllowedBitrate.Name = "txtMinAllowedBitrate";
            this.txtMinAllowedBitrate.Size = new System.Drawing.Size(102, 23);
            this.txtMinAllowedBitrate.TabIndex = 10;
            this.txtMinAllowedBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMinAllowedBitrate.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // lblBitratePercentageThreshold
            // 
            this.lblBitratePercentageThreshold.AutoSize = true;
            this.lblBitratePercentageThreshold.Location = new System.Drawing.Point(16, 57);
            this.lblBitratePercentageThreshold.Name = "lblBitratePercentageThreshold";
            this.lblBitratePercentageThreshold.Size = new System.Drawing.Size(173, 15);
            this.lblBitratePercentageThreshold.TabIndex = 11;
            this.lblBitratePercentageThreshold.Text = "BitratePercentageThreshold (%)";
            // 
            // lblGainPercentageThreshold
            // 
            this.lblGainPercentageThreshold.AutoSize = true;
            this.lblGainPercentageThreshold.Location = new System.Drawing.Point(197, 57);
            this.lblGainPercentageThreshold.Name = "lblGainPercentageThreshold";
            this.lblGainPercentageThreshold.Size = new System.Drawing.Size(163, 15);
            this.lblGainPercentageThreshold.TabIndex = 12;
            this.lblGainPercentageThreshold.Text = "GainPercentageThreshold (%)";
            // 
            // lblMaxAllowedWidth
            // 
            this.lblMaxAllowedWidth.AutoSize = true;
            this.lblMaxAllowedWidth.Location = new System.Drawing.Point(389, 57);
            this.lblMaxAllowedWidth.Name = "lblMaxAllowedWidth";
            this.lblMaxAllowedWidth.Size = new System.Drawing.Size(105, 15);
            this.lblMaxAllowedWidth.TabIndex = 13;
            this.lblMaxAllowedWidth.Text = "MaxAllowedWidth";
            // 
            // lblMaxAllowedHeight
            // 
            this.lblMaxAllowedHeight.AutoSize = true;
            this.lblMaxAllowedHeight.Location = new System.Drawing.Point(508, 57);
            this.lblMaxAllowedHeight.Name = "lblMaxAllowedHeight";
            this.lblMaxAllowedHeight.Size = new System.Drawing.Size(109, 15);
            this.lblMaxAllowedHeight.TabIndex = 14;
            this.lblMaxAllowedHeight.Text = "MaxAllowedHeight";
            // 
            // lblMinAllowedBitrate
            // 
            this.lblMinAllowedBitrate.AutoSize = true;
            this.lblMinAllowedBitrate.Location = new System.Drawing.Point(643, 57);
            this.lblMinAllowedBitrate.Name = "lblMinAllowedBitrate";
            this.lblMinAllowedBitrate.Size = new System.Drawing.Size(105, 15);
            this.lblMinAllowedBitrate.TabIndex = 15;
            this.lblMinAllowedBitrate.Text = "MinAllowedBitrate";
            // 
            // lblInputFolder
            // 
            this.lblInputFolder.AutoSize = true;
            this.lblInputFolder.Location = new System.Drawing.Point(16, 7);
            this.lblInputFolder.Name = "lblInputFolder";
            this.lblInputFolder.Size = new System.Drawing.Size(55, 15);
            this.lblInputFolder.TabIndex = 16;
            this.lblInputFolder.Text = "Directory";
            // 
            // lstMediaInfoItems
            // 
            this.lstMediaInfoItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMediaInfoItems.FormattingEnabled = true;
            this.lstMediaInfoItems.ItemHeight = 15;
            this.lstMediaInfoItems.Location = new System.Drawing.Point(15, 305);
            this.lstMediaInfoItems.Name = "lstMediaInfoItems";
            this.lstMediaInfoItems.Size = new System.Drawing.Size(957, 169);
            this.lstMediaInfoItems.TabIndex = 17;
            this.lstMediaInfoItems.SelectedIndexChanged += new System.EventHandler(this.lstMediaInfoItems_SelectedIndexChanged);
            // 
            // txtMediaInfo
            // 
            this.txtMediaInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMediaInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtMediaInfo.Location = new System.Drawing.Point(15, 480);
            this.txtMediaInfo.Name = "txtMediaInfo";
            this.txtMediaInfo.Size = new System.Drawing.Size(871, 102);
            this.txtMediaInfo.TabIndex = 18;
            this.txtMediaInfo.Text = "";
            // 
            // btnEncode
            // 
            this.btnEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncode.Location = new System.Drawing.Point(892, 480);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 45);
            this.btnEncode.TabIndex = 19;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(984, 594);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtMediaInfo);
            this.Controls.Add(this.lstMediaInfoItems);
            this.Controls.Add(this.lblInputFolder);
            this.Controls.Add(this.lblMinAllowedBitrate);
            this.Controls.Add(this.lblMaxAllowedHeight);
            this.Controls.Add(this.lblMaxAllowedWidth);
            this.Controls.Add(this.lblGainPercentageThreshold);
            this.Controls.Add(this.lblBitratePercentageThreshold);
            this.Controls.Add(this.txtMinAllowedBitrate);
            this.Controls.Add(this.txtMaxAllowedHeight);
            this.Controls.Add(this.txtMaxAllowedWidth);
            this.Controls.Add(this.txtGainPercentageThreshold);
            this.Controls.Add(this.txtBitratePercentageThreshold);
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
        private GTextBox txtBitratePercentageThreshold;
        private GTextBox txtGainPercentageThreshold;
        private GTextBox txtMaxAllowedWidth;
        private GTextBox txtMaxAllowedHeight;
        private GTextBox txtMinAllowedBitrate;
        private System.Windows.Forms.Label lblBitratePercentageThreshold;
        private System.Windows.Forms.Label lblGainPercentageThreshold;
        private System.Windows.Forms.Label lblMaxAllowedWidth;
        private System.Windows.Forms.Label lblMaxAllowedHeight;
        private System.Windows.Forms.Label lblMinAllowedBitrate;
        private System.Windows.Forms.Label lblInputFolder;
        private System.Windows.Forms.ListBox lstMediaInfoItems;
        private System.Windows.Forms.RichTextBox txtMediaInfo;
        private System.Windows.Forms.Button btnEncode;
    }
}


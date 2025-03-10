﻿namespace gMediaTools.Forms
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
            this.btnEncodeAll = new System.Windows.Forms.Button();
            this.txtEncodeLog = new System.Windows.Forms.TextBox();
            this.txtEncodeProgress = new gMediaTools.GTextBox();
            this.txtEncodeLogProgress = new System.Windows.Forms.TextBox();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnItemRemove = new System.Windows.Forms.Button();
            this.btnRemoveDeleted = new System.Windows.Forms.Button();
            this.grpItems = new System.Windows.Forms.GroupBox();
            this.tlpItems = new System.Windows.Forms.TableLayoutPanel();
            this.pnlItemActions = new System.Windows.Forms.Panel();
            this.btnItemDown = new System.Windows.Forms.Button();
            this.btnItempUp = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.txtOutputFilename = new System.Windows.Forms.RichTextBox();
            this.btnOpenSourceFile = new System.Windows.Forms.Button();
            this.btnOpenOutputFile = new System.Windows.Forms.Button();
            this.btnDeleteTempFiles = new System.Windows.Forms.Button();
            this.btnDeleteOutputFile = new System.Windows.Forms.Button();
            this.btnDeleteAllCompletedFiles = new System.Windows.Forms.Button();
            this.grpItems.SuspendLayout();
            this.tlpItems.SuspendLayout();
            this.pnlItemActions.SuspendLayout();
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
            this.txtInputFolder.TextChanged += new System.EventHandler(this.UserInputChanged);
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
            this.txtLog.Size = new System.Drawing.Size(957, 88);
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
            this.txtBitratePercentageThreshold.TextChanged += new System.EventHandler(this.UserInputChanged);
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
            this.txtGainPercentageThreshold.TextChanged += new System.EventHandler(this.UserInputChanged);
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
            this.txtMaxAllowedWidth.TextChanged += new System.EventHandler(this.UserInputChanged);
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
            this.txtMaxAllowedHeight.TextChanged += new System.EventHandler(this.UserInputChanged);
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
            this.txtMinAllowedBitrate.TextChanged += new System.EventHandler(this.UserInputChanged);
            // 
            // lblBitratePercentageThreshold
            // 
            this.lblBitratePercentageThreshold.AutoSize = true;
            this.lblBitratePercentageThreshold.Location = new System.Drawing.Point(16, 57);
            this.lblBitratePercentageThreshold.Name = "lblBitratePercentageThreshold";
            this.lblBitratePercentageThreshold.Size = new System.Drawing.Size(174, 15);
            this.lblBitratePercentageThreshold.TabIndex = 11;
            this.lblBitratePercentageThreshold.Text = "BitratePercentageThreshold (%)";
            // 
            // lblGainPercentageThreshold
            // 
            this.lblGainPercentageThreshold.AutoSize = true;
            this.lblGainPercentageThreshold.Location = new System.Drawing.Point(197, 57);
            this.lblGainPercentageThreshold.Name = "lblGainPercentageThreshold";
            this.lblGainPercentageThreshold.Size = new System.Drawing.Size(164, 15);
            this.lblGainPercentageThreshold.TabIndex = 12;
            this.lblGainPercentageThreshold.Text = "GainPercentageThreshold (%)";
            // 
            // lblMaxAllowedWidth
            // 
            this.lblMaxAllowedWidth.AutoSize = true;
            this.lblMaxAllowedWidth.Location = new System.Drawing.Point(389, 57);
            this.lblMaxAllowedWidth.Name = "lblMaxAllowedWidth";
            this.lblMaxAllowedWidth.Size = new System.Drawing.Size(104, 15);
            this.lblMaxAllowedWidth.TabIndex = 13;
            this.lblMaxAllowedWidth.Text = "MaxAllowedWidth";
            // 
            // lblMaxAllowedHeight
            // 
            this.lblMaxAllowedHeight.AutoSize = true;
            this.lblMaxAllowedHeight.Location = new System.Drawing.Point(508, 57);
            this.lblMaxAllowedHeight.Name = "lblMaxAllowedHeight";
            this.lblMaxAllowedHeight.Size = new System.Drawing.Size(108, 15);
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
            this.lstMediaInfoItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMediaInfoItems.FormattingEnabled = true;
            this.lstMediaInfoItems.ItemHeight = 15;
            this.lstMediaInfoItems.Location = new System.Drawing.Point(3, 3);
            this.lstMediaInfoItems.Name = "lstMediaInfoItems";
            this.lstMediaInfoItems.Size = new System.Drawing.Size(895, 96);
            this.lstMediaInfoItems.TabIndex = 17;
            this.lstMediaInfoItems.SelectedIndexChanged += new System.EventHandler(this.lstMediaInfoItems_SelectedIndexChanged);
            // 
            // txtMediaInfo
            // 
            this.txtMediaInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMediaInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtMediaInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtMediaInfo.Location = new System.Drawing.Point(15, 452);
            this.txtMediaInfo.Name = "txtMediaInfo";
            this.txtMediaInfo.ReadOnly = true;
            this.txtMediaInfo.Size = new System.Drawing.Size(471, 288);
            this.txtMediaInfo.TabIndex = 18;
            this.txtMediaInfo.Text = "";
            // 
            // btnEncode
            // 
            this.btnEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncode.Location = new System.Drawing.Point(892, 599);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 40);
            this.btnEncode.TabIndex = 19;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // btnEncodeAll
            // 
            this.btnEncodeAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeAll.Location = new System.Drawing.Point(892, 645);
            this.btnEncodeAll.Name = "btnEncodeAll";
            this.btnEncodeAll.Size = new System.Drawing.Size(80, 40);
            this.btnEncodeAll.TabIndex = 20;
            this.btnEncodeAll.Text = "Encode All";
            this.btnEncodeAll.UseVisualStyleBackColor = true;
            this.btnEncodeAll.Click += new System.EventHandler(this.btnEncodeAll_Click);
            // 
            // txtEncodeLog
            // 
            this.txtEncodeLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncodeLog.Location = new System.Drawing.Point(15, 390);
            this.txtEncodeLog.Name = "txtEncodeLog";
            this.txtEncodeLog.ReadOnly = true;
            this.txtEncodeLog.Size = new System.Drawing.Size(871, 23);
            this.txtEncodeLog.TabIndex = 21;
            // 
            // txtEncodeProgress
            // 
            this.txtEncodeProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncodeProgress.DataObject = null;
            this.txtEncodeProgress.Decimals = 2;
            this.txtEncodeProgress.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtEncodeProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtEncodeProgress.Int32Value = 0;
            this.txtEncodeProgress.Int64Value = ((long)(0));
            this.txtEncodeProgress.Location = new System.Drawing.Point(892, 390);
            this.txtEncodeProgress.Name = "txtEncodeProgress";
            this.txtEncodeProgress.ReadOnly = true;
            this.txtEncodeProgress.Size = new System.Drawing.Size(80, 23);
            this.txtEncodeProgress.TabIndex = 22;
            this.txtEncodeProgress.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Text;
            // 
            // txtEncodeLogProgress
            // 
            this.txtEncodeLogProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncodeLogProgress.Location = new System.Drawing.Point(15, 419);
            this.txtEncodeLogProgress.Name = "txtEncodeLogProgress";
            this.txtEncodeLogProgress.ReadOnly = true;
            this.txtEncodeLogProgress.Size = new System.Drawing.Size(957, 23);
            this.txtEncodeLogProgress.TabIndex = 23;
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.Location = new System.Drawing.Point(806, 700);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(80, 40);
            this.btnOpenFolder.TabIndex = 24;
            this.btnOpenFolder.Text = "Open Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.Location = new System.Drawing.Point(892, 700);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(80, 40);
            this.btnAbort.TabIndex = 25;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnItemRemove
            // 
            this.btnItemRemove.Location = new System.Drawing.Point(3, 68);
            this.btnItemRemove.Name = "btnItemRemove";
            this.btnItemRemove.Size = new System.Drawing.Size(42, 30);
            this.btnItemRemove.TabIndex = 26;
            this.btnItemRemove.Text = "X";
            this.btnItemRemove.UseVisualStyleBackColor = true;
            this.btnItemRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveDeleted
            // 
            this.btnRemoveDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveDeleted.Location = new System.Drawing.Point(806, 452);
            this.btnRemoveDeleted.Name = "btnRemoveDeleted";
            this.btnRemoveDeleted.Size = new System.Drawing.Size(166, 40);
            this.btnRemoveDeleted.TabIndex = 27;
            this.btnRemoveDeleted.Text = "Remove Deleted Source/Temp Files";
            this.btnRemoveDeleted.UseVisualStyleBackColor = true;
            this.btnRemoveDeleted.Click += new System.EventHandler(this.btnRemoveDeleted_Click);
            // 
            // grpItems
            // 
            this.grpItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpItems.Controls.Add(this.tlpItems);
            this.grpItems.Location = new System.Drawing.Point(15, 261);
            this.grpItems.Name = "grpItems";
            this.grpItems.Size = new System.Drawing.Size(957, 124);
            this.grpItems.TabIndex = 28;
            this.grpItems.TabStop = false;
            this.grpItems.Text = "Items";
            // 
            // tlpItems
            // 
            this.tlpItems.ColumnCount = 2;
            this.tlpItems.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpItems.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpItems.Controls.Add(this.lstMediaInfoItems, 0, 0);
            this.tlpItems.Controls.Add(this.pnlItemActions, 1, 0);
            this.tlpItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpItems.Location = new System.Drawing.Point(3, 19);
            this.tlpItems.Name = "tlpItems";
            this.tlpItems.RowCount = 1;
            this.tlpItems.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpItems.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tlpItems.Size = new System.Drawing.Size(951, 102);
            this.tlpItems.TabIndex = 0;
            // 
            // pnlItemActions
            // 
            this.pnlItemActions.Controls.Add(this.btnItemDown);
            this.pnlItemActions.Controls.Add(this.btnItempUp);
            this.pnlItemActions.Controls.Add(this.btnItemRemove);
            this.pnlItemActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlItemActions.Location = new System.Drawing.Point(901, 0);
            this.pnlItemActions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlItemActions.Name = "pnlItemActions";
            this.pnlItemActions.Size = new System.Drawing.Size(50, 102);
            this.pnlItemActions.TabIndex = 18;
            // 
            // btnItemDown
            // 
            this.btnItemDown.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnItemDown.Location = new System.Drawing.Point(3, 35);
            this.btnItemDown.Name = "btnItemDown";
            this.btnItemDown.Size = new System.Drawing.Size(42, 30);
            this.btnItemDown.TabIndex = 1;
            this.btnItemDown.Text = "Dn";
            this.btnItemDown.UseVisualStyleBackColor = true;
            this.btnItemDown.Click += new System.EventHandler(this.btnItemDown_Click);
            // 
            // btnItempUp
            // 
            this.btnItempUp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnItempUp.Location = new System.Drawing.Point(3, 3);
            this.btnItempUp.Name = "btnItempUp";
            this.btnItempUp.Size = new System.Drawing.Size(42, 30);
            this.btnItempUp.TabIndex = 0;
            this.btnItempUp.Text = "Up";
            this.btnItempUp.UseVisualStyleBackColor = true;
            this.btnItempUp.Click += new System.EventHandler(this.btnItempUp_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.ItemHeight = 15;
            this.lstFiles.Location = new System.Drawing.Point(492, 496);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(308, 244);
            this.lstFiles.TabIndex = 29;
            // 
            // txtOutputFilename
            // 
            this.txtOutputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFilename.BackColor = System.Drawing.SystemColors.Window;
            this.txtOutputFilename.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtOutputFilename.Location = new System.Drawing.Point(492, 452);
            this.txtOutputFilename.Name = "txtOutputFilename";
            this.txtOutputFilename.ReadOnly = true;
            this.txtOutputFilename.Size = new System.Drawing.Size(308, 38);
            this.txtOutputFilename.TabIndex = 30;
            this.txtOutputFilename.Text = "";
            // 
            // btnOpenSourceFile
            // 
            this.btnOpenSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenSourceFile.Location = new System.Drawing.Point(806, 599);
            this.btnOpenSourceFile.Name = "btnOpenSourceFile";
            this.btnOpenSourceFile.Size = new System.Drawing.Size(80, 40);
            this.btnOpenSourceFile.TabIndex = 31;
            this.btnOpenSourceFile.Text = "Open Source File";
            this.btnOpenSourceFile.UseVisualStyleBackColor = true;
            this.btnOpenSourceFile.Click += new System.EventHandler(this.btnOpenSourceFile_Click);
            // 
            // btnOpenOutputFile
            // 
            this.btnOpenOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenOutputFile.Location = new System.Drawing.Point(806, 645);
            this.btnOpenOutputFile.Name = "btnOpenOutputFile";
            this.btnOpenOutputFile.Size = new System.Drawing.Size(80, 40);
            this.btnOpenOutputFile.TabIndex = 32;
            this.btnOpenOutputFile.Text = "Open Output File";
            this.btnOpenOutputFile.UseVisualStyleBackColor = true;
            this.btnOpenOutputFile.Click += new System.EventHandler(this.btnOpenOutputFile_Click);
            // 
            // btnDeleteTempFiles
            // 
            this.btnDeleteTempFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTempFiles.Location = new System.Drawing.Point(806, 498);
            this.btnDeleteTempFiles.Name = "btnDeleteTempFiles";
            this.btnDeleteTempFiles.Size = new System.Drawing.Size(80, 40);
            this.btnDeleteTempFiles.TabIndex = 33;
            this.btnDeleteTempFiles.Text = "Delete Temp Files";
            this.btnDeleteTempFiles.UseVisualStyleBackColor = true;
            this.btnDeleteTempFiles.Click += new System.EventHandler(this.btnDeleteTempFiles_Click);
            // 
            // btnDeleteOutputFile
            // 
            this.btnDeleteOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteOutputFile.Location = new System.Drawing.Point(892, 498);
            this.btnDeleteOutputFile.Name = "btnDeleteOutputFile";
            this.btnDeleteOutputFile.Size = new System.Drawing.Size(80, 40);
            this.btnDeleteOutputFile.TabIndex = 34;
            this.btnDeleteOutputFile.Text = "Delete Output File";
            this.btnDeleteOutputFile.UseVisualStyleBackColor = true;
            this.btnDeleteOutputFile.Click += new System.EventHandler(this.btnDeleteOutputFile_Click);
            // 
            // btnDeleteAllCompletedFiles
            // 
            this.btnDeleteAllCompletedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteAllCompletedFiles.Location = new System.Drawing.Point(806, 544);
            this.btnDeleteAllCompletedFiles.Name = "btnDeleteAllCompletedFiles";
            this.btnDeleteAllCompletedFiles.Size = new System.Drawing.Size(166, 40);
            this.btnDeleteAllCompletedFiles.TabIndex = 35;
            this.btnDeleteAllCompletedFiles.Text = "Delete All Source/Temp Files";
            this.btnDeleteAllCompletedFiles.UseVisualStyleBackColor = true;
            this.btnDeleteAllCompletedFiles.Click += new System.EventHandler(this.btnDeleteAllCompletedFiles_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.btnDeleteAllCompletedFiles);
            this.Controls.Add(this.btnDeleteOutputFile);
            this.Controls.Add(this.btnDeleteTempFiles);
            this.Controls.Add(this.btnOpenOutputFile);
            this.Controls.Add(this.btnOpenSourceFile);
            this.Controls.Add(this.txtOutputFilename);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.grpItems);
            this.Controls.Add(this.btnRemoveDeleted);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.txtEncodeLogProgress);
            this.Controls.Add(this.txtEncodeProgress);
            this.Controls.Add(this.txtEncodeLog);
            this.Controls.Add(this.btnEncodeAll);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtMediaInfo);
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
            this.MinimumSize = new System.Drawing.Size(800, 800);
            this.Name = "FrmMain";
            this.Text = "gMediaTools";
            this.grpItems.ResumeLayout(false);
            this.tlpItems.ResumeLayout(false);
            this.pnlItemActions.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnEncodeAll;
        private System.Windows.Forms.TextBox txtEncodeLog;
        private GTextBox txtEncodeProgress;
        private System.Windows.Forms.TextBox txtEncodeLogProgress;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnItemRemove;
        private System.Windows.Forms.Button btnRemoveDeleted;
        private System.Windows.Forms.GroupBox grpItems;
        private System.Windows.Forms.TableLayoutPanel tlpItems;
        private System.Windows.Forms.Panel pnlItemActions;
        private System.Windows.Forms.Button btnItemDown;
        private System.Windows.Forms.Button btnItempUp;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.RichTextBox txtOutputFilename;
        private System.Windows.Forms.Button btnOpenSourceFile;
        private System.Windows.Forms.Button btnOpenOutputFile;
        private System.Windows.Forms.Button btnDeleteTempFiles;
        private System.Windows.Forms.Button btnDeleteOutputFile;
        private System.Windows.Forms.Button btnDeleteAllCompletedFiles;
    }
}


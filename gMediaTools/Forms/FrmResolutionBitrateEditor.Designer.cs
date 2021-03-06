﻿namespace gMediaTools.Forms
{
    partial class FrmResolutionBitrateEditor
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
            this.txtWidth = new gMediaTools.GTextBox();
            this.txtHeight = new gMediaTools.GTextBox();
            this.txtBitrate = new gMediaTools.GTextBox();
            this.lstCurveData = new System.Windows.Forms.ListBox();
            this.lblX = new System.Windows.Forms.Label();
            this.lblEquals = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblBitrate = new System.Windows.Forms.Label();
            this.cmbCurveFittingType = new System.Windows.Forms.ComboBox();
            this.lblCurveFittingType = new System.Windows.Forms.Label();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lblPreviewBitrate = new System.Windows.Forms.Label();
            this.lblPreviewHeight = new System.Windows.Forms.Label();
            this.lblPreviewWidth = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPreviewBitRate = new gMediaTools.GTextBox();
            this.txtPreviewHeight = new gMediaTools.GTextBox();
            this.txtPreviewWidth = new gMediaTools.GTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.grpPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWidth
            // 
            this.txtWidth.DataObject = null;
            this.txtWidth.Decimals = 0;
            this.txtWidth.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtWidth.Int32Value = 0;
            this.txtWidth.Int64Value = ((long)(0));
            this.txtWidth.Location = new System.Drawing.Point(12, 54);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(100, 23);
            this.txtWidth.TabIndex = 0;
            this.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtWidth.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // txtHeight
            // 
            this.txtHeight.DataObject = null;
            this.txtHeight.Decimals = 0;
            this.txtHeight.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtHeight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtHeight.Int32Value = 0;
            this.txtHeight.Int64Value = ((long)(0));
            this.txtHeight.Location = new System.Drawing.Point(138, 54);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 23);
            this.txtHeight.TabIndex = 1;
            this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHeight.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // txtBitrate
            // 
            this.txtBitrate.DataObject = null;
            this.txtBitrate.Decimals = 0;
            this.txtBitrate.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtBitrate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtBitrate.Int32Value = 0;
            this.txtBitrate.Int64Value = ((long)(0));
            this.txtBitrate.Location = new System.Drawing.Point(273, 54);
            this.txtBitrate.Name = "txtBitrate";
            this.txtBitrate.Size = new System.Drawing.Size(106, 23);
            this.txtBitrate.TabIndex = 2;
            this.txtBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBitrate.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // lstCurveData
            // 
            this.lstCurveData.FormattingEnabled = true;
            this.lstCurveData.ItemHeight = 15;
            this.lstCurveData.Location = new System.Drawing.Point(12, 128);
            this.lstCurveData.Name = "lstCurveData";
            this.lstCurveData.Size = new System.Drawing.Size(367, 169);
            this.lstCurveData.TabIndex = 3;
            this.lstCurveData.SelectedIndexChanged += new System.EventHandler(this.lstCurveData_SelectedIndexChanged);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(118, 54);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(14, 15);
            this.lblX.TabIndex = 4;
            this.lblX.Text = "X";
            // 
            // lblEquals
            // 
            this.lblEquals.AutoSize = true;
            this.lblEquals.Location = new System.Drawing.Point(244, 57);
            this.lblEquals.Name = "lblEquals";
            this.lblEquals.Size = new System.Drawing.Size(23, 15);
            this.lblEquals.TabIndex = 5;
            this.lblEquals.Text = "=>";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(396, 128);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 39);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(396, 214);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 39);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(396, 171);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 39);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Location = new System.Drawing.Point(396, 258);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(100, 39);
            this.btnDefaults.TabIndex = 9;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(14, 36);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(39, 15);
            this.lblWidth.TabIndex = 10;
            this.lblWidth.Text = "Width";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(135, 36);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(43, 15);
            this.lblHeight.TabIndex = 11;
            this.lblHeight.Text = "Height";
            // 
            // lblBitrate
            // 
            this.lblBitrate.AutoSize = true;
            this.lblBitrate.Location = new System.Drawing.Point(271, 36);
            this.lblBitrate.Name = "lblBitrate";
            this.lblBitrate.Size = new System.Drawing.Size(77, 15);
            this.lblBitrate.TabIndex = 12;
            this.lblBitrate.Text = "Bitrate (kbps)";
            // 
            // cmbCurveFittingType
            // 
            this.cmbCurveFittingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurveFittingType.FormattingEnabled = true;
            this.cmbCurveFittingType.Location = new System.Drawing.Point(148, 7);
            this.cmbCurveFittingType.Name = "cmbCurveFittingType";
            this.cmbCurveFittingType.Size = new System.Drawing.Size(231, 23);
            this.cmbCurveFittingType.TabIndex = 13;
            this.cmbCurveFittingType.SelectedIndexChanged += new System.EventHandler(this.cmbCurveFittingType_SelectedIndexChanged);
            // 
            // lblCurveFittingType
            // 
            this.lblCurveFittingType.AutoSize = true;
            this.lblCurveFittingType.Location = new System.Drawing.Point(10, 15);
            this.lblCurveFittingType.Name = "lblCurveFittingType";
            this.lblCurveFittingType.Size = new System.Drawing.Size(102, 15);
            this.lblCurveFittingType.TabIndex = 14;
            this.lblCurveFittingType.Text = "Curve Fitting Type";
            // 
            // picPreview
            // 
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(3, 19);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(387, 268);
            this.picPreview.TabIndex = 15;
            this.picPreview.TabStop = false;
            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this.picPreview);
            this.grpPreview.Location = new System.Drawing.Point(509, 7);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(393, 290);
            this.grpPreview.TabIndex = 16;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview Curve";
            // 
            // lblPreviewBitrate
            // 
            this.lblPreviewBitrate.AutoSize = true;
            this.lblPreviewBitrate.Location = new System.Drawing.Point(271, 80);
            this.lblPreviewBitrate.Name = "lblPreviewBitrate";
            this.lblPreviewBitrate.Size = new System.Drawing.Size(121, 15);
            this.lblPreviewBitrate.TabIndex = 24;
            this.lblPreviewBitrate.Text = "Preview Bitrate (kbps)";
            // 
            // lblPreviewHeight
            // 
            this.lblPreviewHeight.AutoSize = true;
            this.lblPreviewHeight.Location = new System.Drawing.Point(135, 80);
            this.lblPreviewHeight.Name = "lblPreviewHeight";
            this.lblPreviewHeight.Size = new System.Drawing.Size(87, 15);
            this.lblPreviewHeight.TabIndex = 23;
            this.lblPreviewHeight.Text = "Preview Height";
            // 
            // lblPreviewWidth
            // 
            this.lblPreviewWidth.AutoSize = true;
            this.lblPreviewWidth.Location = new System.Drawing.Point(15, 80);
            this.lblPreviewWidth.Name = "lblPreviewWidth";
            this.lblPreviewWidth.Size = new System.Drawing.Size(83, 15);
            this.lblPreviewWidth.TabIndex = 22;
            this.lblPreviewWidth.Text = "Preview Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(244, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 15);
            this.label4.TabIndex = 21;
            this.label4.Text = "=>";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(118, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "X";
            // 
            // txtPreviewBitRate
            // 
            this.txtPreviewBitRate.DataObject = null;
            this.txtPreviewBitRate.Decimals = 0;
            this.txtPreviewBitRate.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtPreviewBitRate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtPreviewBitRate.Int32Value = 0;
            this.txtPreviewBitRate.Int64Value = ((long)(0));
            this.txtPreviewBitRate.Location = new System.Drawing.Point(273, 98);
            this.txtPreviewBitRate.Name = "txtPreviewBitRate";
            this.txtPreviewBitRate.ReadOnly = true;
            this.txtPreviewBitRate.Size = new System.Drawing.Size(106, 23);
            this.txtPreviewBitRate.TabIndex = 19;
            this.txtPreviewBitRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPreviewBitRate.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            // 
            // txtPreviewHeight
            // 
            this.txtPreviewHeight.DataObject = null;
            this.txtPreviewHeight.Decimals = 0;
            this.txtPreviewHeight.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtPreviewHeight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtPreviewHeight.Int32Value = 0;
            this.txtPreviewHeight.Int64Value = ((long)(0));
            this.txtPreviewHeight.Location = new System.Drawing.Point(138, 98);
            this.txtPreviewHeight.Name = "txtPreviewHeight";
            this.txtPreviewHeight.Size = new System.Drawing.Size(100, 23);
            this.txtPreviewHeight.TabIndex = 18;
            this.txtPreviewHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPreviewHeight.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            this.txtPreviewHeight.TextChanged += new System.EventHandler(this.txtPreviewHeight_TextChanged);
            // 
            // txtPreviewWidth
            // 
            this.txtPreviewWidth.DataObject = null;
            this.txtPreviewWidth.Decimals = 0;
            this.txtPreviewWidth.DecimalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtPreviewWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtPreviewWidth.Int32Value = 0;
            this.txtPreviewWidth.Int64Value = ((long)(0));
            this.txtPreviewWidth.Location = new System.Drawing.Point(12, 98);
            this.txtPreviewWidth.Name = "txtPreviewWidth";
            this.txtPreviewWidth.Size = new System.Drawing.Size(100, 23);
            this.txtPreviewWidth.TabIndex = 17;
            this.txtPreviewWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPreviewWidth.TextBoxType = gMediaTools.GTextBox.GTextBoxType.Numeric;
            this.txtPreviewWidth.TextChanged += new System.EventHandler(this.txtPreviewWidth_TextChanged);
            // 
            // FrmResolutionBitrateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(914, 307);
            this.Controls.Add(this.lblPreviewBitrate);
            this.Controls.Add(this.lblPreviewHeight);
            this.Controls.Add(this.lblPreviewWidth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPreviewBitRate);
            this.Controls.Add(this.txtPreviewHeight);
            this.Controls.Add(this.txtPreviewWidth);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.lblCurveFittingType);
            this.Controls.Add(this.cmbCurveFittingType);
            this.Controls.Add(this.lblBitrate);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblEquals);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.lstCurveData);
            this.Controls.Add(this.txtBitrate);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtWidth);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Name = "FrmResolutionBitrateEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resolution - Bitrate Editor";
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.grpPreview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GTextBox txtWidth;
        private GTextBox txtHeight;
        private GTextBox txtBitrate;
        private System.Windows.Forms.ListBox lstCurveData;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblEquals;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblBitrate;
        private System.Windows.Forms.ComboBox cmbCurveFittingType;
        private System.Windows.Forms.Label lblCurveFittingType;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.Label lblPreviewBitrate;
        private System.Windows.Forms.Label lblPreviewHeight;
        private System.Windows.Forms.Label lblPreviewWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private GTextBox txtPreviewBitRate;
        private GTextBox txtPreviewHeight;
        private GTextBox txtPreviewWidth;
    }
}

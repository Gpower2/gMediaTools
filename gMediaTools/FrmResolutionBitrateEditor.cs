using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using gMediaTools.Services;
using Newtonsoft.Json;

namespace gMediaTools
{
    public partial class FrmResolutionBitrateEditor : gMediaTools.BaseForm
    {
        private readonly CurveFittingRepository _curveFittingRepo = new CurveFittingRepository();

        private readonly CurveFittingPreviewService _curveFittingPreviewService = new CurveFittingPreviewService();

        private CurveFittingSettings _curveFittingSettings = new CurveFittingSettings();

        private bool _ignoreEvents = false;

        public FrmResolutionBitrateEditor()
        {
            try
            {
                _ignoreEvents = true;

                InitializeComponent();

                _curveFittingSettings = _curveFittingRepo.GetCurveFittingSettings();

                cmbCurveFittingType.DataSource = Enum.GetValues(typeof(CurveFittingType));
                cmbCurveFittingType.SelectedItem = _curveFittingSettings.CurveFittingType;

                _ignoreEvents = false;

                lstCurveData.DataSource = _curveFittingSettings.Data;

                picPreview.Image = _curveFittingPreviewService.GetPreviewImage(_curveFittingSettings, picPreview.Width, picPreview.Height);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);

                _ignoreEvents = false;
            }
        }

        private void ClearFields()
        {
            txtWidth.Clear();
            txtHeight.Clear();
            txtBitrate.Clear();
        }

        private void FillFieldsFromData(CurveFittingModel data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Empty data!");
            }

            txtWidth.Int32Value = data.Width;
            txtHeight.Int32Value = data.Height;
            txtBitrate.Int32Value = data.Bitrate / 1000;
        }

        private CurveFittingModel GetDataFromFields()
        {
            return new CurveFittingModel()
            {
                Width = txtWidth.Int32Value,
                Height = txtHeight.Int32Value,
                Bitrate = txtBitrate.Int32Value * 1000
            };
        }

        private void cmbCurveFittingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ignoreEvents) return;

            if (cmbCurveFittingType.SelectedIndex == -1)
            {
                return;
            }

            try
            {
                _curveFittingSettings.CurveFittingType = (CurveFittingType)cmbCurveFittingType.SelectedItem;

                _curveFittingRepo.SaveCurveFittingData(_curveFittingSettings);

                picPreview.Image = _curveFittingPreviewService.GetPreviewImage(_curveFittingSettings, picPreview.Width, picPreview.Height);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);            
            }
        }

        private void lstCurveData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ignoreEvents) return;

            if (lstCurveData.SelectedIndex == -1)
            {
                ClearFields();
                return;
            }

            try
            {
                var data = lstCurveData.SelectedItem as CurveFittingModel;

                FillFieldsFromData(data);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtWidth.Int32Value < 10)
                {
                    throw new Exception("Width can't be less than 10 pixels!");
                }
                if (txtHeight.Int32Value < 10)
                {
                    throw new Exception("Height can't be less than 10 pixels!");
                }
                if (txtBitrate.Int32Value < 100)
                {
                    throw new Exception("Bitrate can't be less than 100 kbps!");
                }

                var data = GetDataFromFields();

                if (_curveFittingSettings.Data.Any(x => x.Width == data.Width && x.Height == data.Height))
                {
                    throw new Exception($"There is already a record for resolution {data.Width} X {data.Height}!");
                }

                _curveFittingSettings.Data.Add(data);

                _curveFittingRepo.SaveCurveFittingData(_curveFittingSettings);

                lstCurveData.DataSource = null;
                lstCurveData.DataSource = _curveFittingSettings.Data;

                lstCurveData.Refresh();

                picPreview.Image = _curveFittingPreviewService.GetPreviewImage(_curveFittingSettings, picPreview.Width, picPreview.Height);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lstCurveData.SelectedIndex == -1)
            {
                return;
            }

            try
            {
                var data = lstCurveData.SelectedItem as CurveFittingModel;

                data.Width = txtWidth.Int32Value;
                data.Height = txtHeight.Int32Value;
                data.Bitrate = txtBitrate.Int32Value * 1000;

                _curveFittingSettings.CurveFittingType = (CurveFittingType)cmbCurveFittingType.SelectedItem;

                _curveFittingRepo.SaveCurveFittingData(_curveFittingSettings);

                int idx = lstCurveData.SelectedIndex;

                lstCurveData.DataSource = null;
                lstCurveData.DataSource = _curveFittingSettings.Data;

                lstCurveData.Refresh();

                lstCurveData.SelectedIndex = idx;

                picPreview.Image = _curveFittingPreviewService.GetPreviewImage(_curveFittingSettings, picPreview.Width, picPreview.Height);

            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstCurveData.SelectedIndex == -1)
            {
                return;
            }

            try
            {
                _curveFittingSettings.Data.Remove(lstCurveData.SelectedItem as CurveFittingModel);

                _curveFittingRepo.SaveCurveFittingData(_curveFittingSettings);

                lstCurveData.DataSource = null;
                lstCurveData.DataSource = _curveFittingSettings.Data;

                lstCurveData.Refresh();

                picPreview.Image = _curveFittingPreviewService.GetPreviewImage(_curveFittingSettings, picPreview.Width, picPreview.Height);

            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            try
            {
                _curveFittingSettings = _curveFittingRepo.GetDefaultCurveFittingSettings();

                _curveFittingRepo.SaveCurveFittingData(_curveFittingSettings);

                lstCurveData.DataSource = null;
                lstCurveData.DataSource = _curveFittingSettings.Data;

                lstCurveData.Refresh();

                cmbCurveFittingType.SelectedItem = _curveFittingSettings.CurveFittingType;

                picPreview.Image = _curveFittingPreviewService.GetPreviewImage(_curveFittingSettings, picPreview.Width, picPreview.Height);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }
    }
}

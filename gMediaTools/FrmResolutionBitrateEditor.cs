using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace gMediaTools
{
    public partial class FrmResolutionBitrateEditor : gMediaTools.BaseForm
    {
        private const string _CurveFittingDataFilename = "curveFittingData.json";

        // Get the Data for calculating the CurveFittingFunction
        private static readonly List<CurveFittingModel> _DefaultCurveFittingData = new List<CurveFittingModel>()
        {
            {new CurveFittingModel(640,480, 1200) },
            {new CurveFittingModel( 848,480, 1500 ) },
            {new CurveFittingModel( 1280,720, 2500 ) },
            {new CurveFittingModel( 1920,1080, 4000 ) }
        };

        public FrmResolutionBitrateEditor()
        {
            InitializeComponent();

            List<CurveFittingModel> curveData = new List<CurveFittingModel>();
            if (!File.Exists(_CurveFittingDataFilename))
            {
                curveData = _DefaultCurveFittingData;
            }            
            using (StreamReader sr = new StreamReader(_CurveFittingDataFilename))
            {
                curveData = JsonConvert.DeserializeObject<List<CurveFittingModel>>(sr.ReadToEnd());
            }

            lstCurveData.DataSource = curveData;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Factories;
using gMediaTools.Models.CurveFitting;
using Newtonsoft.Json;

namespace gMediaTools.Services.CurveFitting
{
    public class CurveFittingRepository
    {
        private const string _CurveFittingDataFilename = "curveFittingData.json";

        // Get the Data for calculating the CurveFittingFunction
        private static readonly List<CurveFittingModel> _DefaultCurveFittingData = new List<CurveFittingModel>()
        {
            {new CurveFittingModel(640,480, 1200 * 1000) },
            {new CurveFittingModel( 848,480, 1500 * 1000) },
            {new CurveFittingModel( 1280,720, 2500 * 1000) },
            {new CurveFittingModel( 1920,1080, 4000 * 1000) }
        };

        private static readonly CurveFittingType _DefaultCurveFittingType = CurveFittingType.Logarithmic;

        public CurveFittingSettings GetDefaultCurveFittingSettings()
        {
            return new CurveFittingSettings()
            {
                CurveFittingType = _DefaultCurveFittingType,
                Data = _DefaultCurveFittingData
            };
        }

        public CurveFittingSettings GetCurveFittingSettings()
        {
            if (!File.Exists(_CurveFittingDataFilename))
            {
                var defaultSettings = new CurveFittingSettings()
                {
                    CurveFittingType = _DefaultCurveFittingType,
                    Data = _DefaultCurveFittingData
                };

                SaveCurveFittingData(defaultSettings);

                return defaultSettings;
            }

            using (StreamReader sr = new StreamReader(_CurveFittingDataFilename))
            {
                return JsonConvert.DeserializeObject<CurveFittingSettings>(sr.ReadToEnd());
            }
        }

        public void SaveCurveFittingData(CurveFittingSettings curveSettings)
        {
            using (StreamWriter sw = new StreamWriter(_CurveFittingDataFilename))
            {
                sw.Write(JsonConvert.SerializeObject(curveSettings));
            }
        }
    }
}

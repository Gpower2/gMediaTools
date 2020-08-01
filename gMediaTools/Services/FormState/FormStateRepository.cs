using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Models;
using Newtonsoft.Json;

namespace gMediaTools.Services.FormState
{
    public class FormStateRepository
    {

        private const string _FormStateDataFilename = "formStateData.json";

        public FormStateInfo GetFormStateInfo()
        {
            if (!File.Exists(_FormStateDataFilename))
            {
                var defaultSettings = new FormStateInfo();

                SaveFormStateInfo(defaultSettings);

                return defaultSettings;
            }

            using (StreamReader sr = new StreamReader(_FormStateDataFilename))
            {
                return JsonConvert.DeserializeObject<FormStateInfo>(sr.ReadToEnd());
            }
        }

        public void SaveFormStateInfo(FormStateInfo curveSettings)
        {
            using (StreamWriter sw = new StreamWriter(_FormStateDataFilename))
            {
                sw.Write(JsonConvert.SerializeObject(curveSettings));
            }
        }
    }
}

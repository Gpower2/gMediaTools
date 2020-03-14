using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Services;

namespace gMediaTools.Models.CurveFitting
{
    public class CurveFittingSettings
    {
        public List<CurveFittingModel> Data { get; set; } = new List<CurveFittingModel>();

        public CurveFittingType CurveFittingType { get; set; }
    }
}

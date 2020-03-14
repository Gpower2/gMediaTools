using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.CurveFitting
{
    public interface ICurveFittingService
    {
        Func<double, double> GetCurveFittingFunction(IDictionary<double, double> data);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.CurveFitting
{
    public class LogarithmicCurveFittingService: ICurveFittingService
    {
        public Func<double, double> GetCurveFittingFunction(IDictionary<double, double> data)
        {
            // Check data existance
            if (data == null || !data.Any())
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Function:
            // y = a + b * log (x)

            // x = data.Key
            // y = data.Value

            double a = 0;
            double b = 0;

            long n = data.LongCount();

            b = (n * data.Sum(d => d.Value * Math.Log(d.Key))) - (data.Sum(d => d.Value) * data.Sum(d => Math.Log(d.Key)));
            b /= (n * data.Sum(d => Math.Pow(Math.Log(d.Key), 2))) - Math.Pow(data.Sum(d => Math.Log(d.Key)), 2);

            a = data.Sum(d => d.Value) - (b * data.Sum(d => Math.Log(d.Key)));
            a /= n;

            return (x) => a + b * Math.Log(x);
        }
    }
}

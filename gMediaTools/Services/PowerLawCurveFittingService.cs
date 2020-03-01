using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services
{
    public class PowerLawCurveFittingService : ICurveFittingService
    {
        public Func<double, double> GetCurveFittingFunction(IDictionary<double, double> data)
        {
            // Check data existance
            if (data == null || !data.Any())
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Function:
            // y = (e ^ a) * (x ^ b)

            // x = data.Key
            // y = data.Value

            double a = 0;
            double b = 0;

            long n = data.LongCount();

            b = (n * data.Sum(d => Math.Log(d.Key) * Math.Log(d.Value))) - (data.Sum(d => Math.Log(d.Key)) * data.Sum(d => Math.Log(d.Value)));
            b /= (n * data.Sum(d => Math.Pow(Math.Log(d.Key), 2))) - Math.Pow(data.Sum(d => Math.Log(d.Key)), 2);

            a = data.Sum(d => Math.Log(d.Value)) - (b * data.Sum(d => Math.Log(d.Key)));
            a /= n;

            return (x) =>  Math.Pow(Math.E, a) * Math.Pow(x, b);
        }
    }
}

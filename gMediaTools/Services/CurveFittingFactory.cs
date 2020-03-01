using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services
{
    public enum CurveFittingType
    {
        Logarithmic,
        Linear,
        PowerLaw
    }

    public class CurveFittingFactory
    {
        public ICurveFittingService GetCurveFittingService(CurveFittingType curveFittingType)
        {
            switch (curveFittingType)
            {
                case CurveFittingType.Logarithmic:
                    return new LogarithmicCurveFittingService();
                case CurveFittingType.Linear:
                    break;
                case CurveFittingType.PowerLaw:
                    return new PowerLawCurveFittingService();
                default:
                    break;
            }

            throw new ArgumentException($"Unsupported curveFittingType: {curveFittingType}", nameof(curveFittingType));
        }
    }
}

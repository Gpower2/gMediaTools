using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Services;
using gMediaTools.Services.CurveFitting;

namespace gMediaTools.Factories
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
                    return ServiceFactory.GetService<LogarithmicCurveFittingService>();
                case CurveFittingType.Linear:
                    break;
                case CurveFittingType.PowerLaw:
                    return ServiceFactory.GetService<PowerLawCurveFittingService>();
                default:
                    break;
            }

            throw new ArgumentException($"Unsupported curveFittingType: {curveFittingType}", nameof(curveFittingType));
        }
    }
}

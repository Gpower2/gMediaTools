using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Extensions
{
    public static class DictionaryExtensions
    {

        public static Dictionary<double, double> ScaleValues(this Dictionary<double, double> data, double minX, double maxX, double minY, double maxY, double aX, double bX, double aY, double bY)
        {
            return data.ToDictionary(
                x => aX + (((x.Key - minX) * ((double)bX)) / (maxX - minX))
                , y => (aY + (((y.Value - minY) * ((double)bY)) / (maxY - minY)))
            );
        }

        public static Dictionary<double, double> InverseYValues(this Dictionary<double, double> data, double imgHeight)
        {
            return data.ToDictionary(
                x => x.Key,
                y => imgHeight - y.Value
            );
        }
    }
}

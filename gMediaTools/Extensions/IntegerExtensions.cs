using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Extensions
{
    public static class IntegerExtensions
    {
        public static int ClosestModValue(this int intValue, int mod)
        {
            int remainder = intValue % mod;
            if (remainder == 0)
            {
                return intValue;
            }

            var divResult = intValue / mod;

            return divResult * mod;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Extensions
{
    public static class DecimalExtensions
    {
        public static int GetDecimals(this decimal decimalNumber)
        {
            decimalNumber = Math.Abs(decimalNumber);  // make sure it is positive
            decimalNumber -= (int)decimalNumber;    // remove the integer part of the number
            int decimalPlaces = 0;
            while (decimalNumber > 0)
            {
                decimalPlaces++;
                decimalNumber *= 10;
                decimalNumber -= (int)decimalNumber;
            }
            return decimalPlaces;
        }
    }
}

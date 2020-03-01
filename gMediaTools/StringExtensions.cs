using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools
{
    public static class StringExtensions
    {
        public static string PrepareStringForNumericParse(this string argString)
        {
            if ((argString.Contains(".")))
            {
                // if it's more than one, then the '.' is definetely a thousand separator
                if ((argString.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length > 2))
                {
                    // Remove the thousand separator and replace the decimal point separator
                    return argString.Replace(".", string.Empty).Replace(",", ".");
                }
                else if ((argString.Contains(",")))
                {
                    // if it also contains ',' check to see which is first
                    if ((argString.IndexOf(",") < argString.IndexOf(".")))
                    {
                        // if the ',' is before the '.', then the thousand separator is ','
                        // Remove the thousand separator and leave the decimal point separator
                        return argString.Replace(",", string.Empty);
                    }
                    else
                    {
                        // if the ',' is after the '.', then the thousand separator is '.'
                        // Remove the thousand separator and replace the decimal point separator
                        return argString.Replace(".", string.Empty).Replace(",", ".");
                    }
                }
                else
                {
                    // if we have only a '.' present, we assume it is a decimal point separator
                    // let it be
                    return argString;
                }
            }
            else if ((argString.Contains(",")))
            {
                // if it's more than one, then the ',' is definetely a thousand separator
                if ((argString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length > 2))
                {
                    // Remove the thousand separator and leave the decimal point separator
                    return argString.Replace(",", string.Empty);
                }
                else
                {
                    // if we have only a ',' present, we assume it is a decimal point separator
                    // Replace the decimal point separator
                    return argString.Replace(",", ".");
                }
            }
            else
            {
                // if neither '.' or ',' are found, then return the string as is
                return argString;
            }
        }

        public static Int32 GetDecimals(this Decimal argDecimal)
        {
            argDecimal = Math.Abs(argDecimal);  //make sure it is positive.
            argDecimal -= (Int32)argDecimal;    //remove the integer part of the number.
            Int32 decimalPlaces = 0;
            while (argDecimal > 0)
            {
                decimalPlaces++;
                argDecimal *= 10;
                argDecimal -= (Int32)argDecimal;
            }
            return decimalPlaces;
        }
    }
}

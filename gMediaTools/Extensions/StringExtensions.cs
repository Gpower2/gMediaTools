using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Extensions
{
    public static class StringExtensions
    {
        public static readonly CultureInfo INV_CULTURE = CultureInfo.InvariantCulture;

        public static string PrepareStringForNumericParse(this string stringToParse)
        {
            if ((stringToParse.Contains(".")))
            {
                // if it's more than one, then the '.' is definetely a thousand separator
                if ((stringToParse.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length > 2))
                {
                    // Remove the thousand separator and replace the decimal point separator
                    return stringToParse.Replace(".", string.Empty).Replace(",", ".");
                }
                else if ((stringToParse.Contains(",")))
                {
                    // if it also contains ',' check to see which is first
                    if ((stringToParse.IndexOf(",") < stringToParse.IndexOf(".")))
                    {
                        // if the ',' is before the '.', then the thousand separator is ','
                        // Remove the thousand separator and leave the decimal point separator
                        return stringToParse.Replace(",", string.Empty);
                    }
                    else
                    {
                        // if the ',' is after the '.', then the thousand separator is '.'
                        // Remove the thousand separator and replace the decimal point separator
                        return stringToParse.Replace(".", string.Empty).Replace(",", ".");
                    }
                }
                else
                {
                    // if we have only a '.' present, we assume it is a decimal point separator
                    // let it be
                    return stringToParse;
                }
            }
            else if ((stringToParse.Contains(",")))
            {
                // if it's more than one, then the ',' is definetely a thousand separator
                if ((stringToParse.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length > 2))
                {
                    // Remove the thousand separator and leave the decimal point separator
                    return stringToParse.Replace(",", string.Empty);
                }
                else
                {
                    // if we have only a ',' present, we assume it is a decimal point separator
                    // Replace the decimal point separator
                    return stringToParse.Replace(",", ".");
                }
            }
            else
            {
                // if neither '.' or ',' are found, then return the string as is
                return stringToParse;
            }
        }

        public static decimal ParseDecimal(this string stringToParse)
        {
            return decimal.Parse(stringToParse.PrepareStringForNumericParse(), NumberStyles.Any, INV_CULTURE);
        }

        public static bool TryParseDecimal(this string stringToParse, out decimal decimalValue)
        {
            return decimal.TryParse(stringToParse.PrepareStringForNumericParse(), NumberStyles.Any, INV_CULTURE, out decimalValue);
        }

        public static string GetOnlyDigits(this string stringToProcess)
        {
            if (stringToProcess == null)
            {
                return stringToProcess;
            }

            return new string(stringToProcess.Where(c => char.IsDigit(c)).ToArray());
        }

        public static string RemoveSpaces(this string stringToProcess)
        {
            while(stringToProcess.Contains(" "))
            {
                stringToProcess = stringToProcess.Replace(" ", "");
            }

            return stringToProcess;
        }
    }
}

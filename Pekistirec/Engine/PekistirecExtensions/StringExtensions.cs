using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec
{
    public static class StringExtensions
    {
        public static int ToInteger(this string deger, int defaultValue)
        {
            int returnValue = defaultValue;

            if (int.TryParse(deger, out returnValue))
            {
                return returnValue;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
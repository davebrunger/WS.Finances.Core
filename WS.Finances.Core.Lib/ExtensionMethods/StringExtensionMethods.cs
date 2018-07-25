using System;
using System.Globalization;

namespace WS.Finances.Core.Lib.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static int? ToMonthNumber(this string month)
        {
            if (int.TryParse(month, out var result))
            {
                if (result >= 1 && result <= 12)
                {
                    return result;
                }
            }
            if (DateTime.TryParseExact(month, new[] { "MMMM", "MMM" }, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime.Month;
            }
            return null;
        }
        
        public static int? ToInteger(this string integer)
        {
            if (int.TryParse(integer, out var result))
            {
                return result;
            }
            return null;
        }
    }
}

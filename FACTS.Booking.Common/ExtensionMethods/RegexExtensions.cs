using System.Text.RegularExpressions;

namespace FACTS.GenericBooking.Common.ExtensionMethods
{
    public static class RegexExtensions
    {
        public static decimal ExtractDecimal(this string value, Regex regex, string groupName)
        {
            Match match = regex.Match(value);
            if (!match.Success)
            {
                return 0;
            }

            return decimal.TryParse(match.Groups[groupName].Value, out decimal extract) ? extract : 0;
        }

        public static int ExtractInt(this string value, Regex regex, string groupName)
        {
            Match match = regex.Match(value);
            if (!match.Success)
            {
                return 0;
            }

            return int.TryParse(match.Groups[groupName].Value, out int extract) ? extract : 0;
        }

        public static decimal? ExtractNullableDecimal(this string value, Regex regex, string groupName)
        {
            Match match = regex.Match(value);
            if (!match.Success)
            {
                return null;
            }

            return decimal.TryParse(match.Groups[groupName].Value, out decimal extract) ? extract : (decimal?) null;
        }

        public static string ExtractString(this string value, Regex regex, string groupName)
        {
            Match match = regex.Match(value);
            if (!match.Success)
            {
                return "";
            }

            return match.Groups[groupName].Value;
        }
    }
}
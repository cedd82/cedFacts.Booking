using System;
using System.Globalization;
using System.Linq;

using FACTS.GenericBooking.Common.Constants;

namespace FACTS.GenericBooking.Common.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool IsJson(this string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        public static string ToIngresDateString(this string value)
        {
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outDate))
            {
                outDate = DateTime.Now;
            }

            return outDate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToIngresDateStringAddSecond(this string value)
        {
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outDate))
            {
                outDate = DateTime.Now;
            }

            outDate = outDate.AddSeconds(1);
            return outDate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToIngresString(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value.Replace("'", "''") : string.Empty;
        }

        public static bool ToBoolean(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            value = value.Trim().ToUpper();
            return value switch
            {
                "TRUE" => true,
                "YES" => true,
                "Y" => true,
                "1" => true,
                "FALSE" => false,
                "NO" => false,
                "N" => false,
                "0" => false,
                _ => false
            };
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string TruncateTrim(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length <= maxLength ? value.Trim() : value.Substring(0, maxLength).Trim();
        }

        public static string MapState(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            value = value.ToUpper().Trim();
            return value switch
            {
                "NEW SOUTH WALES" => "NSW",
                "QUEENSLAND" => "QLD",
                "SOUTH AUSTRALIA" => "SA",
                "TASMANIA" => "TAS",
                "VICTORIA" => "VIC",
                "WESTERN AUSTRALIA" => "WA",
                "AUSTRALIAN CAPITAL TERRITORY" => "ACT",
                "NORTHERN TERRITORY" => "NT",
                _ => value
            };
        }

        public static string MapServiceType(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            value = value.ToLower();
            return value switch
            {
                "standard" => ServiceType.Standard,
                "express" => ServiceType.Express,
                "premiumenclosed" => ServiceType.PremiumEnclosed,
                _ => null
            };
        }

        public static string MapServiceTypeDescription(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            value = value.ToUpper();
            return value switch
            {
                ServiceType.Standard => "STANDARD",
                ServiceType.Express => "EXPRESS",
                ServiceType.PremiumEnclosed => "PREMIUMENCLOSED",
                _ => null
            };
        }

        public static string MapDestinationType(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            value = value.ToUpper();
            return value switch
            {
                "CUSTOMER" => LocationType.Customer,
                "DEPOT" => LocationType.Depot,
                _ => null
            };
        }

        public static string MapDestinationDescription(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            value = value.ToUpper();
            return value switch
            {
                LocationType.Customer => "CUSTOMER",
                LocationType.Depot => "DEPOT",
                _ => value
            };
        }

        public static bool IsValidAustralianState(this string value)
        {
            value = value.ToUpper().Trim();
            return AddressConsts.States.Contains(value);
        }
    }
}
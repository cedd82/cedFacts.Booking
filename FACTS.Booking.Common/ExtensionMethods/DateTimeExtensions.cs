using System;
using System.Globalization;

namespace FACTS.GenericBooking.Common.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTimeNoLocale(this string date, string formatter = "s")
        {
            return DateTime.ParseExact(date, formatter, CultureInfo.InvariantCulture);
        }

        public static DateTime ToNoLocale(this DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
        }

        public static DateTime? FromIngresDate(this DateTime value, string formatter = "s")
        {
            if (value.Year == 9999)
            {
                return null;
            }
            return DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
        }

        public static string ToIngresDate(this DateTime value)
        {
            return DateTime.SpecifyKind(value, DateTimeKind.Unspecified).ToString("yyyy-MM-dd HH:mm:ss");
        }
        
        public static string ToStringNoLocale(this DateTime localDate, string formatter = "s")
        {
            return DateTime.SpecifyKind(localDate, DateTimeKind.Unspecified).ToString(formatter);
        }

        // this was suggested but it throws exceptions
        //public static string ToIngresDateString(this DateTime value)
        //{
        //    return value.ToString("dd.MM.yyyy HH:mm:ss");
        //}
    }
}
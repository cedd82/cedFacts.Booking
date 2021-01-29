using System.Text.RegularExpressions;

namespace FACTS.GenericBooking.Common.Constants
{
    public static class QuoteRegex
    {
        public static readonly Regex TransportChargeRegex = new Regex(@"##T\$\s*(?<transportCharge>\d*)\.\d*T##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex InsuranceChargeRegex = new Regex(@"##I\$\s*(?<insuranceCharge>\d*\.\d*)I##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex SurchargeRegex = new Regex(@"##S\$\s*(?<surcharge>\d*\.\d*)S##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex GstRegex = new Regex(@"##G(?<gst>\d*\.\d*)G##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex MiscChargeRegex = new Regex(@"##M\$\s*(?<miscCharge>\d*\.\d*)M##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex TransitDaysRegex = new Regex(@"##D\s*(?<transitDays>\d*)D##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex PickUpDepotRegex = new Regex(@"##1(?<pickupDepot>.{3})1##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex DeliverDepotRegex = new Regex(@"##2(?<deliverDepot>.{3})2##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex RingCodeRegex = new Regex(@"##R(?<ringCode>.{3})R##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex RateCodeRegex = new Regex(@"##A(?<rateCode>.{3})A##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex RateRouteCodeRegex = new Regex(@"##O(?<rateRouteCode>.{3})O##", RegexOptions.Compiled | RegexOptions.Multiline);
        public static readonly Regex IsDiscountRegex = new Regex(@"##C(?<isDiscount>.{1})C##", RegexOptions.Compiled | RegexOptions.Multiline);
    }
}

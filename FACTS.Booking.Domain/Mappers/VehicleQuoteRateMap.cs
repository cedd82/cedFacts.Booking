using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Common.Constants;
using FACTS.GenericBooking.Common.ExtensionMethods;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Domain.Models.Quote;

namespace FACTS.GenericBooking.Domain.Mappers
{
    public static class VehicleQuoteRateMap
    {
        public static Result<VehicleRateDto> Map(string quote,
                                                      CommonAppSettings commonAppSettings,
                                                      string serviceType,
                                                      RateLocationTypeDto rateLocationType,
                                                      decimal insuranceExcess,
                                                      int vehicleRateCode,
                                                      int? quoteNumber,
                                                      string movementType)
        {
            decimal? transportCharge = quote.ExtractNullableDecimal(QuoteRegex.TransportChargeRegex, "transportCharge");
            if (!transportCharge.HasValue)
                return new Result<VehicleRateDto>(ErrorMessages.TransportChargesNotValid);

            decimal insuranceCharge = quote.ExtractDecimal(QuoteRegex.InsuranceChargeRegex, "insuranceCharge");
            decimal surcharge = quote.ExtractDecimal(QuoteRegex.SurchargeRegex, "surcharge");
            decimal? gst = quote.ExtractNullableDecimal(QuoteRegex.GstRegex, "gst");
            // misc charges includes car on-goer fee, wait times etc, just add onto the total charge instead of a different field to avoid confusion for consumers
            decimal miscCharge = quote.ExtractDecimal(QuoteRegex.MiscChargeRegex, "miscCharge");
            int transitDays = quote.ExtractInt(QuoteRegex.TransitDaysRegex, "transitDays");
            string pickupDepot = quote.ExtractString(QuoteRegex.PickUpDepotRegex, "pickupDepot");
            string deliveryDepot = quote.ExtractString(QuoteRegex.DeliverDepotRegex, "deliverDepot");
            string ringCode = quote.ExtractString(QuoteRegex.RingCodeRegex, "ringCode");
            string rateRouteCode = quote.ExtractString(QuoteRegex.RateRouteCodeRegex, "rateRouteCode");
            int isDiscount = quote.ExtractInt(QuoteRegex.IsDiscountRegex, "isDiscount");
            string rateCode = quote.ExtractString(QuoteRegex.RateCodeRegex, "rateCode");

            gst ??= (insuranceCharge + miscCharge + transportCharge + surcharge + insuranceExcess) / commonAppSettings.GST;
            decimal totalRate = transportCharge.Value + insuranceCharge + surcharge + miscCharge + insuranceExcess + gst.Value;

            VehicleRateDto vehicleRate = new()
            {
                ServiceType           = serviceType.MapServiceTypeDescription(),
                PickupType            = rateLocationType.Pickup.MapDestinationDescription(),
                DeliveryType          = rateLocationType.Delivery.MapDestinationDescription(),
                TotalRateIncludingGst = totalRate,
                TransitDays           = transitDays,
                TransportCharge       = transportCharge.Value,
                // properties not used in generic api
                InsuranceCharge       = insuranceCharge,
                Surcharge             = surcharge,
                GST                   = gst.Value,
                MiscCharge            = miscCharge,
                TotalRateExcludingGst = totalRate - gst.Value,
                PickupDepot           = pickupDepot,
                DeliveryDepot         = deliveryDepot,
                RingCode              = ringCode.Trim(),
                VehicleRateCode       = vehicleRateCode,
                RateCode              = rateCode,
                RateRouteCode         = rateRouteCode,
                IsDiscount            = isDiscount,
                IsSpotSpecial         = rateCode == QuoteConsts.SpotSpecial,
                MovementType          = movementType,
                QuoteNumber           = quoteNumber
            };
            return new Result<VehicleRateDto>(vehicleRate);
        }
    }
}
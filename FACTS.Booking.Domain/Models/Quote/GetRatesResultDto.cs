using System.Collections.Generic;

namespace FACTS.GenericBooking.Domain.Models.Quote
{
    public class GetRatesResultDto
    {
        public int AccountCustomerNumber { get; set; }
        public string AccountNumber { get; set; }
        public int MarketingCode { get; set; }
        public bool IsExpired { get; set; }
        public string RateGroupCode { get; set; }
        public LocationDto PickupAddress { get; set; }
        public LocationDto DeliveryAddress { get; set; }
        public VehicleQuoteRateDto Vehicle { get; set; }
    }

    public class VehicleQuoteRateDto
    {
        public string VehicleId { get; set; }
        public int VehicleValue { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int ModelCode { get; set; }
        public string VehicleType { get; set; }
        public int VehicleCode { get; set; }
        public bool IsDriveable { get; set; }
        public List<VehicleRateDto> Rates { get; set; }
    }

    public class LocationDto
    {
        public string LocationType { get; set; }
        public string AddressLine1 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string DepotAbbreviation { get; set; }
        public string DepotName { get; set; }
    }

    public class VehicleRateDto
    {
        public int? QuoteNumber { get; set; }
        public string ServiceType { get; set; }
        public string PickupType { get; set; }
        public string DeliveryType { get; set; }
        public int TransitDays { get; set; }
        public decimal TotalRateIncludingGst { get; set; }
        public decimal TotalRateExcludingGst { get; set; }
        public decimal TransportCharge { get; set; }
        public decimal InsuranceCharge { get; set; }
        public decimal Surcharge { get; set; }
        public decimal MiscCharge { get; set; }
        public decimal GST { get; set; }
        public string PickupDepot { get; set; }
        public string DeliveryDepot { get; set; }
        public string MovementType { get; set; }
        public string RingCode { get; set; }
        public int VehicleRateCode { get; set; }
        public string RateCode { get; set; }
        public string RateRouteCode { get; set; }
        public int IsDiscount { get; set; }
        public bool IsSpotSpecial { get; set; }
    }
}
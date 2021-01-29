using System.Collections.Generic;

namespace FACTS.GenericBooking.Api.Models.Quote
{
    public class GetRatesResponse
    {
        /// <example>AB1234</example>
        public string AccountNumber { get; set; }
        public LocationResponse PickupAddress { get; set; }
        public LocationResponse DeliveryAddress { get; set; }
        public VehicleRateResponse Vehicle { get; set; }
    }

    public class VehicleRateResponse
    {
        /// <example>42000</example>
        public int VehicleValue { get; set; }
        /// <example>FORD</example>
        public string Make { get; set; }
        /// <example>FALCON</example>
        public string Model { get; set; }
        /// <example>SEDAN</example>
        public string VehicleType { get; set; }
        /// <example>true</example>
        public bool IsDriveable { get; set; }
        public List<RateResponse> Rates { get; set; }
    }

    public class LocationResponse
    {
        public string AddressLine1 { get; set; } = null;
        /// <example>sydney</example>
        public string Suburb { get; set; }
        /// <example>NSW</example>
        public string State { get; set; }
        /// <example>2000</example>
        public string PostCode { get; set; }
    }

    public class RateResponse
    {
        /// <example>EXPRESS</example>
        public string ServiceType { get; set; }
        /// <example>CUSTOMER</example>
        public string PickupType { get; set; }
        /// <example>DEPOT</example>
        public string DeliveryType { get; set; }
        /// <example>2</example>
        public int TransitDays { get; set; }
        /// <example>1023.23</example>
        public decimal TotalRateIncludingGst { get; set; }
    }
}
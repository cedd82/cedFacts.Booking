namespace FACTS.GenericBooking.Api.Models.Quote
{
    public class CreateQuoteResponse
    {
        /// <example>AB1234</example>
        public string AccountNumber { get; set; }
        public ContactResponse Contact { get; set; }
        public LocationResponse PickupAddress { get; set; }
        public LocationResponse DeliveryAddress { get; set; }
        public VehicleQuoteResponse Vehicle { get; set; }
    }

    public class VehicleQuoteResponse
    {
        /// <example>VEH332XYZ1</example>
        public string VehicleId { get; set; }
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
        public QuoteRateResponse Rate { get; set; }
    }

    public class QuoteRateResponse
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
        /// <example>12312312</example>
        public int QuoteNumber { get; set; }
    }

    public class ContactResponse
    {
        /// <example>MR</example>
        public string Title { get; set; }
        /// <example>JOHN</example>
        public string FirstName { get; set; }
        /// <example>SMITH</example>
        public string LastName { get; set; }
        /// <example>test@test.com</example>
        public string Email { get; set; }
        /// <example>02</example>
        public string LandlinePhoneAreaCode { get; set; }
        /// <example>98751001</example>
        public string LandlinePhoneNumber { get; set; }
        /// <example>0148725777</example>
        public string MobileNumber { get; set; }
    }
}
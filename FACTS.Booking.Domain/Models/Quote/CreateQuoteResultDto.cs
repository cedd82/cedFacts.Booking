namespace FACTS.GenericBooking.Domain.Models.Quote
{
    public class CreateQuoteResultDto
    {
        public string AccountNumber { get; set; }
        public int MarketingCode { get; set; }
        public bool IsExpired { get; set; }
        public ContactDto Contact { get; set; }
        public LocationDto PickupAddress { get; set; }
        public LocationDto DeliveryAddress { get; set; }
        public VehicleQuoteDto Vehicle { get; set; }
    }

    public class VehicleQuoteDto
    {
        public string VehicleId { get; set; }
        public int VehicleValue { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VehicleType { get; set; }
        public bool IsDriveable { get; set; }
        public VehicleRateDto Rate { get; set; }
    }

    public class ContactDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LandlinePhoneAreaCode { get; set; }
        public string LandlinePhoneNumber { get; set; }
        public string MobileNumber { get; set; }
    }
}

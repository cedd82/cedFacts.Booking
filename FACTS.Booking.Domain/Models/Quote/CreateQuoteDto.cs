namespace FACTS.GenericBooking.Domain.Models.Quote
{
    public class CreateQuoteDto
    {
        public string AccountNumber { get; set; }
        public string ServiceType { get; set; }
        public string PickupType { get; set; }
        public string PickupAddressLine1 { get; set; }
        public string PickupSuburb { get; set; }
        public string PickupPostcode { get; set; }
        public string PickupState { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryAddressLine1 { get; set; }
        public string DeliverySuburb { get; set; }
        public string DeliveryPostcode { get; set; }
        public string DeliveryState { get; set; }
        public bool IsDriveable { get; set; }
        public bool IsModified { get; set; }
        public bool IsDamaged { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleType { get; set; }
        public int VehicleValue { get; set; }
        public string VehicleId { get; set; }
        public string OrderNumber { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LandlinePhoneAreaCode { get; set; }
        public string LandlinePhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
    }
}
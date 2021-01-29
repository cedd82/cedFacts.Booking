namespace FACTS.GenericBooking.Domain.Models.Customer
{
    public class CustomerDetailsDto
    {
        public int AccountCustomerNumber { get; set; }
        public string CustomerAbbreviation { get; set; }
        public string CustomerAlias { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneAreaCode { get; set; }
        public int IsInsuranceWaiver { get; set; }
        public string RateGroupCode { get; set; }
        public decimal DiscountValue { get; set; }
        public string ValueCode { get; set; }
        public decimal VehicleWarrantyAmount { get; set; }
        public int IsVoiceAlert { get; set; }
    }
}
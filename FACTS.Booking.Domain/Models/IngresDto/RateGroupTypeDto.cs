namespace FACTS.GenericBooking.Domain.Models.IngresDto
{
    public class RateGroupTypeDto
    {
        public string RateGroupCode { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public decimal VehicleValueLimit { get; set; }
        public decimal InsuranceExcessAmount { get; set; }
        public decimal VehicleValueExcess { get; set; }
        public decimal NilExcessCharge { get; set; }
        public int ApplySecondaryRateCode { get; set; }
    }
}
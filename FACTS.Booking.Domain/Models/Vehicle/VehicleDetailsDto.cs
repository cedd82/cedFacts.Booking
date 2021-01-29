namespace FACTS.GenericBooking.Domain.Models.Vehicle
{
    public class VehicleDetailsDto
    {
        public string Make { get; set; }
        public int MakeCode { get; set; }
        public string Model { get; set; }
        public int ModelCode { get; set; }
        public string Type { get; set; }
        public int VehicleCode { get; set; }
        public int RateCode { get; set; }
        public int RateCodeSecondary { get; set; }
        public decimal InsuranceExcess { get; set; }
        public decimal VehicleValue { get; set; }
    }
}
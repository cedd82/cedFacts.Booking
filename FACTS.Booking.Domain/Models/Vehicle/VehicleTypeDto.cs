namespace FACTS.GenericBooking.Domain.Models.Vehicle
{
    public class VehicleTypeDto
    {
        public string Make { get; set; }
        public int MakeCode { get; set; }
        public string Model { get; set; }
        public int ModelCode { get; set; }
        public string Type { get; set; }
        public int VehicleCode { get; set; }
        public int RateCode { get; set; }
        public int RateCodeSecondary { get; set; }
    }
}
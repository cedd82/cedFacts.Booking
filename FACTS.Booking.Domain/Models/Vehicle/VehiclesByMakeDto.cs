using System.Collections.Generic;

namespace FACTS.GenericBooking.Domain.Models.Vehicle
{
    public class VehiclesByMakeDto
    {
        public string Make { get; set; }
        public List<VehicleTypeDto> Models { get; set; }
    }

    //public class VehicleModelDto
    //{
    //    public string Model { get; set; }
    //    public int RateCode { get; set; }
    //    public string Type { get; set; }
    //    public int MakeCode { get; set; }
    //    public int ModelCode { get; set; }
    //    public int RateCodeSecondary { get; set; }
    //    public int VehicleCode { get; set; }
    //}
}
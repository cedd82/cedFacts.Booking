using System.Collections.Generic;

namespace FACTS.GenericBooking.Api.Models.Vehicle
{

    public class GetVehiclesByMakeResponse
    {
        /// <example>FORD</example>
        public string Make { get; set; }
        public List<VehicleModelResponse> Models { get; set; }
    }

    public class VehicleModelResponse
    {
        /// <example>TAURUS</example>
        public string Model { get; set; }
        /// <example>SEDAN</example>
        public string Type { get; set; }
    }
}
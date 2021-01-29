using System.Collections.Generic;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Models.Vehicle;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Result<decimal>> CalculateInsuranceExcessFromVehicleValue(string serviceCode, string rateGroupCode, decimal vehicleValue);
        Task<bool> CheckUseSecondaryRateCodeAsync(string rateGroupCode, string serviceType);
        Task<Result<VehicleDetailsDto>> GetVehicleDetailsAsync(GetRatesDto getRates, string rateGroupCode);
        Task<List<VehiclesByMakeDto>> GetVehicleTypeAsync(GetVehicleTypeDto model);
        Task<int> GetVehicleRateCode(VehicleDetailsDto vehicleType, string rateGroupCode, string serviceCode);
    }
}
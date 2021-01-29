using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using FACTS.GenericBooking.Common.Constants;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Domain.Models.IngresDto;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Models.Vehicle;
using FACTS.GenericBooking.Domain.Services.Interfaces;

namespace FACTS.GenericBooking.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ICachedIngresEntitiesService _cachedIngresEntitiesService;
        private readonly IMapper _mapper;

        public VehicleService(IMapper mapper, ICachedIngresEntitiesService cachedIngresEntitiesService)
        {
            _mapper = mapper;
            _cachedIngresEntitiesService = cachedIngresEntitiesService;
        }
        
        public async Task<Result<decimal>> CalculateInsuranceExcessFromVehicleValue(string serviceCode, string rateGroupCode, decimal vehicleValue)
        {
            decimal calculatedExcess = 0;
            if (rateGroupCode == RateGroupCode.Contract)
            {
                return new Result<decimal>(calculatedExcess);
            }

            IList<RateGroupTypeDto> rateGroupTypes = await _cachedIngresEntitiesService.GetRateGroupTypesCachedAsync();
            RateGroupTypeDto rateGroupType = rateGroupTypes.FirstOrDefault(x => x.ServiceCode == serviceCode && x.RateGroupCode == rateGroupCode);
            if (rateGroupType == null)
                return new Result<decimal>(ErrorMessages.RateGroupTypeNotFound);

            decimal excessInsurance = 0;
            decimal vehValueLimit = rateGroupType.VehicleValueLimit;
            decimal valueDiff = vehValueLimit - vehicleValue;
            decimal vehValueExcess = rateGroupType.VehicleValueExcess;
            while (valueDiff < 0 && vehValueExcess > 0)
            {
                excessInsurance += rateGroupType.InsuranceExcessAmount;
                valueDiff += vehValueExcess;
            }
            calculatedExcess = excessInsurance;
            return new Result<decimal>(calculatedExcess);
        }

        /// <summary>
        /// Check to see if the primary or secondary vehicle rate code should be used
        /// </summary>
        /// <param name="rateGroupCode"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public async Task<bool> CheckUseSecondaryRateCodeAsync(string rateGroupCode, string serviceType)
        {
            IList<RateGroupTypeDto> rates = await _cachedIngresEntitiesService.GetRateGroupTypesCachedAsync();
            RateGroupTypeDto secondaryRate = rates.FirstOrDefault(r => r.RateGroupCode == rateGroupCode && r.ServiceCode == serviceType);
            return secondaryRate == null;
        }

        public async Task<Result<VehicleDetailsDto>> GetVehicleDetailsAsync(GetRatesDto getRates, string rateGroupCode)
        {
            string make = getRates.VehicleMake;
            string model = getRates.VehicleModel;
            string type = getRates.VehicleType;
            string serviceCode = getRates.ServiceType;
            decimal vehicleValue = getRates.VehicleValue;
            IList<VehicleTypeDto> vehicles = await _cachedIngresEntitiesService.GetVehicleTypesCachedAsync();
            VehicleTypeDto vehicleType = vehicles.FirstOrDefault(v => v.Make == make && v.Model == model && v.Type == type);
            if (vehicleType == null)
                return new Result<VehicleDetailsDto>(ErrorMessages.VehicleTypeNotFound(make, model, type));

            bool useSecondaryRate = await CheckUseSecondaryRateCodeAsync(rateGroupCode, serviceCode);
            VehicleDetailsDto vehicleDetails = _mapper.Map<VehicleTypeDto, VehicleDetailsDto>(vehicleType);
            int vehicleRateCode = useSecondaryRate ? vehicleType.RateCodeSecondary : vehicleType.RateCode;
            vehicleType.RateCode = vehicleRateCode;
            vehicleDetails.VehicleValue = vehicleValue;
            return new Result<VehicleDetailsDto>(vehicleDetails);
        }

        public async Task<int> GetVehicleRateCode(VehicleDetailsDto vehicleDetails, string rateGroupCode, string serviceCode)
        {
            bool useSecondaryRate = await CheckUseSecondaryRateCodeAsync(rateGroupCode, serviceCode);
            int vehicleRateCode = useSecondaryRate ? vehicleDetails.RateCodeSecondary : vehicleDetails.RateCode;
            return vehicleRateCode;
        }

        public async Task<List<VehiclesByMakeDto>> GetVehicleTypeAsync(GetVehicleTypeDto model)
        {
            IList<VehicleTypeDto> vehicles = await _cachedIngresEntitiesService.GetVehicleTypesCachedAsync();
            IQueryable<VehicleTypeDto> query = vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(model.Make))
                query = query.Where(x => x.Make == model.Make);
            if (!string.IsNullOrEmpty(model.Model))
                query = query.Where(x => x.Model == model.Model);
            if (!string.IsNullOrEmpty(model.Type))
                query = query.Where(x => x.Type == model.Type);

            ILookup<string, VehicleTypeDto> vehiclesLookup = query.ToLookup(x => x.Make);
            List<VehiclesByMakeDto> vehicleMakes = new();
            foreach (IGrouping<string, VehicleTypeDto> v in vehiclesLookup)
            {
                VehiclesByMakeDto vehicleMake = new()
                {
                    Make   = v.Key,
                    Models = v.OrderBy(x=> x.Model).AsEnumerable().ToList()
                };
                vehicleMakes.Add(vehicleMake);
            }

            return vehicleMakes;
        }
    }
}
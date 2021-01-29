using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Constants;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Domain.Models.IngresDto;
using FACTS.GenericBooking.Domain.Services.Interfaces;

namespace FACTS.GenericBooking.Domain.Services
{
    public class LocationService : ILocationService
    {
        private readonly ICachedIngresEntitiesService _cachedIngresEntitiesService;

        public LocationService(ICachedIngresEntitiesService cachedIngresEntitiesService)
        {
            _cachedIngresEntitiesService = cachedIngresEntitiesService;
        }

        public async Task<DepotDto> GetDepotAsync(string depotAbbreviation)
        {
            IList<DepotDto> depots = await _cachedIngresEntitiesService.GetDepotsCachedAsync();
            return depots.FirstOrDefault(x => x.DepotAbbreviation == depotAbbreviation);
        }
        
        public async Task<string> GetMovementTypeAsync(string fromSuburb, string fromState, string toSuburb, string toState)
        {
            IList<SuburbDto> suburbs = await _cachedIngresEntitiesService.GetSuburbsCachedAsync();
            SuburbDto from = suburbs.First(x => x.Suburb == fromSuburb && x.State == fromState);
            SuburbDto to = suburbs.First(x => x.Suburb == toSuburb && x.State == toState);

            //If State and parent city is same then it's LOCAL
            if (from.State == to.State && from.ParAreaCode == to.ParAreaCode)
                return "METRO";
            //If State is same and parent city is different then it's RURAL
            if (from.State == to.State && from.ParAreaCode != to.ParAreaCode)
                return "RURAL";
            //If State and parent city is different then it's INTERSTATE
            return "INTERSTATE";
        }

        public async Task<Result<SuburbDto>> GetSuburbAsync(string pickOrDeliver, string suburb, string postcode, string state)
        {
            IList<SuburbDto> suburbs = await _cachedIngresEntitiesService.GetSuburbsCachedAsync();
            SuburbDto location = suburbs.FirstOrDefault(x => x.PostCode == postcode && x.State == state && x.Suburb == suburb);
            if (location == null)
            {
                return pickOrDeliver == AddressConsts.Deliver ?
                    new Result<SuburbDto>(ErrorMessages.DeliveryLocationNotFound(suburb, postcode, state)) :
                    new Result<SuburbDto>(ErrorMessages.PickLocationNotFound(suburb, postcode, state));
            }

            return new Result<SuburbDto>(location);
        }
        
    }
}
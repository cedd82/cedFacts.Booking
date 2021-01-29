using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.IngresDto;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface ILocationService
    {
        Task<DepotDto> GetDepotAsync(string depotAbbreviation);
        Task<string> GetMovementTypeAsync(string fromSuburb, string fromState, string toSuburb, string toState);
        Task<Result<SuburbDto>> GetSuburbAsync(string pickOrDeliver, string suburb, string postcode, string state);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

using FACTS.GenericBooking.Domain.Models.IngresDto;
using FACTS.GenericBooking.Domain.Models.Vehicle;
using FACTS.GenericBooking.Repository.Ingres.Entities;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface ICachedIngresEntitiesService
    {
        Task<IList<ApplicationRole>> GetApplicationRolesCachedAsync();
        Task<IList<DepotDto>> GetDepotsCachedAsync();
        Task<IList<RateGroupTypeDto>> GetRateGroupTypesCachedAsync();
        Task<IList<SuburbDto>> GetSuburbsCachedAsync();
        Task<IList<VehicleTypeDto>> GetVehicleTypesCachedAsync();
    }
}
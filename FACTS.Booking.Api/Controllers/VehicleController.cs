using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using FACTS.GenericBooking.Api.Models.Vehicle;
using FACTS.GenericBooking.Domain.Models.Vehicle;
using FACTS.GenericBooking.Domain.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FACTS.GenericBooking.Api.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [Route("vehicle")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiVersion( "1.0" )]
    public class VehicleController : ApiControllerBase
    {
        private readonly ICachedIngresEntitiesService _cachedIngresEntitiesService;
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(IMapper mapper,
                                 IVehicleService vehicleService,
                                 ICachedIngresEntitiesService cachedIngresEntitiesService)
        {
            _mapper = mapper;
            _vehicleService = vehicleService;
            _cachedIngresEntitiesService = cachedIngresEntitiesService;
        }

        // <summary>
        // get vehicle types
        // </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///		GET vehicle-types
        ///
        /// Sample response:
        ///
        ///	{
        ///		"data": [
        ///		{
        ///			"make": "FORD",
        ///			"models": [
        ///			{
        ///				"model": "TAURUS",
        ///				"type": "SEDAN"
        ///			},
        ///			{
        ///				"model": "LINCOLN",
        ///				"type": "SEDAN"
        ///			},
        ///			{
        ///				"model": "COUGAR",
        ///				"type": "SEDAN"
        ///			},
        ///			{
        ///				"model": "FOCUS TITANIUM",
        ///				"type": "SEDAN"
        ///			}],
        ///			"status": {
        ///				"code": 200,
        ///				"name": "OK",
        ///				"timestamp": "2020-12-30T14:35:20"
        ///			}
        ///		]
        ///	}
        ///	
        ///	</remarks>
        /// <response code="200">success</response>
        [HttpGet("vehicle-types")]
        [ProducesResponseType(typeof(IEnumerable<GetVehiclesByMakeResponse>), 200)]
        public async Task<IActionResult> GetVehicleTypesAsync([FromQuery] GetVehicleTypesRequest request)
        {
            GetVehicleTypeDto model = _mapper.Map<GetVehicleTypesRequest, GetVehicleTypeDto>(request);
            List<VehiclesByMakeDto> result = await _vehicleService.GetVehicleTypeAsync(model);
            List<GetVehiclesByMakeResponse> response = _mapper.Map<List<VehiclesByMakeDto>, List<GetVehiclesByMakeResponse>>(result);
            return ApiOk(response);
        }

        /// <remarks>
        /// 
        ///	Sample response:
        ///
        ///	{
        ///		"data": [
        ///		    "FORD",
        ///		    "TOYOTA",
        ///		    "MAZDA"
        ///		],
        ///		"status": {
        ///		    "code": 200,
        ///		    "name": "OK",
        ///		    "timestamp": "2020-12-30T14:35:20"
        ///		}
        /// }
        /// 
        /// </remarks>
        /// <response code="200">success</response>
        [HttpGet("makes")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<IActionResult> GetVehicleMakesAsync()
        {
            IList<VehicleTypeDto> result = await _cachedIngresEntitiesService.GetVehicleTypesCachedAsync();
            IEnumerable<string> response = result.Select(x => x.Make).Distinct().OrderBy(x => x);
            return ApiOk(response);
        }
    }
}

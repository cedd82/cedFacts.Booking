using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using FACTS.GenericBooking.Api.Models.Quote;
using FACTS.GenericBooking.Common.Models;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FACTS.GenericBooking.Api.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [Route("rates")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiVersion("1.0")]
    public class QuoteRatesController : ApiControllerBase
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly IMapper _mapper;
        private readonly IQuoteRateService _quoteRateService;
        private readonly IQuoteService _quoteService;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public QuoteRatesController(ClaimsPrincipal claimsPrincipal, 
                               IMapper mapper, 
                               IQuoteRateService quoteRateService,
                               IQuoteService quoteService)
        {
            _claimsPrincipal   = claimsPrincipal;
            _mapper            = mapper;
            _quoteRateService  = quoteRateService;
            _quoteService = quoteService;
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /rates/get-rates
        ///     {
        ///         "accountNumber": "PP9900",
        ///         "serviceType": "standard",
        ///         "pickupType": "customer",
        ///         "pickupSuburb": "MINTO",
        ///         "pickupPostcode": "2566",
        ///         "pickupState": "NSW",
        ///         "deliveryType": "",
        ///         "deliverySuburb": "CAIRNS",
        ///         "deliveryPostcode": "4870",
        ///         "deliveryState": "QLD",
        ///         "isDriveable": true,
        ///         "isModified": false,
        ///         "isDamaged": false,
        ///         "vehicleMake": "FORD",
        ///         "vehicleModel": "FALCON",
        ///         "vehicleType": "SEDAN",
        ///         "vehicleValue": 1233
        ///     }
        /// 
        ///
        ///	Sample Response:
        ///
        ///		POST /rates/get-rates
        ///		{
        ///		"data": {
        ///			"accountNumber": "PP9900",
        ///			"pickupAddress": {
        ///				"suburb": "NORTH SYDNEY",
        ///				"state": "NSW",
        ///				"postCode": "2060"
        ///			},
        ///			"deliveryAddress": {
        ///				"suburb": "CAIRNS",
        ///				"state": "QLD",
        ///				"postCode": "4870"
        ///			},
        ///			"vehicle": {
        ///				"vehicleValue": 12000,
        ///				"make": "FORD",
        ///				"model": "FALCON",
        ///				"vehicleType": "SEDAN",
        ///				"isDriveable": true,
        ///				"rates": [
        ///					{
        ///						"serviceType": "STANDARD",
        ///						"pickupType": "DEPOT",
        ///						"deliveryType": "CUSTOMER",
        ///						"transitDays": 14,
        ///						"totalRateIncludingGst": 1246.63
        ///					}
        ///				]
        ///			}
        ///		},
        ///		"status": {
        ///			"code": 200,
        ///			"name": "OK",
        ///			"timestamp": "2021-01-12T09:59:28"
        ///		}
        ///	}
        ///
        /// 
        /// </remarks>
        /// <returns>one or more rates based on deliveryType, pickupType and serviceCode</returns>
        /// <param name="request">rates object</param>
        /// <response code="200">Rates retrieved</response>
        /// <response code="400">Request has missing/invalid values</response>
        [HttpPost("get-rates")]
        [ProducesResponseType(typeof(GetRatesResponse), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        public async Task<IActionResult> GetRatesAsync([FromBody] GetRatesRequest request)
        {
            GetRatesDto model = _mapper.Map<GetRatesRequest, GetRatesDto>(request);
            model.Username = _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result<GetRatesResultDto> result = await _quoteRateService.GetRatesAsync(model);
            if (!result.Success())
                return ApiBadRequest(result.ApiMessage);
            GetRatesResponse response = _mapper.Map<GetRatesResultDto, GetRatesResponse>(result.Value);
            return ApiOk(response);

        }

        [HttpPost("create-quote")]
        [ProducesResponseType(typeof(CreateQuoteResponse), 200)]
        public async Task<IActionResult> CreateQuoteAsync([FromBody] CreateQuoteRequest request)
        {
            CreateQuoteDto model = _mapper.Map<CreateQuoteRequest, CreateQuoteDto>(request);
            model.Username = _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result<CreateQuoteResultDto> result = await _quoteService.CreateQuoteAsync(model);
            if (!result.Success())
                return ApiBadRequest(result.ApiMessage);
            CreateQuoteResponse response = _mapper.Map<CreateQuoteResultDto, CreateQuoteResponse>(result.Value);
            return ApiOk(response);

        }
    }
}

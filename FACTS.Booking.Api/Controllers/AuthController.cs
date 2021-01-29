using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using FACTS.GenericBooking.Api.Models.Auth;
using FACTS.GenericBooking.Common.Models.Api;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.Auth;
using FACTS.GenericBooking.Domain.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Wangkanai.Detection.Services;

namespace FACTS.GenericBooking.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiVersion( "1.0" )]
#if !DEBUG
    [Authorize]
#endif
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly IDetectionService _detectionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public AuthController(IAuthenticationService authenticationService,
                              ClaimsPrincipal claimsPrincipal,
                              IDetectionService detectionService,
                              IHttpContextAccessor httpContextAccessor,
                              IMapper mapper)
        {
            _authenticationService = authenticationService;
            _claimsPrincipal       = claimsPrincipal;
            _detectionService      = detectionService;
            _httpContextAccessor   = httpContextAccessor;
            _mapper                = mapper;
        }

        // <summary>
        // login user return bearer token
        // </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///         "username": "username",
        ///         "password": "YourPassword1!",
        ///     }
        ///
        /// Sample response:
        ///
        ///	{
        ///	    "data": {
        ///			"username": "USERNAME",
        ///			"accessToken": "eyJhbGc.Ppmh4c4ELUpzDJyde8.SXbFSjEqre.l1p1UsR....."
        ///		},
        ///		"status": {
        ///			"code": 200,
        ///			"name": "OK",
        ///			"timestamp": "2020-12-30T14:50:27"
        ///		}
        ///	}
        /// 
        /// </remarks>
        /// <response code="200">Auth bearer token</response>
        /// <response code="400">Request has missing/invalid values/failed authentication</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginDto), 200)]
        public async Task<IActionResult> AuthenticateUserAsync([FromBody] AuthenticateUserRequest request)
        {
            AuthenticateUserDto model = _mapper.Map<AuthenticateUserRequest, AuthenticateUserDto>(request);
            HttpRequestDeviceDetails deviceDetails = GetUserDeviceDetails();
            Result<UserLoginDto> result = await _authenticationService.AuthenticateUserAsync(model, deviceDetails);
            return result.Success() ? ApiOk(result.Value) : ApiBadRequest(result.ApiMessage);
        }

        // <summary>
        // update user password
        // </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///	POST /update-password
        ///	{
        ///		"OldPassword": "OldPassword",
        ///		"NewPassword": "Password1!"
        ///	}
        ///
        ///	Sample response:
        ///
        ///	{
        ///		"data": {}
        ///		"status": {
        ///			"code": 200,
        ///			"name": "OK",
        ///			"timestamp": "2020-12-30T14:50:27"
        ///		}
        ///	}
        /// 
        /// </remarks>
        /// <response code="200">success</response>
        /// <response code="400">Request has missing/invalid values/failed authentication</response>
        [Route("update-password")]
        [HttpPost]
        [ProducesResponseType(200)]
    #if !DEBUG
        [ApiExplorerSettings(IgnoreApi = true)]
    #endif
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            UpdatePasswordDto model = _mapper.Map<UpdatePasswordRequest, UpdatePasswordDto>(request);
            //model.Username = _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.Username = "TESTCED";
            Result result = await _authenticationService.UpdatePasswordAsync(model);
            return result.Success() ? ApiOk() : ApiBadRequest(result.ApiMessage);
        }

        // <summary>
        // create user
        // </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///	POST /create-user
        ///	{
        ///		"emailAddress": "user@example.com",
        ///		"password": "Password1!",
        ///		"username": "newUser",
        ///		"firstName": "john",
        ///		"lastName": "smith",
        ///		"mobileNumber": "0418123123"
        ///	}
        ///
        ///	Sample response:
        ///
        ///	{
        ///		"data": {}
        ///		"status": {
        ///			"code": 200,
        ///			"name": "OK",
        ///			"timestamp": "2020-12-30T14:50:27"
        ///		}
        ///	}
        /// 
        /// </remarks>
        /// <response code="200">success</response>
        /// <response code="400">Request has missing/invalid values/failed to create login</response>
        [Route("create-user")]
        [HttpPost]
        [ProducesResponseType(200)]
    #if !DEBUG
        [ApiExplorerSettings(IgnoreApi = true)]
    #endif
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
        {
            CreateUserDto model = _mapper.Map<CreateUserRequest, CreateUserDto>(request);
            //model.UserCodeCreateBy = _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result result = await _authenticationService.CreateUserAsync(model);
            return result.Success() ? ApiOk() : ApiBadRequest(result.ApiMessage);
        }

        private HttpRequestDeviceDetails GetUserDeviceDetails()
        {
            HttpRequestDeviceDetails deviceDetails = new HttpRequestDeviceDetails
            {
                DeviceType = _detectionService.Device.Type.ToString(),
                Browser    = _detectionService.Browser.Name.ToString(),
                PlatformOs = _detectionService.Platform.Name.ToString(),
                UserAgent  = _detectionService.UserAgent.ToString(),
                IpAddress  = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? ""
            };
            return deviceDetails;
        }
    }
}
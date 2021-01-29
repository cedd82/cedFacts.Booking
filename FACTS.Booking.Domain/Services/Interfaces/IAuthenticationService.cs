using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Models.Api;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.Auth;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<UserLoginDto>> AuthenticateUserAsync(AuthenticateUserDto authenticateUser, HttpRequestDeviceDetails deviceDetails);
        Task<Result> UpdatePasswordAsync(UpdatePasswordDto model);
        Task<Result> CreateUserAsync(CreateUserDto model);
    }
}

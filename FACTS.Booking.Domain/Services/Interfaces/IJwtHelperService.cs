using FACTS.GenericBooking.Domain.Models.Auth;
using FACTS.GenericBooking.Domain.Models.IngresDto;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface IJwtHelperService
    {
        string BuildEncryptedJwtTokenAsJwe(UserLoginDetailsDto userLogin);
        string BuildJwtTokenAsJws(UserLoginDetailsDto userLogin);
        JweValidationResult ValidateJweTokenGetPrincipal(string tokenString);
    }
}
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

namespace FACTS.GenericBooking.Domain.Models.Auth
{
    public class JweValidationResult
    {
        public ClaimsPrincipal ValidatedClaimsPrincipal { get; set; }
        public SecurityToken ValidatedSecurityToken { get; set; }
    }
}

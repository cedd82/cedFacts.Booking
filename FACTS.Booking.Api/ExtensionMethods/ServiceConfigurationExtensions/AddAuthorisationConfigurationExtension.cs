using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using FACTS.GenericBooking.Common.Constants;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FACTS.GenericBooking.Api.ExtensionMethods.ServiceConfigurationExtensions
{
    public static class AddAuthorisationConfigurationExtension
    {
        public static void AddCustomAuthorisationPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ExternalUser", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        bool? isAuthenticated = context.User?.Identity?.IsAuthenticated;
                        if (isAuthenticated == false)
                            return false;
                        IEnumerable<Claim> claims = context.User.Claims;
                        string roles = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                        if (string.IsNullOrEmpty(roles))
                            return false;
                        List<string> items = roles.Split(',').ToList();

                        bool authorised = items.Contains(AuthRoles.ExternalUser);
                        return authorised;

                    });
                });
            });
        }
    }
}

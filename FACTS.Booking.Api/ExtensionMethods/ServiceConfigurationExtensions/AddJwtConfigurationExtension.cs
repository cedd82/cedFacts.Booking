using System;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FACTS.GenericBooking.Api.ExtensionMethods.ServiceConfigurationExtensions
{
    public static class AddJwtConfigurationExtension
    {
        public static void AddJwtConfiguration(this IServiceCollection services,
                                               IConfiguration configuration,
                                               AppSecrets appSecrets)
        {
            JwtSettings jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.AddSingleton(jwtSettings);
            string jwtSymmetricKey = appSecrets.JwtSymmetricKey;
            string encryptKeyString = jwtSettings.JwtPublicKey;
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSymmetricKey));
            SymmetricSecurityKey encryptKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptKeyString));

            services
                .AddAuthentication(x =>
                {
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    //x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        // Invoked if exceptions are thrown
                        //OnAuthenticationFailed = authenticationFailedContext =>
                        // invoked before challenge sent to consumer
                        //OnChallenge = jwtBearerChallengeContext =>
                        // if auth fails and forbidden response
                        //OnForbidden = forbiddenContext =>
                        // when first message is received
                        //OnMessageReceived = messageReceivedContext =>
                        OnTokenValidated = context =>
                        {
                            //do the below to get the token if manual validation is required
                            //ValidateJweTokenGetPrincipal
                            //var authorization = HttpContext.Request.Headers["Authorization"].SingleOrDefault();
                            //var token = authorization.Substring(authorization.IndexOf(' ') + 1);
                            //var jwt = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                            IIdentity identity = context?.Principal?.Identity;
                            if (identity == null || !identity.IsAuthenticated || string.IsNullOrEmpty(context.Principal?.Identity?.Name))
                            {
                                context?.Fail("Unauthorized");
                            }

                            return Task.CompletedTask;
                        }
                    };

                    x.RequireHttpsMetadata = false;
                    x.SaveToken            = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer              = jwtSettings.Issuer,
                        ValidAudience            = jwtSettings.Audience,
                        IssuerSigningKey         = signingKey,
                        TokenDecryptionKey       = encryptKey,
                        ClockSkew                = TimeSpan.FromMinutes(2),
                    };

                    // if doing JWS no TokenDecryptionKey taking the public key is needed
                    //x.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateIssuer = false,
                    //    ValidateAudience = false,
                    //    ValidateLifetime = true,
                    //    ValidateIssuerSigningKey = true,
                    //    ValidIssuer = jwtSettings.Issuer,
                    //    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),

                    //    //TokenDecryptionKey = new SymmetricSecurityKey(key),
                    //    //ClockSkew = TimeSpan.FromMinutes(5)
                    //};
                });
        }
    }
}
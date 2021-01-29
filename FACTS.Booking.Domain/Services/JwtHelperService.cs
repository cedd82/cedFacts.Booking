using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Domain.Models.Auth;
using FACTS.GenericBooking.Domain.Services.Interfaces;

using Microsoft.IdentityModel.Tokens;

namespace FACTS.GenericBooking.Domain.Services
{
    public class JwtHelperService : IJwtHelperService
    {
        private readonly AppSecrets _appSecrets;
        private readonly JwtSettings _jwtSettings;

        public JwtHelperService(AppSecrets appSecrets, JwtSettings jwtSettings)
        {
            _appSecrets  = appSecrets;
            _jwtSettings = jwtSettings;
        }

        public string BuildEncryptedJwtTokenAsJwe(UserLoginDetailsDto userLogin)
        {
            string signingKeyString = _appSecrets.JwtSymmetricKey; 
            string encryptKeyString = _jwtSettings.JwtPublicKey;

            SymmetricSecurityKey signingKey = new(Encoding.Default.GetBytes(signingKeyString));
            SymmetricSecurityKey encryptKey = new(Encoding.Default.GetBytes(encryptKeyString));

            SigningCredentials signingCredentials = new(signingKey, SecurityAlgorithms.HmacSha256);

            EncryptingCredentials encryptingCredentials = new(encryptKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            
            SecurityTokenDescriptor descriptor = new()
            {
                Audience              = _jwtSettings.Audience,
                SigningCredentials    = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                IssuedAt              = DateTime.Now,
                Claims = new Dictionary<string, object>
                {
                    { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() }
                },
                Expires = DateTime.Now.AddHours(2),
                Issuer  = _jwtSettings.Issuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userLogin.OperatorName.ToUpper()),
                    new Claim(ClaimTypes.NameIdentifier, userLogin.OperatorId.ToUpper()),
                    //new Claim(Codes.Claims.UserTypeCode, userLogin.UserTypeCode)
                }),
                NotBefore = DateTime.Now,
                //AdditionalHeaderClaims = 
                //CompressionAlgorithm = 
            };

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwtSecurityToken = handler.CreateJwtSecurityToken(descriptor);
            
            // If someone tries to view the JWT without validating/decrypting the token,
            // then no claims are retrieved and the token is safe guarded.
            string tokenString = handler.WriteToken(jwtSecurityToken);

            return tokenString;
        }

        public string BuildJwtTokenAsJws(UserLoginDetailsDto userLogin)
        {
            byte[] key = Encoding.ASCII.GetBytes(_appSecrets.JwtSymmetricKey);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userLogin.OperatorName.ToUpper()),
                    new Claim(ClaimTypes.NameIdentifier, userLogin.OperatorId),
                }),
                Expires            = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }

        public JweValidationResult ValidateJweTokenGetPrincipal(string tokenString)
        {
            string signingKeyString = _appSecrets.JwtSymmetricKey; 
            string encryptKeyString = _jwtSettings.JwtPublicKey;
            SymmetricSecurityKey signingKey = new(Encoding.Default.GetBytes(signingKeyString));
            SymmetricSecurityKey encryptKey = new(Encoding.Default.GetBytes(encryptKeyString));

            // Verification
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidAudiences = new[]
                {
                    _jwtSettings.Audience
                },
                ValidIssuers = new[]
                {
                    _jwtSettings.Issuer
                },
                IssuerSigningKey = signingKey,
                // This is the decryption key
                TokenDecryptionKey = encryptKey,
            };

            JwtSecurityTokenHandler handler = new();
            ClaimsPrincipal validatedPrincipal = handler.ValidateToken(tokenString, tokenValidationParameters, out SecurityToken validatedSecurityToken);
            return new JweValidationResult
            {
                ValidatedClaimsPrincipal = validatedPrincipal,
                ValidatedSecurityToken   = validatedSecurityToken,
            };
        }
    }
}
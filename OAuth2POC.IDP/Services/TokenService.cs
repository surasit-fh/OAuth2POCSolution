using Microsoft.IdentityModel.Tokens;
using OAuth2POC.IDP.Services.IService;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Services
{
    public class TokenService : ITokenService
    {
        private static string _secret;

        public TokenService(string secret)
        {
            _secret = secret;
        }

        public string GenerateToken(JWTInfo jWTInfo)
        {
            try
            {
                byte[] secretBytes = Convert.FromBase64String(Base64Encode(_secret));
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secretBytes);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer = !string.IsNullOrEmpty(jWTInfo.Payload.Issuer) ? jWTInfo.Payload.Issuer : null,
                    Audience = !string.IsNullOrEmpty(jWTInfo.Payload.Audience.ToString()) ? jWTInfo.Payload.Audience.ToString() : null,
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, jWTInfo.Payload.Subject)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(jWTInfo.Payload.ExpirationTime),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
                string response = handler.WriteToken(securityToken);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Base64Encode(_secret));
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.ASCII.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.ASCII.GetString(base64EncodedBytes);
        }
    }
}
using Microsoft.IdentityModel.Tokens;
using OAuth2POC.API.Helpers;
using OAuth2POC.API.Services.IServices;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.API.Services
{
    public class TokenService : ITokenService
    {
        public bool ValidateToken(string token)
        {
            try
            {
                ClaimsPrincipal principal = GetPrincipal(token);

                if (principal == null)
                    return false;

                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;
                string audience = identity.Claims.First(x => x.Type == "aud").Value;
                UserInfo user = new UserRepository().GetById<UserInfo>(audience);

                if (user != null)
                    return true;
                else
                    return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            ClaimsPrincipal principal = null;

            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                byte[] secretKey = Convert.FromBase64String(Base64Encode(SettingHelper.ConfigMapping.Secret));

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };

                principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.ASCII.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
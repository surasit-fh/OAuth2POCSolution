using Microsoft.IdentityModel.Tokens;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.IDP.Helpers;
using OAuth2POC.IDP.Services.IService;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserService _userService;

        public TokenService(IUserService userService)
        {
            _userService = userService;
        }

        public string GetToken(JWTInfo jwtInfo)
        {
            try
            {
                return GenerateToken(jwtInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RefreshToken(string token, string refreshToken)
        {
            try
            {
                ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token);
                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;
                string audience = identity.Claims.First(x => x.Type == "aud").Value;
                UserInfo user = _userService.GetUser(audience);

                if (user != null)
                {
                    
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string RevokeToken()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #region Get Token

        private string GenerateToken(JWTInfo jwtInfo)
        {
            try
            {
                byte[] secretBytes = Convert.FromBase64String(Base64Encode(SettingHelper.ConfigMapping.Secret));
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secretBytes);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer = !string.IsNullOrEmpty(jwtInfo.Payload.Issuer) ? jwtInfo.Payload.Issuer : null,
                    Audience = !string.IsNullOrEmpty(jwtInfo.Payload.Audience.ToString()) ? jwtInfo.Payload.Audience.ToString() : null,
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, jwtInfo.Payload.Subject)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(jwtInfo.Payload.ExpirationTime),
                    NotBefore = DateTime.UtcNow,
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

        #endregion Get Token

        #region Refresh Token

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                byte[] secretBytes = Convert.FromBase64String(Base64Encode(SettingHelper.ConfigMapping.Secret));
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ValidateLifetime = false
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];

            using (RandomNumberGenerator numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        #endregion Refresh Token

        #region Validate Token

        private ClaimsPrincipal GetPrincipal(string token)
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

        #endregion Validate Token

        #region Other Function

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

        #endregion Other Function
    }
}
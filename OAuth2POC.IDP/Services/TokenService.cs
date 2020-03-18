using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.IDP.Helpers;
using OAuth2POC.IDP.Services.IService;
using OAuth2POC.Model.Enums;
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
        public TokenInfo GetToken(string clientId)
        {
            try
            {
                TokenInfo tokenInfo = new TokenInfo()
                {
                    AccessToken = GenerateToken(clientId),
                    TokenType = TokenType.Bearer,
                    ExpiresIn = 60 * 60,
                    RefreshToken = GenerateRefreshToken(32),
                    ClientId = ObjectId.Parse(clientId),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    TokenStatus = TokenStatus.Active
                };

                List<TokenInfo> listToken = new TokenRepository().GetByCriteria<TokenInfo>(new TokenInfo() { ClientId = ObjectId.Parse(clientId) });
                string token = string.Empty;

                if (listToken.Count > 0)
                {
                    tokenInfo.TokenId = listToken.FirstOrDefault().TokenId;
                    new TokenRepository().Update<TokenInfo>(tokenInfo);
                    token = listToken.FirstOrDefault().TokenId.ToString();
                }
                else
                {
                    token = new TokenRepository().Insert<TokenInfo>(tokenInfo);
                }

                TokenInfo tokenResponse = new TokenRepository().GetById<TokenInfo>(token);
                return tokenResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TokenInfo RefreshToken(string token, string refreshToken)
        {
            try
            {
                ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token);
                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;
                string audience = identity.Claims.First(x => x.Type == "aud").Value;
                List<TokenInfo> listToken = new TokenRepository().GetByCriteria<TokenInfo>(new TokenInfo() { RefreshToken = refreshToken, ClientId = ObjectId.Parse(audience) });

                if (listToken.Count > 0)
                {
                    TokenInfo tokenInfo = new TokenInfo()
                    {
                        AccessToken = GenerateToken(listToken.FirstOrDefault().ClientId.ToString()),
                        TokenType = TokenType.Bearer,
                        ExpiresIn = 60 * 60,
                        RefreshToken = GenerateRefreshToken(32),
                        ClientId = listToken.FirstOrDefault().ClientId,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                        TokenStatus = TokenStatus.Active
                    };

                    tokenInfo.TokenId = listToken.FirstOrDefault().TokenId;
                    new TokenRepository().Update<TokenInfo>(tokenInfo);
                    TokenInfo tokenResponse = new TokenRepository().GetById<TokenInfo>(listToken.FirstOrDefault().TokenId.ToString());
                    return tokenResponse;
                }
                else
                {
                    return null;
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
        }

        public bool RevokeToken(string token)
        {
            try
            {
                ClaimsPrincipal principal = GetPrincipal(token);

                if (principal == null)
                    return false;

                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;
                string audience = identity.Claims.First(x => x.Type == "aud").Value;
                List<TokenInfo> listToken = new TokenRepository().GetByCriteria<TokenInfo>(new TokenInfo() { ClientId = ObjectId.Parse(audience) });

                if (listToken.Count > 0)
                {
                    listToken.FirstOrDefault().TokenStatus = TokenStatus.Terminate;
                    new TokenRepository().Update<TokenInfo>(listToken.FirstOrDefault());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Get Token

        private string GenerateToken(string clientId)
        {
            string response = string.Empty;

            try
            {
                UserInfo user = new UserRepository().GetById<UserInfo>(clientId);

                if (user != null)
                {
                    byte[] secretBytes = Convert.FromBase64String(Base64Encode(SettingHelper.ConfigMapping.Secret));
                    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secretBytes);
                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Issuer = "OAuth2POC",
                        Audience = user.UserId.ToString(),
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, Base64Encode(user.UserId.ToString())),//nameid
                            new Claim(ClaimTypes.Role, user.UserRole.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(60),
                        NotBefore = DateTime.UtcNow,
                        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                    };

                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
                    response = handler.WriteToken(securityToken);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
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

        private string GenerateRefreshToken(int numberlength)
        {
            try
            {
                int length = numberlength >= 10 ? numberlength : 10;
                byte[] randomNumber = new byte[length];

                using (RandomNumberGenerator numberGenerator = RandomNumberGenerator.Create())
                {
                    numberGenerator.GetBytes(randomNumber);
                }

                return Convert.ToBase64String(randomNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Refresh Token

        #region Validate Token

        private ClaimsPrincipal GetPrincipal(string token)
        {
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

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
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
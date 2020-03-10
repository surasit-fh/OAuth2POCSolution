using Microsoft.Extensions.Options;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.IDP.Helpers;
using OAuth2POC.IDP.Process.IProcess;
using OAuth2POC.IDP.Services;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Process
{
    public class AccountProcess : IAccountProcess
    {
        private readonly AppSettings _appSettings;

        public AccountProcess(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticationResponse LoginProcess(UserInfo userInfo, AuthenticationInfo authenticationInfo)
        {
            try
            {
                switch (authenticationInfo.GrantType)
                {
                    case GrantType.AuthorizationCode:
                        {
                            return AuthenticationByCode(authenticationInfo);
                        }
                    case GrantType.ClientCredentials:
                        {
                            return AuthenticationByCredentials(userInfo);
                        }
                    case GrantType.RefreshToken:
                        {
                            return AuthenticationByToken(authenticationInfo);
                        }
                    default: return MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AuthenticationResponse LogoutProcess(TokenInfo tokenInfo)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AuthenticationResponse AuthenticationByCode(AuthenticationInfo authenticationInfo)
        {
            try
            {
                JWTInfo jwtInfo = new JWTInfo()
                {
                    Header = new Header()
                    {
                        Type = "HS256",
                        Algorithm = "JWT"
                    },
                    Payload = new Payload()
                    {
                        JWTid = MongoDB.Bson.ObjectId.GenerateNewId(),
                        Issuer = "OAuth2POC",
                        Subject = "OAuth2Token",
                        Audience = authenticationInfo.ClientId,
                        ExpirationTime = 60
                    }
                };

                TokenService tokenService = new TokenService(_appSettings.Secret);
                string token = tokenService.GenerateToken(jwtInfo);

                if (!string.IsNullOrEmpty(token))
                {
                    TokenInfo tokenInfo = new TokenInfo()
                    {
                        AccessToken = token,
                        TokenType = TokenType.Bearer,
                        ExpiresIn = jwtInfo.Payload.ExpirationTime * 60,
                        RefreshToken = "",
                        ClientId = authenticationInfo.ClientId,
                        CreatedAt = DateTime.Now
                    };

                    return MappingSuccessResponse(null, tokenInfo);
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.InternalServerError, ErrorCode.InternalServerError.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AuthenticationResponse AuthenticationByCredentials(string base64credentials)
        {
            try
            {
                byte[] credentialsByte = Convert.FromBase64String(base64credentials);
                string strCredentials = Encoding.ASCII.GetString(credentialsByte);
                string[] arrCre = strCredentials.Split(':');

                List<UserInfo> listUser = new UserRepository().GetAll<UserInfo>();
                UserInfo user = listUser.Find(u => u.Username == arrCre[0] && u.Password == arrCre[1]);

                if (user != null)
                {
                    AuthenticationInfo authenticationInfo = new AuthenticationInfo()
                    {
                        ClientId = user.UserId,
                        ClientSecret = Convert.ToBase64String(Encoding.ASCII.GetBytes(_appSettings.Secret)),
                        GrantType = GrantType.AuthorizationCode,
                        Code = Convert.ToBase64String(Encoding.ASCII.GetBytes(user.UserId.ToString())),
                        State = "",
                        Scope = new string[] { "profile" },
                        RedirectURI = ""
                    };

                    return MappingSuccessResponse(authenticationInfo, null);
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AuthenticationResponse AuthenticationByCredentials(UserInfo userInfo)
        {
            try
            {
                List<UserInfo> listUser = new UserRepository().GetAll<UserInfo>();
                UserInfo user = listUser.Find(u => u.Username == userInfo.Username && u.Password == userInfo.Password);

                if (user != null)
                {
                    AuthenticationInfo authenticationInfo = new AuthenticationInfo()
                    {
                        ClientId = user.UserId,
                        ClientSecret = Convert.ToBase64String(Encoding.ASCII.GetBytes(_appSettings.Secret)),
                        GrantType = GrantType.AuthorizationCode,
                        Code = Convert.ToBase64String(Encoding.ASCII.GetBytes(user.UserId.ToString())),
                        State = "",
                        Scope = new string[] { "profile" },
                        RedirectURI = ""
                    };

                    return MappingSuccessResponse(authenticationInfo, null);
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AuthenticationResponse AuthenticationByToken(AuthenticationInfo authenticationInfo)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Mapping Response

        private AuthenticationResponse MappingSuccessResponse(AuthenticationInfo authenticationInfo, TokenInfo tokenInfo)
        {
            return new AuthenticationResponse() { AuthenticationInfo = authenticationInfo, TokenInfo = tokenInfo, IsSuccess = true, ErrorCode = (int)ErrorCode.Success, Description = ErrorCode.Success.ToString() };
        }

        private AuthenticationResponse MappingErrorResponse(ErrorCode errorCode, string description)
        {
            return new AuthenticationResponse() { AuthenticationInfo = null, TokenInfo = null, IsSuccess = false, ErrorCode = (int)errorCode, Description = description };
        }

        #endregion Mapping Response
    }
}
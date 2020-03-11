using Microsoft.Extensions.Options;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.IDP.Helpers;
using OAuth2POC.IDP.Process.IProcess;
using OAuth2POC.IDP.Services;
using OAuth2POC.IDP.Services.IService;
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
        private readonly ITokenService _tokenService;

        public AccountProcess(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public AuthenticationResponse LoginProcess(UserInfo userInfo, AuthenticationInfo authenticationInfo, TokenInfo tokenInfo)
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
                            return AuthenticationByToken(tokenInfo);
                        }
                    default: return MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AuthenticationResponse LogoutProcess(string AccessToken)
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

                string token = _tokenService.GetToken(jwtInfo);

                if (!string.IsNullOrEmpty(token))
                {
                    AuthenticationInfo authenResponse = new AuthenticationInfo()
                    {
                        GrantType = GrantType.AuthorizationCode
                    };

                    TokenInfo tokenResponse = new TokenInfo()
                    {
                        AccessToken = token,
                        TokenType = TokenType.Bearer,
                        ExpiresIn = jwtInfo.Payload.ExpirationTime * 60,
                        RefreshToken = _tokenService.GenerateRefreshToken(),
                        ClientId = authenticationInfo.ClientId.ToString(),
                        ExpiresAt = DateTime.UtcNow.AddSeconds(jwtInfo.Payload.ExpirationTime * 60)
                    };

                    List<TokenInfo> listToken = new TokenRepository().GetByCriteria<TokenInfo>(new TokenInfo() { ClientId = authenticationInfo.ClientId.ToString() });

                    if (listToken.Count > 0)
                    {
                        new TokenRepository().Update<TokenInfo>(listToken.FirstOrDefault());
                    }
                    else
                    {
                        new TokenRepository().Insert<TokenInfo>(tokenResponse);
                    }

                    return MappingSuccessResponse(authenResponse, tokenResponse);
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

        private AuthenticationResponse AuthenticationByCredentials(UserInfo userInfo)
        {
            try
            {
                List<UserInfo> listUser = new UserRepository().GetAll<UserInfo>();
                UserInfo user = listUser.Find(u => u.Username == userInfo.Username && u.Password == userInfo.Password);

                if (user != null)
                {
                    AuthenticationInfo authenResponse = new AuthenticationInfo()
                    {
                        ClientId = user.UserId,
                        ClientSecret = Convert.ToBase64String(Encoding.ASCII.GetBytes(SettingHelper.ConfigMapping.Secret)),
                        GrantType = GrantType.ClientCredentials,
                        Code = Convert.ToBase64String(Encoding.ASCII.GetBytes(user.UserId.ToString())),
                        State = string.Empty,
                        Scope = new string[] { "profile" },
                        RedirectURI = ""
                    };

                    return MappingSuccessResponse(authenResponse, null);
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

        private AuthenticationResponse AuthenticationByToken(TokenInfo tokenInfo)
        {
            try
            {
                var response = _tokenService.RefreshToken(tokenInfo.AccessToken, tokenInfo.RefreshToken);

                AuthenticationInfo authenResponse = new AuthenticationInfo()
                {
                    GrantType = GrantType.RefreshToken
                };

                TokenInfo tokenResponse = new TokenInfo()
                {

                };

                return MappingSuccessResponse(authenResponse, tokenResponse);
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
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.IDP.Helpers;
using OAuth2POC.IDP.Process.IProcess;
using OAuth2POC.IDP.Services.IService;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public AuthenticationResponse LoginProcess(string credentials, AuthenticationInfo authenticationInfo, TokenInfo tokenInfo)
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
                            return AuthenticationByCredentials(credentials);
                        }
                    case GrantType.RefreshToken:
                        {
                            return AuthenticationByToken(tokenInfo);
                        }
                    default: return MappingErrorResponse(ErrorCode.NotFound, "GrantType not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AuthenticationResponse LogoutProcess(string token)
        {
            try
            {
                bool isSuccess = _tokenService.RevokeToken(token);

                if (isSuccess)
                {
                    return MappingSuccessResponse(null, null);
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.Unauthorized, "Invalid token!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AuthenticationResponse AuthenticationByCredentials(string credentials)
        {
            try
            {
                string strCredentials = Encoding.ASCII.GetString(Convert.FromBase64String(credentials));
                string[] creArr = strCredentials.Split(':');
                List<UserInfo> listUser = new UserRepository().GetByCriteria<UserInfo>(new UserInfo() { Username = creArr[0], Password = creArr[1] });

                if (listUser.Count > 0)
                {
                    AuthenticationInfo authenResponse = new AuthenticationInfo();
                    authenResponse.ClientId = listUser.FirstOrDefault().UserId;
                    authenResponse.ClientSecret = Convert.ToBase64String(Encoding.ASCII.GetBytes(SettingHelper.ConfigMapping.Secret));
                    authenResponse.GrantType = GrantType.ClientCredentials;
                    authenResponse.Code = GetRandomNumber(40);
                    authenResponse.State = string.Empty;

                    if (listUser.FirstOrDefault().UserRole.Equals(UserRole.Admin))
                    {
                        authenResponse.Scope = new string[] 
                        {
                            Scope.Profile.ToString(),
                            Scope.Address.ToString(),
                            Scope.ServiceAPI.ToString()
                        };
                    }
                    else
                    {
                        authenResponse.Scope = new string[] 
                        {
                            Scope.Profile.ToString(),
                            Scope.ServiceAPI.ToString()
                        };
                    }

                    authenResponse.RedirectURI = string.Empty;

                    listUser.FirstOrDefault().Code = authenResponse.Code;
                    new UserRepository().Update<UserInfo>(listUser.FirstOrDefault());

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

        private AuthenticationResponse AuthenticationByCode(AuthenticationInfo authenticationInfo)
        {
            try
            {
                List<UserInfo> listUser = new UserRepository().GetByCriteria<UserInfo>(new UserInfo() { Code = authenticationInfo.Code });

                if (listUser.Count > 0)
                {
                    TokenInfo tokenResponse = _tokenService.GetToken(listUser.FirstOrDefault().UserId.ToString());

                    if (tokenResponse != null)
                    {
                        AuthenticationInfo authenResponse = new AuthenticationInfo()
                        {
                            GrantType = GrantType.AuthorizationCode
                        };

                        return MappingSuccessResponse(authenResponse, tokenResponse);
                    }
                    else
                    {
                        return MappingErrorResponse(ErrorCode.InternalServerError, ErrorCode.InternalServerError.ToString());
                    }
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.Unauthorized, "Invalid authorization code");
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
                TokenInfo tokenResponse = _tokenService.RefreshToken(tokenInfo.AccessToken, tokenInfo.RefreshToken);

                if (tokenResponse != null)
                {
                    AuthenticationInfo authenResponse = new AuthenticationInfo()
                    {
                        GrantType = GrantType.RefreshToken
                    };

                    return MappingSuccessResponse(authenResponse, tokenResponse);
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.Unauthorized, "Invalid token!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Other Function

        private string GetRandomNumber(int numberLength)
        {
            try
            {
                int length = numberLength >= 10 ? numberLength : 10;
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

        #endregion Other Function

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
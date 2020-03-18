using Newtonsoft.Json;
using OAuth2POC.Client.Adapters;
using OAuth2POC.Client.Process.IProcess;
using OAuth2POC.Client.Services;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Process
{
    public class UserProcess : IUserProcess
    {
        public UserControlResponse GetUsers(string credentials)
        {
            try
            {
                AuthenticationResponse getAuthen = GetAuthentication(credentials);

                if (getAuthen.IsSuccess)
                {
                    UserService userService = new UserService(30, 30, "application/x-www-form-urlencoded", "application/json");
                    UserControlResponse getUsers = userService.GetUsers(getAuthen.TokenInfo.AccessToken);

                    if (getUsers.IsSuccess)
                    {
                        return MappingSuccessResponse(getUsers.UserInfos);
                    }
                    else
                    {
                        return MappingErrorResponse((ErrorCode)getUsers.ErrorCode, getUsers.Description);
                    }
                }
                else
                {
                    return MappingErrorResponse((ErrorCode)getAuthen.ErrorCode, getAuthen.Description);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse GetUser(string credentials, UserInfo userInfo)
        {
            try
            {
                AuthenticationResponse getAuthen = GetAuthentication(credentials);

                if (getAuthen.IsSuccess)
                {
                    UserService userService = new UserService(30, 30, "application/json", "application/json");
                    UserControlResponse getUser = userService.GetUser(getAuthen.TokenInfo.AccessToken, userInfo);

                    if (getUser.IsSuccess)
                    {
                        return MappingSuccessResponse(getUser.UserInfos);
                    }
                    else
                    {
                        return MappingErrorResponse((ErrorCode)getUser.ErrorCode, getUser.Description);
                    }
                }
                else
                {
                    return MappingErrorResponse((ErrorCode)getAuthen.ErrorCode, getAuthen.Description);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse InsertUser(string credentials, UserInfo userInfo)
        {
            try
            {
                AuthenticationResponse getAuthen = GetAuthentication(credentials);

                if (getAuthen.IsSuccess)
                {
                    UserService userService = new UserService(30, 30, "application/json", "application/json");
                    UserControlResponse getUser = userService.InsertUser(getAuthen.TokenInfo.AccessToken, userInfo);

                    if (getUser.IsSuccess)
                    {
                        return MappingSuccessResponse(getUser.UserInfos);
                    }
                    else
                    {
                        return MappingErrorResponse((ErrorCode)getUser.ErrorCode, getUser.Description);
                    }
                }
                else
                {
                    return MappingErrorResponse((ErrorCode)getAuthen.ErrorCode, getAuthen.Description);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse UpdateUser(string credentials, UserInfo userInfo)
        {
            try
            {
                AuthenticationResponse getAuthen = GetAuthentication(credentials);

                if (getAuthen.IsSuccess)
                {
                    UserService userService = new UserService(30, 30, "application/json", "application/json");
                    UserControlResponse getUser = userService.UpdateUser(getAuthen.TokenInfo.AccessToken, userInfo);

                    if (getUser.IsSuccess)
                    {
                        return MappingSuccessResponse(getUser.UserInfos);
                    }
                    else
                    {
                        return MappingErrorResponse((ErrorCode)getUser.ErrorCode, getUser.Description);
                    }
                }
                else
                {
                    return MappingErrorResponse((ErrorCode)getAuthen.ErrorCode, getAuthen.Description);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse DeleteUser(string credentials, UserInfo userInfo)
        {
            try
            {
                AuthenticationResponse getAuthen = GetAuthentication(credentials);

                if (getAuthen.IsSuccess)
                {
                    UserService userService = new UserService(30, 30, "application/json", "application/json");
                    UserControlResponse getUser = userService.DeleteUser(getAuthen.TokenInfo.AccessToken, userInfo);

                    if (getUser.IsSuccess)
                    {
                        return MappingSuccessResponse(getUser.UserInfos);
                    }
                    else
                    {
                        return MappingErrorResponse((ErrorCode)getUser.ErrorCode, getUser.Description);
                    }
                }
                else
                {
                    return MappingErrorResponse((ErrorCode)getAuthen.ErrorCode, getAuthen.Description);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AuthenticationResponse GetAuthentication(string credentials)
        {
            try
            {
                string strCredentials = Encoding.ASCII.GetString(Convert.FromBase64String(credentials));
                string[] creArr = strCredentials.Split(':');

                AuthenticationInfo getAuthcodeRequest = new AuthenticationInfo()
                {
                    GrantType = GrantType.ClientCredentials
                };

                AuthenticationService authenService = new AuthenticationService(30, 30, "application/json", "application/json", creArr[0], creArr[1]);
                AuthenticationResponse getAuthorizationCode = authenService.GetAuthorization(getAuthcodeRequest, null);

                if (getAuthorizationCode.IsSuccess)
                {
                    AuthenticationInfo getTokenRequest = new AuthenticationInfo()
                    {
                        GrantType = GrantType.AuthorizationCode,
                        Code = getAuthorizationCode.AuthenticationInfo.Code
                    };

                    AuthenticationResponse getToken = authenService.GetAuthorization(getTokenRequest, null);
                    return new AuthenticationResponse()
                    {
                        AuthenticationInfo = getToken.AuthenticationInfo,
                        TokenInfo = getToken.TokenInfo,
                        IsSuccess = getToken.IsSuccess,
                        ErrorCode = getToken.ErrorCode,
                        Description = getToken.Description
                    };
                }
                else
                {
                    return new AuthenticationResponse()
                    {
                        AuthenticationInfo = getAuthorizationCode.AuthenticationInfo,
                        TokenInfo = getAuthorizationCode.TokenInfo,
                        IsSuccess = getAuthorizationCode.IsSuccess,
                        ErrorCode = getAuthorizationCode.ErrorCode,
                        Description = getAuthorizationCode.Description
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Mapping Response

        private UserControlResponse MappingSuccessResponse(List<UserInfo> userInfos)
        {
            return new UserControlResponse() { UserInfos = userInfos, IsSuccess = true, ErrorCode = (int)ErrorCode.Success, Description = ErrorCode.Success.ToString() };
        }

        private UserControlResponse MappingErrorResponse(ErrorCode errorCode, string description)
        {
            return new UserControlResponse() { UserInfos = null, IsSuccess = false, ErrorCode = (int)errorCode, Description = description };
        }

        #endregion Mapping Response
    }
}
using Microsoft.AspNetCore.Mvc;
using OAuth2POC.Client.Process.IProcess;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Requests;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Controllers
{
    [Route("api/auth/oauth2/v2/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserProcess _userProcess;

        public UsersController(IUserProcess userProcess)
        {
            _userProcess = userProcess;
        }

        [HttpPost("GetUsers")]
        [Consumes("application/json")]
        public UserControlResponse GetUsers([FromBody] UserRequest userRequest)
        {
            AuthenticationResponse getAuthCode = _userProcess.Authentication(userRequest.UserInfo, new AuthenticationInfo() { GrantType = GrantType.ClientCredentials }, null);

            if (getAuthCode.IsSuccess)
            {
                AuthenticationInfo authenRequest = new AuthenticationInfo()
                {
                    GrantType = GrantType.AuthorizationCode,
                    Code = getAuthCode.AuthenticationInfo.Code
                };

                AuthenticationResponse getToken = _userProcess.Authentication(userRequest.UserInfo, authenRequest, null);

                if (getToken.IsSuccess)
                {
                    UserControlResponse getUsers = _userProcess.GetUsers(getToken.TokenInfo.AccessToken);

                    if (getUsers.IsSuccess)
                    {
                        return getUsers;
                    }
                    else
                    {
                        return new UserControlResponse()
                        {
                            UserInfos = null,
                            IsSuccess = getAuthCode.IsSuccess,
                            ErrorCode = getAuthCode.ErrorCode,
                            Description = getAuthCode.Description
                        };
                    }
                }
                else
                {
                    return new UserControlResponse()
                    {
                        UserInfos = null,
                        IsSuccess = getAuthCode.IsSuccess,
                        ErrorCode = getAuthCode.ErrorCode,
                        Description = getAuthCode.Description
                    };
                }
            }
            else
            {
                return new UserControlResponse()
                {
                    UserInfos = null,
                    IsSuccess = getAuthCode.IsSuccess,
                    ErrorCode = getAuthCode.ErrorCode,
                    Description = getAuthCode.Description
                };
            }
        }

        [HttpPost("GetUser")]
        [Consumes("application/json")]
        public UserControlResponse GetUser([FromBody] UserRequest userRequest)
        {
            UserControlResponse getUsers = _userProcess.GetUsers("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik9BdXRoMlBPQ1Rva2VuIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNTg0MzQyNTEzLCJleHAiOjE1ODQzNDYxMTMsImlhdCI6MTU4NDM0MjUxMywiaXNzIjoiT0F1dGgyUE9DIiwiYXVkIjoiNWU2YTBlMjc3ZjFhYzRlN2NkNDM3YmFmIn0.ASrB_nbVYdCHYtDYw8zpu--o2WI-oOLRNuJPO4lqalE");

            if (getUsers.IsSuccess)
            {
                return getUsers;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
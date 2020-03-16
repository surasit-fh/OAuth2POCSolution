using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Process.IProcess
{
    public interface IUserProcess
    {
        AuthenticationResponse Authentication(UserInfo userInfo, AuthenticationInfo authenticationInfo, TokenInfo tokenInfo);
        UserControlResponse GetUsers(string token);
        UserControlResponse GetUser(string token, UserInfo userInfo);
    }
}
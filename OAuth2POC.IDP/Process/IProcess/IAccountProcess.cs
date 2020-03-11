using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Process.IProcess
{
    public interface IAccountProcess
    {
        AuthenticationResponse LoginProcess(UserInfo userInfo, AuthenticationInfo authenticationInfo, TokenInfo tokenInfo);
        AuthenticationResponse LogoutProcess(string AccessToken);
    }
}
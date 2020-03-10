using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.API.Process.IProcess
{
    public interface IServiceProcess
    {
        UserControlResponse GetUsers();
        UserControlResponse GetUser(UserInfo userInfo);
        UserControlResponse InsertUser(UserInfo userInfo);
        UserControlResponse UpdateUser(UserInfo userInfo);
        UserControlResponse DeleteUser(UserInfo userInfo);
    }
}
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
        UserControlResponse GetUsers(string credentials);
        UserControlResponse GetUser(string credentials, UserInfo userInfo);
        UserControlResponse InsertUser(string credentials, UserInfo userInfo);
        UserControlResponse UpdateUser(string credentials, UserInfo userInfo);
        UserControlResponse DeleteUser(string credentials, UserInfo userInfo);
    }
}
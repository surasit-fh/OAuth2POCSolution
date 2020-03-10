using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.API.Services.IServices
{
    public interface IUserService
    {
        List<UserInfo> GetUsers();
        UserInfo GetUser(string userId);
        bool InsertUser(UserInfo userInfo);
        bool UpdateUser(UserInfo userInfo);
        bool DeleteUser(string userId);
    }
}
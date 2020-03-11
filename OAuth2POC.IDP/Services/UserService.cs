using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.IDP.Services.IService;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Services
{
    public class UserService : IUserService
    {
        public List<UserInfo> GetUsers()
        {
            try
            {
                List<UserInfo> listUser = new UserRepository().GetAll<UserInfo>();
                return listUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserInfo GetUser(string userId)
        {
            try
            {
                UserInfo user = new UserRepository().GetById<UserInfo>(userId);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertUser(UserInfo userInfo)
        {
            try
            {
                bool isSuccess = new UserRepository().Insert<UserInfo>(userInfo);
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUser(UserInfo userInfo)
        {
            try
            {
                bool isSuccess = new UserRepository().Update<UserInfo>(userInfo);
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteUser(string userId)
        {
            try
            {
                bool isSuccess = new UserRepository().Delete<UserInfo>(userId);
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
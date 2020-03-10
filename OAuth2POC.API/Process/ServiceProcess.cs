using Microsoft.Extensions.Options;
using OAuth2POC.API.Helpers;
using OAuth2POC.API.Process.IProcess;
using OAuth2POC.API.Services;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.API.Process
{
    public class ServiceProcess : IServiceProcess
    {
        private readonly AppSettings _appSettings;

        public ServiceProcess(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public UserControlResponse GetUsers()
        {
            try
            {
                List<UserInfo> listUser = new UserService().GetUsers();

                if (listUser.Count > 0)
                {
                    return MappingSuccessResponse(listUser);
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.NotFound, ErrorCode.NotFound.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse GetUser(UserInfo userInfo)
        {
            try
            {
                UserInfo user = new UserService().GetUser(userInfo.UserId.ToString());

                if (user != null)
                {
                    return MappingSuccessResponse(new List<UserInfo>() { user });
                }
                else
                {
                    return MappingErrorResponse(ErrorCode.NotFound, ErrorCode.NotFound.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse InsertUser(UserInfo userInfo)
        {
            try
            {
                bool isSuccess = new UserService().InsertUser(userInfo);

                if (isSuccess)
                {
                    return MappingSuccessResponse(null);
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

        public UserControlResponse UpdateUser(UserInfo userInfo)
        {
            try
            {
                bool isSuccess = new UserService().UpdateUser(userInfo);

                if (isSuccess)
                {
                    return MappingSuccessResponse(null);
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

        public UserControlResponse DeleteUser(UserInfo userInfo)
        {
            try
            {
                bool isSuccess = new UserService().DeleteUser(userInfo.UserId.ToString());

                if (isSuccess)
                {
                    return MappingSuccessResponse(null);
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
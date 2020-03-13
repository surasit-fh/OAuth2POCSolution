using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.IDP.Services;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Helpers
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public SecurityHeadersAttribute()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var headers = filterContext.HttpContext.Request.Headers;

            if (!headers.ContainsKey("Authorization"))
            {
                filterContext.Result = new JsonResult(MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString()));
            }

            string authorizationHeader = headers["Authorization"];

            if (authorizationHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
            {
                string credentials = authorizationHeader.Substring("Basic".Length).Trim();
                string strCredentials = Encoding.ASCII.GetString(Convert.FromBase64String(credentials));
                string[] creArr = strCredentials.Split(':');
                List<UserInfo> listUser = new UserRepository().GetByCriteria<UserInfo>(new UserInfo() { Username = creArr[0], Password = creArr[1] });

                if (listUser.Count == 0)
                {
                    filterContext.Result = new JsonResult(MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString()));
                }
            }
            else
            {
                filterContext.Result = new JsonResult(MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString()));
            }

            base.OnActionExecuting(filterContext);
        }

        #region Mapping Response

        private BaseResponse MappingErrorResponse(ErrorCode errorCode, string description)
        {
            return new BaseResponse() { IsSuccess = false, ErrorCode = (int)errorCode, Description = description };
        }

        #endregion Mapping Response
    }
}
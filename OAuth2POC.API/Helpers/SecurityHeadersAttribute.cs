using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2POC.API.Services;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OAuth2POC.API.Helpers
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

            if (!authorizationHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new JsonResult(MappingErrorResponse(ErrorCode.Unauthorized, ErrorCode.Unauthorized.ToString()));
            }
                
            string token = authorizationHeader.Substring("Bearer".Length).Trim();

            if (!new TokenService().ValidateToken(token))
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
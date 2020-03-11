using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2POC.API.Services;
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
                filterContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
            }

            string authorizationHeader = headers["Authorization"];

            if (!authorizationHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
            }
                
            string token = authorizationHeader.Substring("Bearer".Length).Trim();

            if (!new TokenService().ValidateToken(token))
            {
                filterContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
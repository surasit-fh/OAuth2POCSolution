using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                filterContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
            }

            string authorizationHeader = headers["Authorization"];


        }
    }
}
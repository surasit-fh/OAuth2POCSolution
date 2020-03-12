using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2POC.DAL.Repositories.Repositories;
using OAuth2POC.Model.Models.Interface;
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
                filterContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
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
                    filterContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
                }
            }
            else if (authorizationHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                string token = authorizationHeader.Substring("Bearer".Length).Trim();
            }
            else
            {
                filterContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
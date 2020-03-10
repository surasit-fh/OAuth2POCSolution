using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuth2POC.API.Services.IServices
{
    public interface ITokenService
    {
        string ValidateToken(string token);
        ClaimsPrincipal GetPrincipal(string token);
    }
}
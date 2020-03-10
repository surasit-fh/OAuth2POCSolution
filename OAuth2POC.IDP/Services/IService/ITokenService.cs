using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Services.IService
{
    public interface ITokenService
    {
        string GenerateToken(JWTInfo jWTInfo);
        string ValidateToken(string token);
        ClaimsPrincipal GetPrincipal(string token);
    }
}
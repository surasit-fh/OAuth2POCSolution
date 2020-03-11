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
        string GetToken(JWTInfo jwtInfo);
        string RefreshToken(string token, string refreshToken);
        bool ValidateToken(string token);
        //string RevokeToken();
        string GenerateRefreshToken();
    }
}
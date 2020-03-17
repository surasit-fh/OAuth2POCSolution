using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2POC.IDP.Helpers;
using OAuth2POC.IDP.Process.IProcess;
using OAuth2POC.Model.Headers;
using OAuth2POC.Model.Models.Requests;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Controllers
{
    [SecurityHeaders]
    [ApiController]
    [Route("api/auth/oauth2/v2/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IAccountProcess _accountProcess;

        public AccountsController(IAccountProcess accountProcess)
        {
            _accountProcess = accountProcess;
        }

        [HttpPost("Login")]
        [Consumes("application/json")]
        public AuthenticationResponse Login([FromHeader]BaseHeader header, AuthenRequest authenRequest)
        {
            AuthenticationResponse authenResponse = _accountProcess.LoginProcess(header.Authorization.Substring("Basic".Length).Trim(), authenRequest.AuthenticationInfo, authenRequest.TokenInfo);
            return authenResponse;
        }

        [HttpGet("Logout")]
        [Consumes("application/x-www-form-urlencoded")]
        public AuthenticationResponse Logout([FromHeader]BaseHeader header)
        {
            AuthenticationResponse authenResponse = _accountProcess.LogoutProcess(header.Authorization.Substring("Bearer".Length).Trim());
            return authenResponse;
        }
    }
}
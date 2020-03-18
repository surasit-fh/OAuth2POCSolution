using Microsoft.AspNetCore.Mvc;
using OAuth2POC.Client.Helpers;
using OAuth2POC.Client.Process.IProcess;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Headers;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Requests;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Controllers
{
    [SecurityHeaders]
    [ApiController]
    [Route("api/auth/oauth2/v2/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserProcess _userProcess;

        public UsersController(IUserProcess userProcess)
        {
            _userProcess = userProcess;
        }

        [HttpPost("GetUsers")]
        [Consumes("application/json")]
        public UserControlResponse GetUsers([FromHeader]BaseHeader header)
        {
            UserControlResponse userControl = _userProcess.GetUsers(header.Authorization.Substring("Basic".Length).Trim());
            return userControl;
        }

        [HttpPost("GetUser")]
        [Consumes("application/json")]
        public UserControlResponse GetUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse userControl = _userProcess.GetUser(header.Authorization.Substring("Basic".Length).Trim(), userRequest.UserInfo);
            return userControl;
        }

        [HttpPost("InsertUser")]
        [Consumes("application/json")]
        public UserControlResponse InsertUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse userControl = _userProcess.InsertUser(header.Authorization.Substring("Basic".Length).Trim(), userRequest.UserInfo);
            return userControl;
        }

        [HttpPost("UpdateUser")]
        [Consumes("application/json")]
        public UserControlResponse UpdateUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse userControl = _userProcess.UpdateUser(header.Authorization.Substring("Basic".Length).Trim(), userRequest.UserInfo);
            return userControl;
        }

        [HttpPost("DeleteUser")]
        [Consumes("application/json")]
        public UserControlResponse DeleteUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse userControl = _userProcess.DeleteUser(header.Authorization.Substring("Basic".Length).Trim(), userRequest.UserInfo);
            return userControl;
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2POC.API.Helpers;
using OAuth2POC.API.Process.IProcess;
using OAuth2POC.Model.Headers;
using OAuth2POC.Model.Models.Requests;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.API.Controllers
{
    [SecurityHeaders]
    [Authorize(Policy = "Bearer")]
    [ApiController]
    [Route("api/auth/oauth2/v2/[controller]")]
    public class ServicesController
    {
        private readonly IServiceProcess _serviceProcess;

        public ServicesController(IServiceProcess serviceProces)
        {
            _serviceProcess = serviceProces;
        }

        [HttpGet("GetUsers")]
        [Consumes("application/x-www-form-urlencoded")]
        public UserControlResponse GetAllUsers([FromHeader]BaseHeader header)
        {
            UserControlResponse controlResponse = _serviceProcess.GetUsers();
            return controlResponse;
        }

        [HttpPost("GetUsers")]
        [Consumes("application/json")]
        public UserControlResponse GetUsers([FromHeader]BaseHeader header)
        {
            UserControlResponse controlResponse = _serviceProcess.GetUsers();
            return controlResponse;
        }

        [HttpPost("GetUser")]
        [Consumes("application/json")]
        public UserControlResponse GetUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.GetUser(userRequest.UserInfo);
            return controlResponse;
        }

        [HttpPost("InsertUser")]
        [Consumes("application/json")]
        public UserControlResponse InsertUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.InsertUser(userRequest.UserInfo);
            return controlResponse;
        }

        [HttpPost("UpdateUser")]
        [Consumes("application/json")]
        public UserControlResponse UpdateUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.UpdateUser(userRequest.UserInfo);
            return controlResponse;
        }

        [AllowAnonymous]
        [HttpPost("DeleteUser")]
        [Consumes("application/json")]
        public UserControlResponse DeleteUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.DeleteUser(userRequest.UserInfo);
            return controlResponse;
        }
    }
}
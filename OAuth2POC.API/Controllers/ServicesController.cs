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
    public class ServicesController : Controller
    {
        private readonly IServiceProcess _serviceProcess;

        public ServicesController(IServiceProcess serviceProces)
        {
            _serviceProcess = serviceProces;
        }

        [Authorize(Roles = "Admin, User")]
        [Consumes("application/x-www-form-urlencoded")]
        [HttpGet("GetUsers")]
        public UserControlResponse GetAllUsers([FromHeader]BaseHeader header)
        {
            UserControlResponse controlResponse = _serviceProcess.GetUsers();
            return controlResponse;
        }

        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [HttpPost("GetUsers")]
        public UserControlResponse GetUsers([FromHeader]BaseHeader header)
        {
            UserControlResponse controlResponse = _serviceProcess.GetUsers();
            return controlResponse;
        }

        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [HttpPost("GetUser")]
        public UserControlResponse GetUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.GetUser(userRequest.UserInfo);
            return controlResponse;
        }

        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [HttpPost("InsertUser")]
        public UserControlResponse InsertUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.InsertUser(userRequest.UserInfo);
            return controlResponse;
        }

        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [HttpPost("UpdateUser")]
        public UserControlResponse UpdateUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.UpdateUser(userRequest.UserInfo);
            return controlResponse;
        }

        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [HttpPost("DeleteUser")]
        public UserControlResponse DeleteUser([FromHeader]BaseHeader header, UserRequest userRequest)
        {
            UserControlResponse controlResponse = _serviceProcess.DeleteUser(userRequest.UserInfo);
            return controlResponse;
        }
    }
}
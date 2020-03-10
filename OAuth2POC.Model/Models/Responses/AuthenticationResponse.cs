using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.Model.Models.Responses
{
    public class AuthenticationResponse : BaseResponse
    {
        public AuthenticationInfo AuthenticationInfo { get; set; }
        public TokenInfo TokenInfo { get; set; }
    }
}
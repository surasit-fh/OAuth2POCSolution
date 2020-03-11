using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.Model.Models.Requests
{
    public class UserRequest
    {
        public UserInfo UserInfo { get; set; }
    }

    public class AuthenRequest
    {
        public UserInfo UserInfo { get; set; }
        public AuthenticationInfo AuthenticationInfo { get; set; }
        public TokenInfo TokenInfo { get; set; }
    }

    public class TokenRequest
    {
        public TokenInfo TokenInfo { get; set; }
    }

    public class JWTRequest
    {
        public JWTInfo JWTInfo { get; set; }
    }
}
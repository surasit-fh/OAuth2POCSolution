using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.API.Authentications
{
    public class JwtBearerOptions : AuthenticationSchemeOptions
    {
        public JwtBearerOptions()
        {

        }
    }
}
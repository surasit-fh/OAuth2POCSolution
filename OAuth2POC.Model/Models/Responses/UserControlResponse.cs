using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.Model.Models.Responses
{
    public class UserControlResponse : BaseResponse
    {
        public List<UserInfo> UserInfos { get; set; }
    }
}
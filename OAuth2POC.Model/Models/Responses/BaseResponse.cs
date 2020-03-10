using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Models.Responses
{
    public class BaseResponse
    {
        [DefaultValue(null)]
        public bool IsSuccess { get; set; }

        [DefaultValue(null)]
        public int ErrorCode { get; set; }

        [DefaultValue(null)]
        public string Description { get; set; }

        [DefaultValue(null)]
        public string Message { get; set; }
    }
}
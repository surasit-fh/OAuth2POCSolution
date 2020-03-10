using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace OAuth2POC.Model.Headers
{
    public class BaseHeader
    {
        [FromHeader]
        [DataMember(Name = "Authorization")]
        [Required]
        public string Authorization { get; set; }
    }
}
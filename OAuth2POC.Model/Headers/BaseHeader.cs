using Microsoft.AspNetCore.Mvc;
using OAuth2POC.Model.ValidationAttributeHandle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace OAuth2POC.Model.Headers
{
    [ModelBinder(BinderType = typeof(FromHeaderBinder))]
    public class BaseHeader
    {
        [FromHeader]
        [DataMember(Name = "Authorization")]
        [Required]
        public string Authorization { get; set; }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRole : int
    {
        [Description("Admin")]
        Admin = 1,

        [Description("User")]
        User = 2
    }
}
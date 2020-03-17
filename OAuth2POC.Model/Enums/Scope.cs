using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Scope : int
    {
        [Description("Profile")]
        Profile = 0,

        [Description("Email")]
        Email = 1,

        [Description("Address")]
        Address = 2,

        [Description("Phone")]
        Phone = 3,

        [Description("OpenId")]
        OpenId = 4,

        [Description("OfflineAccess")]
        OfflineAccess = 5,

        [Description("ServiceAPI")]
        ServiceAPI = 6,

        [Description("Role")]
        Role = 7
    }
}
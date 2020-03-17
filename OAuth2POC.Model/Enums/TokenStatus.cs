using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TokenStatus : int
    {
        [Description("None")]
        None = 0,

        [Description("Active")]
        Active = 1,

        [Description("Terminate")]
        Terminate = 2,

        [Description("Pending")]
        Pending = 3
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TokenType : int
    {
        [Description("None")]
        None = 0,

        [Description("Bearer")]
        Bearer = 1
    }
}
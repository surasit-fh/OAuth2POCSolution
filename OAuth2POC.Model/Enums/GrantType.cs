using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GrantType : int
    {
        [Description("Authorization Code")]
        AuthorizationCode = 1,

        [Description("Implicit")]
        Implicit = 2,

        [Description("Resource Owner Password Credentials")]
        ResourceOwnerPasswordCredentials = 3,

        [Description("Client Credentials")]
        ClientCredentials = 4,

        [Description("Device Code")]
        DeviceCode = 5,

        [Description("Refresh Token")]
        RefreshToken = 6
    }
}
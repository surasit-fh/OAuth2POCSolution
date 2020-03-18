using Newtonsoft.Json;
using OAuth2POC.Client.Adapters;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Services
{
    public class AuthenticationService
    {
        private readonly Basic_Authentication_Adapter _basicAuthen;

        public AuthenticationService(int connectionTimeout, int bufferSize, string contentType, string accept, string username, string password)
        {
            _basicAuthen = new Basic_Authentication_Adapter(connectionTimeout, bufferSize, contentType, accept, username, password);
        }

        public AuthenticationResponse GetAuthorization(AuthenticationInfo authenticationInfo, TokenInfo tokenInfo)
        {
            try
            {
                string request = JsonConvert.SerializeObject(authenticationInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                request = "{\"AuthenticationInfo\":" + request + "}";

                if (authenticationInfo.GrantType == GrantType.RefreshToken)
                {
                    string tokenRequest = JsonConvert.SerializeObject(tokenInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                    request = "{\"AuthenticationInfo\":" + request + ", \"TokenInfo\":" + tokenRequest + "}";
                }                

                string url = "https://localhost:44350/api/auth/oauth2/v2/accounts/login";
                string result = _basicAuthen.AuthenticationHandle(url, HttpMethod.Post, request);
                AuthenticationResponse authenResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(result);
                return authenResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
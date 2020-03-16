using Newtonsoft.Json;
using OAuth2POC.Client.Adapters;
using OAuth2POC.Client.Process.IProcess;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Process
{
    public class UserProcess : IUserProcess
    {
        public AuthenticationResponse Authentication(UserInfo userInfo, AuthenticationInfo authenticationInfo, TokenInfo tokenInfo)
        {
            string url = "https://localhost:44350/api/auth/oauth2/v2/accounts/login";
            string strRequest = string.Empty;

            if (authenticationInfo.GrantType == GrantType.RefreshToken)
            {

            }
            else
            {
                strRequest = JsonConvert.SerializeObject(authenticationInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                strRequest = "{\"AuthenticationInfo\":" + strRequest + "}";
            }

            try
            {
                Basic_Authentication_Adapter basicAuthen = new Basic_Authentication_Adapter(30, 30, "application/json", "application/json", userInfo.Username, userInfo.Password);
                string result = basicAuthen.AuthenticationHandle(url, HttpMethod.Post, strRequest);
                AuthenticationResponse authenResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(result);
                return authenResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse GetUsers(string token)
        {
            try
            {
                string url = "https://localhost:44345/api/auth/oauth2/v2/services/getusers";
                string request = "";

                Bearer_Authentication_Adapter bearerAuthen = new Bearer_Authentication_Adapter(30, 30, "application/x-www-form-urlencoded", "application/json");
                string result = bearerAuthen.AuthenticationHandle(url, HttpMethod.Get, request, token);
                UserControlResponse userResponse = JsonConvert.DeserializeObject<UserControlResponse>(result);
                return userResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse GetUser(string token, UserInfo userInfo)
        {
            try
            {
                string url = "https://localhost:44345/api/auth/oauth2/v2/services/getuser";
                string request = JsonConvert.SerializeObject(userInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                request = "{\"UserInfo\":" + request + "}";

                Bearer_Authentication_Adapter bearerAuthen = new Bearer_Authentication_Adapter(30, 30, "application/json", "application/json");
                string result = bearerAuthen.AuthenticationHandle(url, HttpMethod.Post, request, token);
                UserControlResponse userResponse = JsonConvert.DeserializeObject<UserControlResponse>(result);
                return userResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
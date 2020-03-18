using Newtonsoft.Json;
using OAuth2POC.Client.Adapters;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Services
{
    public class UserService
    {
        private readonly Bearer_Authentication_Adapter _bearerAuthen;
        public UserService(int connectionTimeout, int bufferSize, string contentType, string accept)
        {
            _bearerAuthen = new Bearer_Authentication_Adapter(connectionTimeout, bufferSize, contentType, accept);
        }

        public UserControlResponse GetUsers(string token)
        {
            try
            {
                string url = "https://localhost:44345/api/auth/oauth2/v2/services/getusers";
                string result = _bearerAuthen.AuthenticationHandle(url, HttpMethod.Get, string.Empty, token);
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
                string request = JsonConvert.SerializeObject(userInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                request = "{\"UserInfo\":" + request + "}";
                string url = "https://localhost:44345/api/auth/oauth2/v2/services/getuser";
                string result = _bearerAuthen.AuthenticationHandle(url, HttpMethod.Post, request, token);
                UserControlResponse userResponse = JsonConvert.DeserializeObject<UserControlResponse>(result);
                return userResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse InsertUser(string token, UserInfo userInfo)
        {
            try
            {
                string request = JsonConvert.SerializeObject(userInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                request = "{\"UserInfo\":" + request + "}";
                string url = "https://localhost:44345/api/auth/oauth2/v2/services/insertuser";
                string result = _bearerAuthen.AuthenticationHandle(url, HttpMethod.Post, request, token);
                UserControlResponse userResponse = JsonConvert.DeserializeObject<UserControlResponse>(result);
                return userResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse UpdateUser(string token, UserInfo userInfo)
        {
            try
            {
                string request = JsonConvert.SerializeObject(userInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                request = "{\"UserInfo\":" + request + "}";
                string url = "https://localhost:44345/api/auth/oauth2/v2/services/updateuser";
                string result = _bearerAuthen.AuthenticationHandle(url, HttpMethod.Post, request, token);
                UserControlResponse userResponse = JsonConvert.DeserializeObject<UserControlResponse>(result);
                return userResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserControlResponse DeleteUser(string token, UserInfo userInfo)
        {
            try
            {
                string request = JsonConvert.SerializeObject(userInfo, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                request = "{\"UserInfo\":" + request + "}";
                string url = "https://localhost:44345/api/auth/oauth2/v2/services/deleteuser";
                string result = _bearerAuthen.AuthenticationHandle(url, HttpMethod.Post, request, token);
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
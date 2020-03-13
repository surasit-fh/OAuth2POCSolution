using Newtonsoft.Json;
using OAuth2POC.Client.Adapters;
using OAuth2POC.Client.Services.IServices;
using OAuth2POC.Model.Models.Interface;
using OAuth2POC.Model.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Services
{
    public class UserService : IUserService
    {
        public AuthenticationResponse Authentication()
        {
            try
            {
                string url = "https://localhost:44350/api/auth/oauth2/v2/accounts/login";
                string request = "";
                Basic_Authentication_Adapter basicAuthen = new Basic_Authentication_Adapter(30, 30, "application/json", "application/json", "", "");
                string result = basicAuthen.AuthenticationHandle(url, HttpMethod.Post, request);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserInfo> GetUsers()
        {
            try
            {
                string url = "https://localhost:44350/api/auth/oauth2/v2/accounts/login";
                string request = "";
                string token = "";
                Bearer_Authentication_Adapter bearerAuthen = new Bearer_Authentication_Adapter(30, 30, "application/x-www-form-urlencoded", "application/json");
                string result = bearerAuthen.AuthenticationHandle(url, HttpMethod.Get, request, token);
                List<UserInfo> response = JsonConvert.DeserializeObject<List<UserInfo>>(result);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserInfo> GetUsers()
        {
            try
            {
                string url = "https://localhost:44350/api/auth/oauth2/v2/accounts/login";
                string request = "";
                Basic_Authentication_Adapter basicAuthen = new Basic_Authentication_Adapter(30, 30, "", "", "", "");
                string result = basicAuthen.AuthenticationHandle(url, HttpMethod.Post, request);
                List<UserInfo> response = JsonConvert.DeserializeObject<List<UserInfo>>(result);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
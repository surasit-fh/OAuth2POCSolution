using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Adapters
{
    public class HTTP_Protocol_Adapter
    {
        private int _connectionTimeoutInSec;
        private int _bufferSizeInKB;
        private string _contentType;
        private string _accept;
        private string _username;
        private string _password;

        public HTTP_Protocol_Adapter(int connectionTimeoutInSec, int bufferSizeInKB, string contentType, string accept, string username, string password)
        {
            this._connectionTimeoutInSec = connectionTimeoutInSec < 0 ? 30 * 1000 : connectionTimeoutInSec * 1000;
            this._bufferSizeInKB = bufferSizeInKB * 1024;
            this._contentType = contentType;
            this._accept = accept;
            this._username = username;
            this._password = password;
        }

        internal string GetStringResponse(string strUrl, HttpMethod httpMethod, string strRequest, string token)
        {
            string response = string.Empty;

            try
            {
                HttpWebResponse httpWebResponse = GetHttpWebResponse(strUrl, httpMethod, strRequest, token);

                using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        internal HttpWebResponse GetHttpWebResponse(string strUrl, HttpMethod httpMethod, string strRequest, string token)
        {
            HttpWebResponse httpWebResponse = null;

            try
            {
                Uri uri = new Uri(strUrl);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest = SetHttpWebRequestAttribute(httpWebRequest, httpMethod);
                httpWebRequest.PreAuthenticate = true;

                if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
                {
                    byte[] postByte = Encoding.UTF8.GetBytes(strRequest);
                    httpWebRequest.ContentLength = postByte.Length;
                    Stream stream = httpWebRequest.GetRequestStream();
                    stream.Write(postByte, 0, postByte.Length);
                    stream.Flush();
                    stream.Close();
                }

                if (!string.IsNullOrEmpty(token))
                {
                    httpWebRequest.Headers.Add("Bearer", token);
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                else
                {
                    byte[] clientCredentials = Encoding.UTF8.GetBytes($"{this._username}:{this._password}");
                    httpWebRequest.Headers.Add("Basic", Convert.ToBase64String(clientCredentials));
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return httpWebResponse;
        }

        private HttpWebRequest SetHttpWebRequestAttribute(HttpWebRequest httpWebRequest, HttpMethod httpMethod)
        {
            httpWebRequest.Timeout = this._connectionTimeoutInSec;
            httpWebRequest.KeepAlive = true;
            httpWebRequest.ContentType = this._contentType;
            httpWebRequest.Accept = this._accept;
            httpWebRequest.Method = httpMethod.ToString();
            return httpWebRequest;
        }
    }
}
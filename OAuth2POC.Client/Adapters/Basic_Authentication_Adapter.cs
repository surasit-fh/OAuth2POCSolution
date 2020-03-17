using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.Client.Adapters
{
    public class Basic_Authentication_Adapter
    {
        private int _connectionTimeout;
        private int _bufferSize;
        private string _contentType;
        private string _accept;
        private string _username;
        private string _password;

        public Basic_Authentication_Adapter(int connectionTimeoutInSec, int bufferSizeInKB, string contentType, string accept, string username, string password)
        {
            this._connectionTimeout = connectionTimeoutInSec > 0 ? connectionTimeoutInSec * 1000 : 30 * 1000;
            this._bufferSize = bufferSizeInKB * 1024;
            this._contentType = contentType;
            this._accept = accept;
            this._username = username;
            this._password = password;
        }

        public string AuthenticationHandle(string strURL, HttpMethod httpMethod, string strRequest)
        {
            string response = string.Empty;
            HttpClient client = new HttpClient();
            HttpRequestMessage httpRequest = null;
            HttpResponseMessage httpResponse = null;

            try
            {
                Uri uri = new Uri(strURL);
                byte[] clientCredentials = Encoding.ASCII.GetBytes($"{this._username}:{this._password}");
                client.Timeout = new TimeSpan(0, 0, this._connectionTimeout);
                client.MaxResponseContentBufferSize = this._bufferSize;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(this._accept));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCredentials));

                httpRequest = new HttpRequestMessage(httpMethod, uri);
                httpRequest.Content = new StringContent(strRequest, Encoding.UTF8, this._contentType);

                httpResponse = client.SendAsync(httpRequest).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = httpResponse.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception($"Error status code is : " + httpResponse.StatusCode.ToString());
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (Stream stream = ex.Response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            response = streamReader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                httpResponse.Dispose();
                httpRequest.Dispose();
                client.Dispose();
            }

            return response;
        }
    }
}
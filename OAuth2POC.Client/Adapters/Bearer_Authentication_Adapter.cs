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
    public class Bearer_Authentication_Adapter
    {
        private int _connectionTimeout;
        private int _bufferSize;
        private string _contentType;
        private string _accept;

        public Bearer_Authentication_Adapter(int connectionTimeoutInSec, int bufferSizeInKB, string contentType, string accept)
        {
            this._connectionTimeout = connectionTimeoutInSec > 0 ? connectionTimeoutInSec * 1000 : 30 * 1000;
            this._bufferSize = bufferSizeInKB * 1024;
            this._contentType = contentType;
            this._accept = accept;
        }

        public string AuthenticationHandle(string strURL, HttpMethod httpMethod, string strRequest, string accessToken)
        {
            string response = string.Empty;
            HttpClient client = new HttpClient();
            StringContent content = null;
            HttpRequestMessage httpRequest = null;
            HttpResponseMessage httpResponse = null;

            try
            {
                Uri uri = new Uri(strURL);
                client.Timeout = new TimeSpan(0, 0, this._connectionTimeout);
                client.MaxResponseContentBufferSize = this._bufferSize;
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(this._accept));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                httpRequest = new HttpRequestMessage(httpMethod, uri);

                if (!string.IsNullOrEmpty(strRequest))
                {
                    content = new StringContent(strRequest, Encoding.ASCII, this._contentType);
                    httpRequest.Content = content;
                }

                httpResponse = client.GetAsync(strRequest).Result;

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
                content.Dispose();
                client.Dispose();
            }

            return response;
        }
    }
}
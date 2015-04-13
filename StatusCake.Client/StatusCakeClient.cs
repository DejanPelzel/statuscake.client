using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusCake.Client.Auth;
using StatusCake.Client.Models;
using System.Net;
using System.Web;
using System.IO;
using StatusCake.Client.Extensions;
using Newtonsoft.Json;

namespace StatusCake.Client
{
    /// <summary>
    ///     
    /// </summary>
    public class StatusCakeClient
    {
        /// <summary>
        /// The statuscake api endpoint url
        /// </summary>
        public const string ApiEndpoint = "https://www.statuscake.com/API/";

        private string _apiUsername = "";
        private string _apiAccessKey = "";

        /// <summary>
        /// Create a new StatusCakeClient object with the default credentials
        /// </summary>
        public StatusCakeClient()
        {
            this._apiAccessKey = StatusCakeAuth.DefaultAccessKey;
            this._apiUsername = StatusCakeAuth.DefaultUsername;
        }

        /// <summary>
        /// Create a new StatusCakeClient object with the given authentication credentials
        /// </summary>
        public StatusCakeClient(string username, string accessKey)
        {
            this._apiAccessKey = accessKey;
            this._apiUsername = username;
        }

        /// <summary>
        /// Get the list of all the tests in the client
        /// </summary>
        /// <returns></returns>
        public async Task<List<Test>> GetAllTests()
        {
            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.Tests, "GET", null);
            using(var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<List<Test>>(
                    await response.GetResponseStringAsync()
                );
            }
        }

        /// <summary>
        /// Crea
        /// </summary>
        /// <returns></returns>
        public HttpWebRequest GetAuthenticationRequest(string endpoint, string method, NameValueCollection parameters)
        {
            var parameterBuilder = new StringBuilder();

            // Authentication
            parameterBuilder.Append("?Username=");
            parameterBuilder.Append(HttpUtility.UrlEncode(this._apiUsername));
            parameterBuilder.Append("&API=");
            parameterBuilder.Append(HttpUtility.UrlEncode(this._apiAccessKey));
            
            // Parameters
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    parameterBuilder.Append("&");
                    parameterBuilder.Append(parameter.Key);
                    parameterBuilder.Append("=");
                    parameterBuilder.Append(HttpUtility.UrlEncode(parameter.Value));
                }
            }

            var parameterString = parameterBuilder.ToString();

            // Create the request
            HttpWebRequest request;
            if(method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                request = (HttpWebRequest)HttpWebRequest.CreateHttp(ApiEndpoint + endpoint + "/" + parameterString);
            }
            else
            {
                request = (HttpWebRequest)HttpWebRequest.CreateHttp(ApiEndpoint + endpoint);

                // Send the POST data
                byte[] requestData = Encoding.ASCII.GetBytes(parameterString);
                using(var requstStream = request.GetRequestStream())
                {
                    requstStream.Write(requestData, 0, requestData.Length);
                    requstStream.Close();
                }
            }
            
            request.Method = method;
            return request;
        }
    }
}

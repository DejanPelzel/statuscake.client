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
    /// TODO
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
        /// The default instance of StatusCakeClient that can be statically accessed
        /// </summary>
        private static StatusCakeClient _defaultInstance = null;

        /// <summary>
        /// A default instance of the StatusCakeClient, using the credentials from the configuration file
        /// </summary>
        public static StatusCakeClient Instance 
        {
            get
            {
                if (_defaultInstance == null)
                {
                    _defaultInstance = new StatusCakeClient();
                }

                return _defaultInstance;
            }
        }

        // ~~~ GET methods
        #region GET: GetTestDetailsAsync
        /// <summary>
        /// Get the details of the test with the given test ID
        /// </summary>
        /// <returns></returns>
        public async Task<TestDetails> GetTestDetailsAsync(long testId)
        {
            var parameters = new NameValueCollection();
            parameters.Add("TestID", testId.ToString());

            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.TestDetails, "GET", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<TestDetails>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        #region GET: GetTestsAsync
        /// <summary>
        /// Get the list of all the tests in the client
        /// </summary>
        /// <returns></returns>
        public async Task<List<Test>> GetTestsAsync()
        {
            return await this.GetTestsAsync(null, null);
        }

        /// <summary>
        /// Get the list of all the tests in the client optionally filtered by group ID and status
        /// </summary>
        /// <param name="contactGroupId">Filter to just the tests using the contact group ID. Set to null to skip this filter.</param>
        /// <param name="status">Filter to just the tests that currently have the given status. Set to null to skip this filter.</param>
        /// <returns></returns>
        public async Task<List<Test>> GetTestsAsync(long? contactGroupId, string status)
        {
            var parameters = new NameValueCollection();
            // Add the filtering parameters
            if (contactGroupId != null)
            {
                parameters.Add("ContactGroupID", contactGroupId.Value.ToString());
            }
            // Filter by status
            if (status != null)
            {
                parameters.Add("Status", status);
            }


            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.Tests, "GET", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<List<Test>>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        #region GET: GetPeriodsAsync
        /// <summary>
        /// Get the list of all the periods linked to the test ID
        /// </summary>
        /// <returns></returns>
        public async Task<List<Period>> GetPeriodsAsync(long testId)
        {
            var parameters = new NameValueCollection();
            parameters.Add("TestID", testId.ToString());

            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.Periods, "GET", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<List<Period>>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        #region GET: GetCheckResultsAsync
        /// <summary>
        /// Get a list of all the check results for the given test
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <returns></returns>
        public async Task<List<Alert>> GetCheckResultsAsync(long testId)
        {
            return await this.GetCheckResultsAsync(testId, null, null, null);
        }

        /// <summary>
        /// Get a list of all the check results for the given test
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <returns></returns>
        public async Task<List<Alert>> GetCheckResultsAsync(long testId, int limit)
        {
            return await this.GetCheckResultsAsync(testId, null, null, limit);
        }

        /// <summary>
        /// Get a list of all the check results for the given test filtered by the given parameters
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <param name="fields">TODO</param>
        /// <returns></returns>
        public async Task<List<Alert>> GetCheckResultsAsync(long testId, string[] fields)
        {
            return await this.GetCheckResultsAsync(testId, fields, null, null);
        }

        /// <summary>
        /// Get a list of all the check results for the given test filtered by the given parameters
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <param name="fields">TODO</param>
        /// <param name="startTime">TODO</param>
        /// <returns></returns>
        public async Task<List<Alert>> GetCheckResultsAsync(long testId, string[] fields, DateTime? startTime)
        {
            return await this.GetCheckResultsAsync(testId, fields, startTime, null);
        }

        /// <summary>
        /// Get a list of all the check results for the given test filtered by the given parameters
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <param name="fields">TODO</param>
        /// <param name="startTime">TODO</param>
        /// <param name="limit">TODO</param>
        /// <returns></returns>
        public async Task<List<Alert>> GetCheckResultsAsync(long testId, string[] fields, DateTime? startTime, int? limit)
        {
            // Validate the limit
            if(limit != null && (limit > 1000 || limit < 1))
            {
                throw new ArgumentException("Limit cannot be bigger than 1000 or smaller than 1");
            }
            
            var parameters = new NameValueCollection();
            parameters.Add("TestID", testId.ToString());

            // Fields parameter
            if (fields != null)
            {
                parameters.Add("Fields", string.Join(", ", fields));
            }

            // Start parameter
            if (startTime != null)
            {
                parameters.Add("Start", startTime.Value.ToUnix().ToString());
            }

            // Limit parameter
            if(limit != null)
            {
                parameters.Add("Limit", limit.Value.ToString());
            }

            // Send the request
            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.Alerts, "GET", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<List<Alert>>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        #region GET: GetAlertsAsync
        /// <summary>
        /// Get the list of all the alerts linked to the test
        /// </summary>
        /// <returns></returns>
        public async Task<List<Alert>> GetAlertsAsync(long testId)
        {
            return await this.GetAlertsAsync(testId, null);
        }

        /// <summary>
        /// Get the list of all the alerts linked to the test
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <param name="since">TODO</param>
        /// <returns></returns>
        public async Task<List<Alert>> GetAlertsAsync(long testId, DateTime? since)
        {
            var parameters = new NameValueCollection();
            parameters.Add("TestID", testId.ToString());

            // Add the since parameter
            if(since != null)
            {
                parameters.Add("Since", since.Value.ToUnix().ToString());
            }

            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.Alerts, "GET", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<List<Alert>>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        #region GET: GetAuthDetailsAsync
        /// <summary>
        /// Get the details of the currently authenticated user
        /// </summary>
        /// <returns></returns>
        public async Task<Models.Auth> GetAuthDetailsAsync()
        {
            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.Auth, "GET", null);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<Models.Auth>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        #region GET: GetContactGroupsAsync
        /// <summary>
        /// Get the details of the currently authenticated user
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContactGroup>> GetContactGroupsAsync()
        {
            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.ContactGroups, "GET", null);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<List<ContactGroup>>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        // ~~~ DELETE methods
        #region DELETE: DeleteTestAsync
        /// <summary>
        /// Get the details of the test with the given test ID
        /// </summary>
        /// <returns></returns>
        public async Task<DeleteTest> DeleteTestAsync(long testId)
        {
            var parameters = new NameValueCollection();
            parameters.Add("TestID", testId.ToString());

            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.TestDetails, "DELETE", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<DeleteTest>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion


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
                foreach (var paramKey in parameters.Keys)
                {
                    var value = parameters[paramKey.ToString()];

                    parameterBuilder.Append("&");
                    parameterBuilder.Append(paramKey);
                    parameterBuilder.Append("=");
                    parameterBuilder.Append(HttpUtility.UrlEncode(value));
                }
            }

            var parameterString = parameterBuilder.ToString();

            // Create the request
            HttpWebRequest request;
            if (method.Equals("GET", StringComparison.OrdinalIgnoreCase) || method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
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

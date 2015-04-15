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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
            // Add the filtering dataParameters
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
        /// Get a list of all the check results for the given test filtered by the given dataParameters
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <param name="fields">TODO</param>
        /// <returns></returns>
        public async Task<List<Alert>> GetCheckResultsAsync(long testId, string[] fields)
        {
            return await this.GetCheckResultsAsync(testId, fields, null, null);
        }

        /// <summary>
        /// Get a list of all the check results for the given test filtered by the given dataParameters
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
        /// Get a list of all the check results for the given test filtered by the given dataParameters
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

        /// <summary>
        /// Get the details of the test with the given test ID
        /// </summary>
        /// <returns></returns>
        public async Task<DeleteTest> DeleteContactGroupAsync(long contactId)
        {
            var parameters = new NameValueCollection();
            parameters.Add("ContactID", contactId.ToString());

            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.ContactGroups, "DELETE", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<DeleteTest>(
                    await response.GetResponseStringAsync()
                );
            }
        }
        #endregion

        // ~~~ PUT methods
        #region PUT: UpdateOrCreateContactGroupAsync
        /// <summary>
        /// Update or create a new status cake ContactGroup
        /// </summary>
        /// <param name="contactGroup"></param>
        public async Task<PutResponse> UpdateOrCreateContactGroupAsync(ContactGroup contactGroup)
        {
            // Validate the group
            if (contactGroup == null)
            {
                throw new ArgumentNullException("ContactGroup cannot be null");
            }

            ValidationContext context = new ValidationContext(contactGroup, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            // Validate the model
            if (Validator.TryValidateObject(contactGroup, context, results))
            {
                // Wire up the dataParameters
                var dataParameters = new NameValueCollection();
                dataParameters.Add("Boxcar", contactGroup.Boxcar);
                dataParameters.Add("ContactID", contactGroup.ContactID > 0 ? contactGroup.ContactID.ToString() : null);
                dataParameters.Add("DesktopAlert", contactGroup.DesktopAlert.ToString());
                dataParameters.Add("Emails", contactGroup.Emails != null ? string.Join(", ", contactGroup.Emails) : null);
                dataParameters.Add("GroupName", contactGroup.GroupName);
                dataParameters.Add("Mobiles", contactGroup.Mobiles != null ? string.Join(", ", contactGroup.Mobiles) : null);
                dataParameters.Add("PingURL", contactGroup.PingURL);
                dataParameters.Add("Pushover", contactGroup.Pushover);

                var request = this.GetAuthenticatedPutRequest(StatusCakeEndpoints.ContactGroupsUpdate, dataParameters);
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    return JsonConvert.DeserializeObject<PutResponse>(
                        await response.GetResponseStringAsync()
                    );
                }
            }
            else
            {
                throw new ValidationException("ContactGroup was not valid");
            }
        }
        #endregion

        /// <summary>
        /// Update or create a new status cake Test
        /// </summary>
        /// <param name="contactGroup"></param>
        public async Task<PutResponse> UpdateOrCreateTestAsync(TestUpdate test)
        {
            // Validate the group
            if (test == null)
            {
                throw new ArgumentNullException("Test cannot be null");
            }

            ValidationContext context = new ValidationContext(test, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            // Validate the model
            if (Validator.TryValidateObject(test, context, results))
            {
                // Wire up the dataParameters
                var dataParameters = new NameValueCollection();
                dataParameters.Add("TestID", test.TestID > 0 ? test.TestID.ToString() : null);
                dataParameters.Add("Paused", test.Paused == null ? null : (test.Paused.Value ? "1" : "0"));
                dataParameters.Add("WebsiteName", test.WebsiteName);
                dataParameters.Add("WebsiteURL", test.WebsiteURL);
                dataParameters.Add("Port", test.Paused == null ? null : test.Port.ToString());
                dataParameters.Add("NodeLocations", test.NodeLocations != null ? string.Join(",", test.NodeLocations) : null);
                dataParameters.Add("Timeout", test.Timeout == null ? null : test.Timeout.Value.ToString());
                dataParameters.Add("PingURL", test.PingURL);
                dataParameters.Add("Confirmation", test.Confirmations == null ? null : test.Confirmations.Value.ToString());
                dataParameters.Add("CheckRate", test.CheckRate.ToString());
                dataParameters.Add("Public", test.Public == null ? null : (test.Public.Value ? "1" : "0"));
                dataParameters.Add("LogoImage", test.LogoImageUrl);
                dataParameters.Add("Branding", test.DisplayBranding == null ? null : (test.DisplayBranding.Value ? "1" : "0"));
                dataParameters.Add("WebsiteHost", test.WebsiteHost);
                dataParameters.Add("Virus", test.VirusCheckEnabled == null ? null : (test.VirusCheckEnabled.Value ? "1" : "0"));
                dataParameters.Add("FindString", test.FindString);
                dataParameters.Add("DoNotFind", test.DoNotFind == null ? null : (test.DoNotFind.Value ? "1" : "0"));
                dataParameters.Add("TestType", test.TestType);
                dataParameters.Add("ContactGroup", test.ContactGroupID == null ? null : test.ContactGroupID.Value.ToString());
                dataParameters.Add("RealBrowser", test.TestWithRealBrowser == null ? null : (test.TestWithRealBrowser.Value ? "1" : "0"));
                dataParameters.Add("TriggerRate", test.TriggerRate == null ? null : test.TriggerRate.Value.ToString());
                dataParameters.Add("TestTags", test.Tags != null ? string.Join(",", test.Tags) : null);
                dataParameters.Add("StatusCodes", test.StatusCodes != null ? string.Join(",", test.StatusCodes) : null);

                var request = this.GetAuthenticatedPutRequest(StatusCakeEndpoints.TestsUpdate, dataParameters);
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    return JsonConvert.DeserializeObject<PutResponse>(
                        await response.GetResponseStringAsync()
                    );
                }
            }
            else
            {
                throw new ValidationException("Test was not valid");
            }
        }

        /// <summary>
        /// Create an authenticated PUT request to the status cake API
        /// </summary>
        /// <returns></returns>
        public HttpWebRequest GetAuthenticatedPutRequest(string endpoint, NameValueCollection dataParameters)
        {
            // Build the AUTH query string
            string authQuery = string.Format("?Username={0}&API={1}",
                HttpUtility.UrlEncode(this._apiUsername),
                HttpUtility.UrlEncode(this._apiAccessKey));

            // Build the data parameters
            var dataParameterBuilder = new StringBuilder();
            if (dataParameters != null)
            {
                foreach (var paramKey in dataParameters.Keys)
                {
                    var value = dataParameters[paramKey.ToString()];
                    // Skip empty values
                    if (value != null)
                    {
                        dataParameterBuilder.Append("&");
                        dataParameterBuilder.Append(paramKey);
                        dataParameterBuilder.Append("=");
                        dataParameterBuilder.Append(HttpUtility.UrlEncode(value));
                    }
                }
            }

            // Create the request
            var request = (HttpWebRequest)HttpWebRequest.CreateHttp(ApiEndpoint + endpoint + "/" + authQuery);
            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";

            // Send the data
            byte[] requestData = Encoding.ASCII.GetBytes(dataParameterBuilder.ToString());
            using (var requstStream = request.GetRequestStream())
            {
                requstStream.Write(requestData, 0, requestData.Length);
                requstStream.Close();
            }
            return request;
        }

        /// <summary>
        /// Create an authenticated request to the status cake API
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
                request.Method = method;
            }
            else
            {
                // Send the POST data
                request = (HttpWebRequest)HttpWebRequest.CreateHttp(ApiEndpoint + endpoint);
                request.Method = method;

                byte[] requestData = Encoding.ASCII.GetBytes(parameterString);
                using (var requstStream = request.GetRequestStream())
                {
                    requstStream.Write(requestData, 0, requestData.Length);
                    requstStream.Close();
                }
            }

            return request;
        }
    }
}

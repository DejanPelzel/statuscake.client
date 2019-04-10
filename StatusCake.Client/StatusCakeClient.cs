using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using StatusCake.Client.Auth;
using StatusCake.Client.Models;
using System.Net;
using System.Web;
using StatusCake.Client.Extensions;
using Newtonsoft.Json;
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
        public const string ApiEndpoint = "https://app.statuscake.com/API/";

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
        public static StatusCakeClient Instance => _defaultInstance ?? (_defaultInstance = new StatusCakeClient());

        // ~~~ GET methods
        #region GET: GetTestDetailsAsync
        /// <summary>
        /// Get the details of the test with the given test ID
        /// </summary>
        /// <returns></returns>
        public async Task<TestDetails> GetTestDetailsAsync(long testId)
        {
            var parameters = new NameValueCollection {{"TestID", testId.ToString()}};

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
            var parameters = new NameValueCollection {{"TestID", testId.ToString()}};

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
        public async Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId)
        {
            return await this.GetCheckResultsAsync(testId, null, null, null);
        }

        /// <summary>
        /// Get a list of all the check results for the given test
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <returns></returns>
        public async Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, int limit)
        {
            return await this.GetCheckResultsAsync(testId, null, null, limit);
        }

        /// <summary>
        /// Get a list of all the check results for the given test filtered by the given dataParameters
        /// </summary>
        /// <param name="testId">TODO</param>
        /// <param name="fields">TODO</param>
        /// <returns></returns>
        public async Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, string[] fields)
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
        public async Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, string[] fields, DateTime? startTime)
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
        public async Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, string[] fields, DateTime? startTime, int? limit)
        {
            // Validate the limit
            if (limit != null && (limit > 1000 || limit < 1))
            {
                throw new ArgumentException("Limit cannot be bigger than 1000 or smaller than 1");
            }

            var parameters = new NameValueCollection {{"TestID", testId.ToString()}};

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
            if (limit != null)
            {
                parameters.Add("Limit", limit.Value.ToString());
            }

            // Send the request
            var request = this.GetAuthenticationRequest(StatusCakeEndpoints.TestChecks, "GET", parameters);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                return JsonConvert.DeserializeObject<Dictionary<string, CheckResult>>(
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
            var parameters = new NameValueCollection {{"TestID", testId.ToString()}};

            // Add the since parameter
            if (since != null)
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

        #region Get: GetUptimes
        /// <summary>
        /// Get all uptime precentage per day since test creation
        /// </summary>
        /// <returns></returns>
        public async Task<SortedDictionary<DateTime, Availability>> GetUptimesAsync(long testId)
        {
            var periods = await GetPeriodsAsync(testId);
            
            var availability = new SortedDictionary<DateTime, Availability>();

            foreach (var period in periods)
            {
                var day = new TimeSpan(1, 0, 0, 0);

                // longer than 1 day
                if (period.Start.Date != period.End.Date)
                {
                    var date = period.Start;

                    while (date < period.End)
                    {
                        var endOfDay = date.Date.AddDays(1).Subtract(new TimeSpan(0, 0, 1));
                        var elapsedTime = period.End < endOfDay ? period.End.Subtract(date) : endOfDay.Subtract(date);
                        var percent = (elapsedTime.TotalSeconds / (day.TotalSeconds - 1)) * 100;

                        UpdateAvailabilityDictionary(date, period, ref availability, percent);

                        // go to next day in period
                        date = date.Date.AddDays(1);
                    }
                }
                // shorten than 1 day
                else
                {
                    var elapsedTime = period.End.Subtract(period.Start);
                    var percent = (elapsedTime.TotalSeconds / (day.TotalSeconds - 1)) * 100;

                    UpdateAvailabilityDictionary(period.Start, period, ref availability, percent);
                }
            }

            return availability;
        }

        private void UpdateAvailabilityDictionary(DateTime date, Period period, ref SortedDictionary<DateTime,Availability> availability, double percent)
        {
            // uptime
            if (period.Status == Enumerators.TestStatus.Up)
            {
                // update
                if (availability.ContainsKey(date.Date))
                {
                    availability[date.Date].Uptime += percent;
                }
                // create
                else
                {
                    availability.Add(date.Date, new Availability() { Downtime = 0, Uptime = percent });
                }
            }
            // downtime
            else
            {
                // update
                if (availability.ContainsKey(date.Date))
                {
                    availability[date.Date].Downtime += percent;
                }
                // create
                else
                {
                    availability.Add(date.Date, new Availability() { Downtime = percent, Uptime = 0 });
                }
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
            var parameters = new NameValueCollection {{"TestID", testId.ToString()}};

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
            var parameters = new NameValueCollection {{"ContactID", contactId.ToString()}};

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

            var context = new ValidationContext(contactGroup, null, null);
            var results = new List<ValidationResult>();

            // Validate the model
            if (Validator.TryValidateObject(contactGroup, context, results))
            {
                // Wire up the dataParameters
                var dataParameters = new NameValueCollection
                {
                    {"Boxcar", contactGroup.Boxcar},
                    {"ContactID", contactGroup.ContactID > 0 ? contactGroup.ContactID.ToString() : null},
                    {"DesktopAlert", contactGroup.DesktopAlert.ToString()},
                    {"Emails", contactGroup.Emails != null ? string.Join(", ", contactGroup.Emails) : null},
                    {"GroupName", contactGroup.GroupName},
                    {"Mobiles", contactGroup.Mobiles != null ? string.Join(", ", contactGroup.Mobiles) : null},
                    {"PingURL", contactGroup.PingURL},
                    {"Pushover", contactGroup.Pushover}
                };

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

            var context = new ValidationContext(test, null, null);
            var results = new List<ValidationResult>();

            // Validate the model
            if (Validator.TryValidateObject(test, context, results))
            {
                // Wire up the dataParameters
                var dataParameters = new NameValueCollection
                {
                    {"TestID", test.TestID > 0 ? test.TestID.ToString() : null},
                    {"Paused", test.Paused == null ? null : (test.Paused.Value ? "1" : "0")},
                    {"WebsiteName", test.WebsiteName},
                    {"WebsiteURL", test.WebsiteURL},
                    {"Port", test.Paused == null ? null : test.Port.ToString()},
                    {"NodeLocations", test.NodeLocations != null ? string.Join(",", test.NodeLocations) : null},
                    {"Timeout", test.Timeout?.ToString()},
                    {"PingURL", test.PingURL},
                    {"Confirmation", test.Confirmations?.ToString()},
                    {"CheckRate", test.CheckRate.ToString()},
                    {"Public", test.Public == null ? null : (test.Public.Value ? "1" : "0")},
                    {"LogoImage", test.LogoImageUrl},
                    {"Branding", test.DisplayBranding == null ? null : (test.DisplayBranding.Value ? "1" : "0")},
                    {"WebsiteHost", test.WebsiteHost},
                    {"Virus", test.VirusCheckEnabled == null ? null : (test.VirusCheckEnabled.Value ? "1" : "0")},
                    {"FindString", test.FindString},
                    {"DoNotFind", test.DoNotFind == null ? null : (test.DoNotFind.Value ? "1" : "0")},
                    {"TestType", test.TestType.ToString()},
                    {"ContactGroup", test.ContactGroupID?.ToString()},
                    {
                        "RealBrowser",
                        test.TestWithRealBrowser == null ? null : (test.TestWithRealBrowser.Value ? "1" : "0")
                    },
                    {"TriggerRate", test.TriggerRate?.ToString()},
                    {"TestTags", test.Tags != null ? string.Join(",", test.Tags) : null},
                    {"StatusCodes", test.StatusCodes != null ? string.Join(",", test.StatusCodes) : null}
                };

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
            var authQuery =
                $"?Username={HttpUtility.UrlEncode(this._apiUsername)}&API={HttpUtility.UrlEncode(this._apiAccessKey)}";

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
            var requestData = Encoding.ASCII.GetBytes(dataParameterBuilder.ToString());
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestData, 0, requestData.Length);
                requestStream.Close();
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

                var requestData = Encoding.ASCII.GetBytes(parameterString);
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestData, 0, requestData.Length);
                    requestStream.Close();
                }
            }

            return request;
        }
    }
}

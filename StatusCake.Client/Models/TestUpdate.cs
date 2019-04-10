using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StatusCake.Client.Enumerators;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// StatusCake test details
    /// </summary>
    public class TestUpdate
    {
        /// <summary>
        /// Create a new TestUpdate with the default values
        /// </summary>
        public TestUpdate(string testName, string url, int checkRate, TestType TestType)
        {
            this.Initialize();

            // Set the initial values
            this.TestType = TestType;
            this.WebsiteName = testName;
            this.WebsiteURL = url;
            this.CheckRate = checkRate;
        }

        /// <summary>
        /// Create a new TestUpdate with the current data from an existing test
        /// </summary>
        public TestUpdate(Test test, string url, int checkRate)
        {
            this.Initialize();

            // Copy the values from an existing test
            this.TestID = test.TestID;
            this.TestType = test.TestType;
            this.WebsiteName = test.WebsiteName;
            this.WebsiteURL = url;
            this.CheckRate = checkRate;
        }

        /// <summary>
        /// Initialize the model with the default values
        /// </summary>
        private void Initialize()
        {
            this.TestType = null;
            this.WebsiteName = null;
            this.WebsiteURL = null;
            this.CheckRate = 300;
            this.TestID = null;
            this.Paused = null;
            this.ContactGroupID = null;
            this.Port = null;
            this.NodeLocations = null;
            this.Timeout = null;
            this.PingURL = null;
            this.Confirmations = null;
            this.Public = null;
            this.LogoImageUrl = null;
            this.DisplayBranding = null;
            this.WebsiteHost = null;
            this.VirusCheckEnabled = null;
            this.FindString = null;
            this.DoNotFind = null;
            this.TestWithRealBrowser = null;
            this.TriggerRate = null;
            this.Tags = null;
            this.StatusCodes = null;
        }

        /// <summary>
        /// The unique ID of the test
        /// </summary>
        public int? TestID { get; set; }

        /// <summary>
        /// True if the test is paused
        /// </summary>
        public bool? Paused { get; set; }

        /// <summary>
        /// The type of the test. Possible values: HTTP or TCP
        /// </summary>
        public TestType? TestType { get; set; }

        /// <summary>
        /// The name of the test
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// Contains the contact group or null if the group is not set
        /// </summary>
        public long? ContactGroupID { get; set; }

        /// <summary>
        /// The URL of the website that will be tested
        /// </summary>
        public string WebsiteURL { get; set; }

        /// <summary>
        /// The port where the website will be pinged
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// The comma separated list of locations where the test will be ran from
        /// </summary>
        public List<string> NodeLocations { get; set; }

        /// <summary>
        /// The timeout of the tests in seconds
        /// </summary>
        [Range(5, 100)]
        public int? Timeout { get; set; }

        /// <summary>
        /// The URL to ping if a site goes down.
        /// </summary>
        public string PingURL { get; set; }

        /// <summary>
        /// The number of confirmaion
        /// </summary>
        [Range(0, 10)]
        public int? Confirmations { get; set; }

        /// <summary>
        /// The check rate at which the website will be tested
        /// </summary>
        [Range(0, 24000)]
        public int CheckRate { get; set; }

        /// <summary>
        /// Set to true to enable public reporting for the test
        /// </summary>
        public bool? Public { get; set; }

        /// <summary>
        /// The Url to the logo image in the public reporting
        /// </summary>
        public string LogoImageUrl { get; set; }

        /// <summary>
        /// Set to true to enable public branding on the public reporting page
        /// </summary>
        public bool? DisplayBranding { get; set; }

        /// <summary>
        /// The website host
        /// </summary>
        public string WebsiteHost { get; set; }

        /// <summary>
        /// Set to true to enable virus checking on the website
        /// </summary>
        public bool? VirusCheckEnabled { get; set; }

        /// <summary>
        /// The string that the test should result should contain
        /// </summary>
        public string FindString { get; set; }

        /// <summary>
        /// Set to true to invert the FindString value
        /// </summary>
        public bool? DoNotFind { get; set; }

        /// <summary>
        /// Set to true ti test the website with a real browser
        /// </summary>
        public bool? TestWithRealBrowser { get; set; }

        /// <summary>
        /// How many minutes to wait before sending an event
        /// </summary>
        [Range(0, 60)]
        public int? TriggerRate { get; set; }

        /// <summary>
        /// The tags linked to this test
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// The list of status codes that should trigger the error
        /// </summary>
        public List<int> StatusCodes { get; set; }
    }
}

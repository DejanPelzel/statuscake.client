using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client
{
    /// <summary>
    /// A collection of the StatusCake API endpoint names
    /// </summary>
    public static class StatusCakeEndpoints
    {
        /// <summary>
        /// Tests endpoint
        /// </summary>
        public const string Tests = "Tests";

        /// <summary>
        /// Test details endpoint
        /// </summary>
        public const string TestDetails = "Tests/Details/";

        /// <summary>
        /// Auth endpoint
        /// </summary>
        public const string Auth = "Auth";

        /// <summary>
        /// Periods endpoint
        /// </summary>
        public const string Periods = "Tests/Periods";

        /// <summary>
        /// Alerts
        /// </summary>
        public const string Alerts = "Alerts";

        /// <summary>
        /// ContactGroups
        /// </summary>
        public const string ContactGroups = "ContactGroups";

        /// <summary>
        /// Checks
        /// </summary>
        public const string Checks = "Checks";
    }
}

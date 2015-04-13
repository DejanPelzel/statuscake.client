using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace StatusCake.Client.Auth
{
    /// <summary>
    /// A helper for loading the API auth configuration
    /// </summary>
    internal class StatusCakeAuth
    {
        public const string AccessKeyConfigKey = "StatusCake.Client.AccessKey";
        public const string AccessKeyConfigUsername = "StatusCake.Client.Username";

        /// <summary>
        /// Get the configured StatusCake API access key
        /// </summary>
        public static string DefaultAccessKey
        {
            get
            {
                return ConfigurationManager.AppSettings[AccessKeyConfigKey];
            }
        }

        /// <summary>
        /// Get the configured StatusCake API access username
        /// </summary>
        public static string DefaultUsername
        {
            get
            {
                return ConfigurationManager.AppSettings[AccessKeyConfigUsername];
            }
        }
    }
}

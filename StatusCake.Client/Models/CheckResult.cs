using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// Contains data from a check result
    /// </summary>
    public class CheckResult
    {
        /// <summary>
        /// The status code of the check result
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// The UNIX timestamp time of the check result
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Human readable timestamp. Converted to the timezone of the account
        /// </summary>
        public DateTime Human { get; set; }

        /// <summary>
        /// The location from where the check was performed from
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The response headers from the test taken
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// The execution time of the test in ms 
        /// </summary>
        public string Performance { get; set; }
    }
}

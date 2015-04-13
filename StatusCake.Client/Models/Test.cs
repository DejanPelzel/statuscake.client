using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// A StatusCake test
    /// </summary>
    public class Test
    {
        /// <summary>
        /// The unique ID of the test
        /// </summary>
        public int TestID { get; set; }

        /// <summary>
        /// True if the test is paused
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// The type of the test. Possible values: HTTP or TCP
        /// </summary>
        public string TestType { get; set; }

        /// <summary>
        /// The name of the test
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// Contains the contact group or null if the group is not set
        /// </summary>
        public string ContactGroup { get; set; }

        /// <summary>
        /// The contact ID the test is tied to
        /// </summary>
        public int ContactID { get; set; }

        /// <summary>
        /// Current status of the test. Possible values: Up, Down
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The uptime percentage for the last 7 days
        /// </summary>
        public double Uptime { get; set; }
    }
}

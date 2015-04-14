using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// Status cake alert log object
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// The date and time when the alert was sent in GMT.
        /// </summary>
        public DateTime Triggered { get; set; }

        /// <summary>
        /// The status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The linux timestamp
        /// </summary>
        public long Unix { get; set; }

        /// <summary>
        /// The status logged that triggered the alert
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The ID of the test that triggered the alert
        /// </summary>
        public long TestID { get; set; }
    }
}

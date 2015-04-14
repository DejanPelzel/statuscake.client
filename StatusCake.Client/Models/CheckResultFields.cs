using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// A list of field options when calling the get check results
    /// </summary>
    public class CheckResultFields
    {
        /// <summary>
        /// Includes the Status field
        /// </summary>
        public const string Status = "status";

        /// <summary>
        /// Includes the Location field
        /// </summary>
        public const string Location = "location";

        /// <summary>
        /// Includes the Time field
        /// </summary>
        public const string Time = "time";

        /// <summary>
        /// Includes the Headers field
        /// </summary>
        public const string Headers = "headers";

        /// <summary>
        /// Includes the Performance field
        /// </summary>
        public const string Performance = "performance";
    }
}

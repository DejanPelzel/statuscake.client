using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// Auth details class
    /// </summary>
    public class AuthDetails
    {
        /// <summary>
        /// The username of the authenticated user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The first name of the authenticated user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the authenticated user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The plan type of the authenticated account
        /// </summary>
        public string Plan { get; set; }

        /// <summary>
        /// The timezone of the authenticated user
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// The country code of the authenticated user
        /// </summary>
        public string CountryCode { get; set; }
    }
}

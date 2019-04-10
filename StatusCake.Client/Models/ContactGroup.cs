using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// A ContactGroup object with details about a status cake contact group
    /// </summary>
    public class ContactGroup
    {
        /// <summary>
        /// The name of the contact group
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string GroupName { get; set; }

        /// <summary>
        /// The emails in use by the group
        /// </summary>
        public List<string> Emails { get; set; }

        /// <summary>
        /// The list of mobile numbers in use by the group
        /// </summary>
        public List<string> Mobiles { get; set; }

        /// <summary>
        /// The Boxcar account linked to the contact group
        /// </summary>
        public string Boxcar { get; set; }

        /// <summary>
        /// The Pushover account linked to the contact group
        /// </summary>
        public string Pushover { get; set; }

        /// <summary>
        /// The ID of the contact ID
        /// </summary>
        public long ContactID { get; set; }

        /// <summary>
        /// The webhook ping URL
        /// </summary>
        [Url]
        public string PingURL { get; set; }

        /// <summary>
        /// 1 if the desktops alerts via AlertCake are enabled
        /// </summary>
        [Range(0, 1)]
        public int DesktopAlert { get; set; }
    }
}

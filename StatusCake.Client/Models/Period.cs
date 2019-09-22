using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StatusCake.Client.Models
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Period
    {        
        /// <summary>
        /// The start of the period
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// The end of the period. If this is 0000 00 00 it means the period is still ongoing.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// The status type
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public string Additional { get; set; }

        /// <summary>
        /// Period time in text
        /// </summary>
        [JsonProperty("Period")]
        public string PeriodText { get; set; }
    }
}

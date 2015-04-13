using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Models
{
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
        public string Up { get; set; }
    }
}

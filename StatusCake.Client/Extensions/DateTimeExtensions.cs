using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The unix greenwitchtime
        /// </summary>
        private static DateTime unixBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        /// <summary>
        /// Returns the UNIX timestamp representation of the current DateTime object
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnix(this DateTime dateTime)
        {
            return (long)(dateTime - unixBaseTime).TotalSeconds;
        }
    }
}

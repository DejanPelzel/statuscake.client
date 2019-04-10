using System;

namespace StatusCake.Client.Utils
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// The unix greenwitchtime
        /// </summary>
        private static readonly DateTime UnixBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        /// <summary>
        /// Get a DateTime object from a unix time 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnix(long unixTime)
        {
            return UnixBaseTime.AddSeconds(unixTime);
        }
    }
}

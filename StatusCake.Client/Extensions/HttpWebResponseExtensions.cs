using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace StatusCake.Client.Extensions
{
    /// <summary>
    /// A collection of extensions for the HttpWebRequest class
    /// </summary>
    internal static class HttpWebResponseExtensions
    {
        /// <summary>
        /// Read the response string of the 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static async Task<string> GetResponseStringAsync(this HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            var responseBuilder = new StringBuilder();

            // Read the response
            int bytesRead = 0;
            byte[] buffer = new byte[4096];
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }

            return responseBuilder.ToString();
        }
    }
}

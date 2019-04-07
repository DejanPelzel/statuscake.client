using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

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

            dynamic pageContent;
            using (BufferedStream buffer = new BufferedStream(stream))
            {
                using (StreamReader reader = new StreamReader(buffer))
                {
                    pageContent = await reader.ReadToEndAsync();
                }
            }

            return pageContent.ToString();
        }
    }
}

using System;
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
        /// <param name="response"></param>
        /// <returns></returns>
        internal static async Task<string> GetResponseStringAsync(this HttpWebResponse response)
        {
            var stream = response.GetResponseStream();

            dynamic pageContent;
            using (var buffer = new BufferedStream(stream ?? throw new InvalidOperationException()))
            {
                using (var reader = new StreamReader(buffer))
                {
                    pageContent = await reader.ReadToEndAsync();
                }
            }

            return pageContent.ToString();
        }
    }
}

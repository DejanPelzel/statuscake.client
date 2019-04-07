using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusCake.Client.Models;

namespace StatusCake.Client.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new StatusCakeClient();

            // First, check if the server is already registered
            var existingTests = client.GetTestsAsync().Result;

            foreach (var test in existingTests)
            {
                var details = client.GetTestDetailsAsync(test.TestID).Result;
                var alerts = client.GetAlertsAsync(test.TestID).Result;
                var periods = client.GetPeriodsAsync(test.TestID).Result;
                var uptimes = client.GetUptimesAsync(test.TestID).Result;
            }
            Console.ReadKey();
        }
    }
}

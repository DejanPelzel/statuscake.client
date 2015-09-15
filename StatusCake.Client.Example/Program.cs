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
            Console.ReadKey();
        }
    }
}

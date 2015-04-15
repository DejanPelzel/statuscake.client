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
            // A temporary example
            foreach (var test in StatusCakeClient.Instance.GetTestsAsync().Result)
            {
                Console.WriteLine(test.WebsiteName);
            }
            Console.ReadKey();
        }
    }
}

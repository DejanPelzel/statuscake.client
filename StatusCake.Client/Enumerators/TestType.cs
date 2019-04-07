using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCake.Client.Enumerators
{
    /// <summary>
    /// Available status values
    /// </summary>
    public enum TestType
    {
        /// <summary>
        /// HTTP tests are great for testing any standard website that you would access using your browser. If you don't know what test type to use this one is likely the best option.
        /// </summary>
        Http,

        /// <summary>
        /// HEAD tests are like HTTP tests except they do not load the body of the site and instead of a GET/POST request only via a HEAD type; not all servers support this.
        /// </summary>
        Head,

        /// <summary>
        /// TCP tests can ping most services, including FTP, game servers, and a huge range more. Though it can ping HTTP, SMTP etc the dedicated test types are better suited for this.
        /// </summary>
        Tcp,

        /// <summary>
        /// DNS tests ensure the system that directs visitors from your domain to your actual server is working correctly. If your DNS fails your users will not be able to access your site easily
        /// </summary>
        Dns,

        /// <summary>
        /// SMTP tests check if your email server can send out mail as required.
        /// </summary>
        Smtp,

        /// <summary>
        /// SSH tests use a secure shell to login to your (most likely) unix based server; this checks if it's responding and usable
        /// </summary>
        Ssh,

        /// <summary>
        /// PING tests send a ICMP ping to an IP. This can be a great way of testing routers or a basic way of testing a server is responding.
        /// </summary>
        Ping
    }
}

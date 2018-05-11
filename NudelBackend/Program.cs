using Nudel.Backend.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nudel.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8000);
            server.Start();
        }
    }
}

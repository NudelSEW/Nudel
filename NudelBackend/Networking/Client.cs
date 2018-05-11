using System;
using System.Net.Sockets;

namespace Nudel.Backend.Networking
{
    public class Client
    {
        public Socket Socket { get; set; }
        public String Username { get; set; }

        public Client()
        {
            Socket = null;
            Username = null;
        }

        public Client(Socket socket)
        {
            Socket = socket;
            Username = null;
        }

        public Client(Socket socket, String username)
        {
            Socket = socket;
            Username = username;
        }
    }
}

using Nudel.Backend.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Nudel.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8000);
            NudelService nudel = new NudelService();

            server.Log += (string data) =>
            {
                Console.WriteLine("Server: " + data);
            server.Accepted += (Socket clientSocket) =>
            {
                Console.WriteLine($"Client accepted: {((IPEndPoint)clientSocket.RemoteEndPoint).Address}");
            };
            server.Received += (string data, Socket clientSocket) =>
            {
                Console.WriteLine(((IPEndPoint)clientSocket.RemoteEndPoint).Address + " sent:");
                Console.WriteLine(data);
            };

            server.Start();
        }
    }
}

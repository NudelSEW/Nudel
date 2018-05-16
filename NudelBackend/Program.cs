using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using TcpServer = JustConnect.Tcp.Server;

namespace Nudel.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServer server = new TcpServer(8000);
            NudelService nudel = new NudelService();

            server.Log += (string data) =>
            {
                Console.WriteLine($"Log: {data}");
            };
            server.Accepted += (Socket clientSocket) =>
            {
                Console.WriteLine($"Client accepted: {((IPEndPoint)clientSocket.RemoteEndPoint).Address}");
            };
            server.Received += (string data, Socket clientSocket) =>
            {
                Console.WriteLine($"{((IPEndPoint)clientSocket.RemoteEndPoint).Address} sent:");
                Console.WriteLine(data);

            };

            server.Start();

            Console.ReadLine();
        }
    }
}

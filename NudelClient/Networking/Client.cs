using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Nudel.Client.Networking
{
    public delegate void LogHandler(string data);
    public delegate void ReceivedHandler(string data);

    public class Client
    {
        private const int BUFFER_SIZE = 2048;
        private static Socket socket;
        private static byte[] buffer = new byte[BUFFER_SIZE];

        public bool IsConnected { get; set; }
        public int Port { get; set; }

        public event LogHandler Log;
        public event ReceivedHandler Received;

        public Client() : this(3131) { }
        public Client(int port)
        {
            Port = port;
        }

        public void Connect(string ip)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddress = ip.ToLower() == "loopback" ? IPAddress.Loopback : IPAddress.Parse(ip);

            if (!IsConnected)
            {
                socket.Connect(ipAddress, Port);
                socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receive, socket);

                IsConnected = true;

                Log?.Invoke("Client connected");
            }
            else
            {
                Log?.Invoke("Client already connected");
            }
        }
        public void Disconnect()
        {
            socket.Close();

            IsConnected = false;

            Log?.Invoke("Client disconnected");
        }
        public void Receive(IAsyncResult AR)
        {
            Socket serverSocket = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = serverSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                serverSocket.Close();

                Log?.Invoke("Client disconnected");

                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string data = Encoding.ASCII.GetString(recBuf);

            Received?.Invoke(data);

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receive, socket);
        }
        public void Send(String data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            new Thread(() =>
            {
                socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }).Start();
        }
    }
}

using Nudel.Backend.Networking.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Nudel.Backend.Networking
{
    public delegate void LogHandler(string data);
    public delegate void ReceivedHandler(string data, Socket client);
    public delegate void AcceptedHandler(Socket client);

    public class Server
    {
        private const int BACKLOG = 10;
        private const int BUFFER_SIZE = 2048;

        public bool IsRunning { get; set; }

        public event LogHandler Log;
        public event ReceivedHandler Received;
        public event AcceptedHandler Accepted;

        private Socket socket;
        private List<Socket> clients;
        private int port;
        private byte[] buffer = new byte[BUFFER_SIZE];

        public Server() : this(3131) { }
        public Server(int port)
        {
            this.port = port;

            clients = new List<Client>();
        }

        public void Start()
        {
            if (!IsRunning)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(BACKLOG);
                socket.BeginAccept(Accept, null);

                IsRunning = true;

                Log?.Invoke("Server started");
            }
            else
            {
                Log?.Invoke("Server already running");
            }
        }
        public void Stop()
        {
            foreach (Socket client in clients)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }

            socket.Close();
            IsRunning = false;
        }
        private void Accept(IAsyncResult AR)
        {
            Socket clientSocket;

            try
            {
                clientSocket = socket.EndAccept(AR);
                clients.Add(clientSocket);

                Log?.Invoke(((IPEndPoint)clientSocket.RemoteEndPoint).Address + " connected");
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            Accepted?.Invoke(clientSocket);

            socket.BeginAccept(Accept, null);

            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receive, clientSocket);
        }
        private void Receive(IAsyncResult AR)
        {
            Socket clientSocket = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = clientSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                foreach (Socket client in clients)
                {
                    if (client == clientSocket)
                    {
                        Log?.Invoke(((IPEndPoint)client.RemoteEndPoint).Address + " disconnected");
                        clients.Remove(client);
                    }
                }
                clientSocket.Close();

                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string data = Encoding.ASCII.GetString(recBuf);

            Received?.Invoke(data, clientSocket);

            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receive, clientSocket);
        }
        public void Send(string data, Socket clientSocket)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        public void SendToAll(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            foreach (Socket client in clients)
            {
                client.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
        }
    }
}

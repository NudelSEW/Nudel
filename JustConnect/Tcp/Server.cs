using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JustConnect.Tcp
{
    
    public delegate void ServerLogHandler(string data);
    public delegate void ServerReceivedHandler(string data, Socket client);
    public delegate void ServerAcceptedHandler(Socket client);

    /// <summary>
    /// the basic information and functions about the server are defined here
    /// </summary>
    public class Server
    {
        private const int BACKLOG = 10;
        private const int BUFFER_SIZE = 2048;
        private Socket socket;
        private List<Socket> clients;
        private byte[] buffer = new byte[BUFFER_SIZE];

        public bool IsRunning { get; set; }
        public int Port { get; set; }

        public event ServerLogHandler Log;
        public event ServerReceivedHandler Received;
        public event ServerAcceptedHandler Accepted;

        /// <summary>
        /// setting the portnumber in the constructor
        /// </summary>
        public Server() : this(3131) { }

        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="port"> the chosen portnumber </param>
        public Server(int port)
        {
            Port = port;

            clients = new List<Socket>();
        }

        /// <summary>
        /// starting the server and informing the powershell
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(new IPEndPoint(IPAddress.Any, Port));
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

        /// <summary>
        /// stoping the server and closing the socket
        /// </summary>
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

        /// <summary>
        /// accepting a connection with a client
        /// </summary>
        /// <param name="AR"> represents the status of an asynchronous operation </param>
        private void Accept(IAsyncResult AR)
        {
            Socket clientSocket;

            try
            {
                clientSocket = socket.EndAccept(AR);
                clients.Add(clientSocket);

                Log?.Invoke(((IPEndPoint)clientSocket.RemoteEndPoint).Address + " connected");

                Accepted?.Invoke(clientSocket);

                socket.BeginAccept(Accept, null);

                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receive, clientSocket);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }

        /// <summary>
        /// receiving data from a accepted client application
        /// </summary>
        /// <param name="AR"> represents the status of an asynchronous operation </param>
        private void Receive(IAsyncResult AR)
        {
            Socket clientSocket = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = clientSocket.EndReceive(AR);

                byte[] recBuf = new byte[received];
                Array.Copy(buffer, recBuf, received);
                string data = Encoding.ASCII.GetString(recBuf);

                Received?.Invoke(data, clientSocket);

                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receive, clientSocket);
            }
            catch (SocketException)
            {
                Log?.Invoke(((IPEndPoint)clientSocket.RemoteEndPoint).Address + " disconnected");
                clients.Remove(clientSocket);
                clientSocket.Close();

                return;
            }
        }

        /// <summary>
        /// instantiating a new thread to send a message
        /// </summary>
        /// <param name="data">the message string </param>
        /// <param name="clientSocket">the receiving socket </param>
        public void Send(string data, Socket clientSocket)
        {
            new Thread(() =>
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }).Start();

        }

        /// <summary>
        /// broadcast message for every connected client
        /// </summary>
        /// <param name="data"> the broadcasting message </param>
        public void SendToAll(string data)
        {
            foreach (Socket client in clients)
            {
                Send(data, client);
            }
        }
    }
}

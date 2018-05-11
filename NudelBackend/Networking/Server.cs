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
    public delegate void ReceivedHandler(string data, string sender);

    public class Server
    {
        private const int BACKLOG = 10;
        private const int BUFFER_SIZE = 2048;

        public bool IsRunning { get; set; }

        public event LogHandler Log;
        public event ReceivedHandler Received;

        private Socket socket;
        private static List<Client> clients;
        private int port;
        private byte[] buffer = new byte[BUFFER_SIZE];

        public Server() : this(3131) { }
        public Server(int port)
        {
            this.port = port;

            users = new List<Client>();
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
            foreach (Client client in clients)
            {
                client.Socket.Shutdown(SocketShutdown.Both);
                client.Socket.Close();
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
                clients.Add(new Client(clientSocket));
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }
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
                foreach (Client user in users)
                {
                    if (user.Socket == clientSocket)
                    {
                        Log?.Invoke(user.Username + " disconnected");
                        users.Remove(user);
                    }
                }
                clientSocket.Close();

                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            if (text.Substring(0, 1) == "!")
            {
                ValidateCommand(text, clientSocket);
            }
            else
            {
                Client sourceUser = null;

                foreach (Client user in users)
                {
                    if (user.Socket == clientSocket)
                        sourceUser = user;
                }
                if (sourceUser == null)
                    throw new UserNotFoundException("User not found");

                SendMessageToOthers(text, sourceUser);
            }
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveData, clientSocket);
        }
        public void Send(String data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            foreach (Client user in users)
            {
                user.Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
        }
        public void SendMessageToOthers(String message, Client sourceUser)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            foreach (Client user in users)
            {
                if (user != sourceUser)
                {
                    user.Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                }
            }
        }
        public void SendCommand(String command, Socket clientSocket)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        public void ValidateCommand(String command, Socket userSocket)
        {
            if (command == "!exit")
            {
                foreach (Client user in users)
                {
                    if (user.Socket == userSocket)
                    {
                        users.Remove(new Client(userSocket, user.Username));
                    }
                }
                userSocket.Close();
            }
        }
    }
}

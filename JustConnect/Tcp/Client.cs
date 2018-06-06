﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JustConnect.Tcp
{
    public delegate void ClientLogHandler(string data);
    public delegate void ClientReceivedHandler(string data);

    public class Client
    {
        private const int BUFFER_SIZE = 2048;
        private Socket socket;
        private byte[] buffer = new byte[BUFFER_SIZE];

        public bool IsConnected { get; set; }
        public int Port { get; set; }

        public event ClientLogHandler Log;
        public event ClientReceivedHandler Received;

        public Client() : this(3131) { }
        public Client(int port)
        {
            Port = port;
        }

        public void Connect(IPAddress ip)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (!IsConnected)
            {
                socket.Connect(ip, Port);
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
            socket.Disconnect(false);
            socket.Close();

            IsConnected = false;

            Log?.Invoke("Client disconnected");
        }
        private void Receive(IAsyncResult AR)
        {
            Socket serverSocket = (Socket)AR.AsyncState;

            try
            {
                int received = serverSocket.EndReceive(AR);

                byte[] recBuf = new byte[received];
                Array.Copy(buffer, recBuf, received);
                string data = Encoding.ASCII.GetString(recBuf);

                Received?.Invoke(data);

                socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receive, socket);
            }
            catch (Exception)
            {
                socket = null;
                IsConnected = false;

                Log?.Invoke("Client disconnected");

                return;
            }
        }
        public void Send(String data)
        {
            new Thread(() =>
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }).Start();
        }
    }
}

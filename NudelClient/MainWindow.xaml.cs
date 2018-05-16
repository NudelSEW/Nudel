using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Windows;

using TcpClient = JustConnect.Tcp.Client;

namespace Nudel.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TcpClient client = new TcpClient(8000);

            client.Log += (string data) =>
            {
                Console.WriteLine($"Log: {data}");
            };
            client.Received += (string data) =>
            {
                Console.WriteLine("Received:");
                Console.WriteLine(data);
            };

            client.Connect(IPAddress.Loopback);

            JObject json = JObject.Parse(@"{
                type: 'test',
                message: 'hello world'
            }");

            client.Send(json.ToString());
        }
    }
}

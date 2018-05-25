using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nudel.Networking.Requests;
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

            TcpClient client = new TcpClient(8181);

            client.Received += (string data) =>
            {
                Console.WriteLine("Received:");
                Console.WriteLine(data);
            };

            client.Connect(IPAddress.Loopback);

            RegisterRequest request = new RegisterRequest
            {
                Username = "chrispypb",
                Email = "christoph.paderbarosch@gmail.com",
                Password = "hallo123",
                FirstName = "Christoph",
                LastName = "Pader-Barosch"
            };

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            client.Send(JsonConvert.SerializeObject(request, jsonSettings));
        }
    }
}

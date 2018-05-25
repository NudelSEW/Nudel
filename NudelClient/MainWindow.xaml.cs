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
            NetworkListener.Start();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NetworkListener.Stop();
        }
    }
}

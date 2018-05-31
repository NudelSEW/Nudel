using Nudel.Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
            DataContext = new MainViewModel();
            NetworkListener.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NetworkListener.Stop();
        }
    }
}

using Nudel.Client.ViewModels;
using System;
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

#if DEBUG
            NetworkListener.Log += (string data) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    log.AppendText(data + "\n");
                }));
            };
#else
            logSection.Visibility = Visibility.Collapsed;
#endif

            NetworkListener.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NetworkListener.Stop();
        }

        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            log.ScrollToEnd();
        }


        public void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}

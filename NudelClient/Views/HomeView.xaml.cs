using Nudel.BusinessObjects;
using Nudel.Client.Model;
using Nudel.Client.ViewModels;
using Nudel.Networking;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Nudel.Client.Views
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public MainWindow MainWindow { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public HomeView()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }

        private void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow = (MainWindow)Window.GetWindow(this);
            MainViewModel = (MainViewModel)MainWindow.DataContext;

            MainWindow.Width = 1200;
            MainWindow.Height = 800;
            MainWindow.CenterWindowOnScreen();

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                Request request = new Request
                {
                    Type = RequestResponseType.FindCurrentUser,
                    SessionToken = MainModel.SessionToken
                };
                NetworkListener.SendRequest(request);
            }));

            ModelChangedHandler handler = null;
            handler = (string fieldName, Object field) =>
            {
                if (fieldName == "User")
                {
                    User user = MainModel.User;

                    ((HomeViewModel)DataContext).DisplayName = $"{user.FirstName} {user.LastName} ({user.Username})";
                }

                MainModel.ModelChanged -= handler;
            };

            MainModel.ModelChanged += handler;
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            Request request = new Request
            {
                Type = RequestResponseType.Logout,
                SessionToken = MainModel.SessionToken
            };

            MainModel.User = null;
            MainModel.SessionToken = null;

            NetworkListener.SendRequest(request);

            MainViewModel.CurrentView = new LoginView();
        }
    }
}

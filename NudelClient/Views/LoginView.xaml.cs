using Nudel.BusinessObjects;
using Nudel.Client.Model;
using Nudel.Client.ViewModels;
using Nudel.Networking;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nudel.Client.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public MainViewModel MainViewModel { get; set; }

        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel = (MainViewModel)Window.GetWindow(this).DataContext;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            Request request = new Request
            {
                Type = RequestResponseType.Login,
                User = new User
                {
                    Username = usernameOrEmail.Text,
                    Email = usernameOrEmail.Text,
                    Password = password.Password
                }
            };

            ModelChangedHandler handler = null;
            handler = (string fieldName, Object field) =>
            {
                if (fieldName == "SessionToken")
                {
                    MainViewModel.CurrentView = new HomeView();
                }

                MainModel.ModelChanged -= handler;
            };

            MainModel.ModelChanged += handler;

            NetworkListener.SendRequest(request);
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            MainViewModel.CurrentView = new RegisterView();
        }
    }
}

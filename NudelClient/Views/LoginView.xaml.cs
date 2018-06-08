using Nudel.Client.ViewModels;
using Nudel.Networking.Requests;
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
            LoginRequest request = new LoginRequest
            {
                UsernameOrEmail = usernameOrEmail.Text,
                Password = password.Password
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
    }
}

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
    public partial class RegisterView : UserControl
    {
        public MainViewModel MainViewModel { get; set; }

        public RegisterView()
        {
            InitializeComponent();
        }

        private void RegisterView_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel = (MainViewModel)Window.GetWindow(this).DataContext;
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            Request request = new Request
            {
                Type = RequestResponseType.Register,
                User = new User
                {
                    Username = username.Text,
                    Email = email.Text,
                    Password = password.Password,
                    FirstName = firstName.Text,
                    LastName = lastName.Text
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

        private void Login(object sender, RoutedEventArgs e)
        {
            MainViewModel.CurrentView = new LoginView();
        }
    }
}

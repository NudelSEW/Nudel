using Nudel.Client.Model;
using Nudel.Client.ViewModels;
using Nudel.Networking.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            RegisterRequest request = new RegisterRequest
            {
                Username = username.Text,
                Email = email.Text,
                Password = password.Password,
                FirstName = firstName.Text,
                LastName = lastName.Text
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

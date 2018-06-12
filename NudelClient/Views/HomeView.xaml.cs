using Nudel.BusinessObjects;
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
using System.Windows.Threading;

namespace Nudel.Client.Views
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }

        private void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => {
                GetUserRequest request = new GetUserRequest(MainModel.SessionToken);
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


        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            NetworkListener.Stop();
            Environment.Exit(0);
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;

        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

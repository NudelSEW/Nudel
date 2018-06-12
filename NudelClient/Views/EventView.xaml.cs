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

namespace Nudel.Client.Views
{
    /// <summary>
    /// Interaction logic for EventView.xaml
    /// </summary>
    public partial class EventView : UserControl
    {
        public EventView()
        {
            InitializeComponent();
        }

        private void EventView_Loaded(object sender, RoutedEventArgs e)
        {
            ModelChangedHandler handler = null;
            handler = (string fieldName, Object field) =>
            {
                if (fieldName == "User")
                {
                    User user = MainModel.User;
                    
                    if (user.JoinedEvents != null)
                    {
                        foreach (Event @event in MainModel.User.JoinedEvents)
                        {
                            mainGrid.Children.Add(new EventCardView
                            {
                                DataContext = new EventCardViewModel
                                {
                                    Title = @event.Title,
                                    Description = @event.Description
                                }
                            });
                        }
                    }
                    else
                    {
                        mainGrid.Children.Add(new Label
                        {
                            Content = "There are no events yet..."
                        });
                    }
                }

                MainModel.ModelChanged -= handler;
            };

            MainModel.ModelChanged += handler;
        }
    }
}

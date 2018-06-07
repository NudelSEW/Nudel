using Nudel.Client.ViewModels;
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
            EventCardView card = new EventCardView
            {
                DataContext = new EventCardViewModel("Test Event", "This is a test event")
            };

            mainGrid.Children.Add(card);
        }
    }
}

using Nudel.Client.Views;
using System.ComponentModel;
using System.Windows.Controls;

namespace Nudel.Client.ViewModels
{
    /// <summary>
    /// contains the attributes of the homeView and invokes the new attributes if changed by an user
    /// </summary>
    public class HomeViewModel : INotifyPropertyChanged
    {
        private UserControl currentView;

        /// <summary>
        /// setter and getter methods
        /// </summary>
        public UserControl CurrentView {
            get
            {
                return currentView;
            }
            set
            {
                currentView = value;
                NotifyPropertyChanged("CurrentView");
            }
        }

        private string displayName;

        /// <summary>
        /// setter and getter methods
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
                NotifyPropertyChanged("DisplayName");
            }
        }

        public HomeViewModel()
        {
            CurrentView = new EventView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// invokes the new property string if it was changed
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

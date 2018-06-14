using Nudel.BusinessObjects;
using Nudel.Client.Views;
using System.ComponentModel;
using System.Windows.Controls;

namespace Nudel.Client.ViewModels
{
    /// <summary>
    /// contains the current View and changes the value of the changed property if necessary
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private UserControl currentView;
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

        /// <summary>
        /// default constructor
        /// </summary>
        public MainViewModel()
        {
            CurrentView = new LoginView();
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

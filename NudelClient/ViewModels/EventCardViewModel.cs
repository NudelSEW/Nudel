using Nudel.Client.Views;
using System.ComponentModel;
using System.Windows.Controls;

namespace Nudel.Client.ViewModels
{
    /// <summary>
    /// defining the attributes of the eventCard GUI
    /// </summary>
    public class EventCardViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public EventCardViewModel() { }

        /// <summary>
        /// constructor setting the title and description
        /// </summary>
        /// <param name="title"> the changed title of the event </param>
        /// <param name="description"> the new description of the event </param>
        public EventCardViewModel(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///  invoking the changed property string
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

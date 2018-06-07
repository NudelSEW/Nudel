using Nudel.Client.Views;
using System.ComponentModel;
using System.Windows.Controls;

namespace Nudel.Client.ViewModels
{
    public class EventCardViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public EventCardViewModel() { }

        public EventCardViewModel(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

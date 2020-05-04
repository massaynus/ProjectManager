using System.ComponentModel;

namespace Windows_CLient.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };
        public void OnPropertyChanged(string PropertyName) => PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
    }
}

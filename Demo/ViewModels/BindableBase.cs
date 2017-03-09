using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Demo.ViewModels
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (value?.Equals(storage) == true) return;

            storage = value;

            OnPropertyChanged(propertyName);
        }
    }
}

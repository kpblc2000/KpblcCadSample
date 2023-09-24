using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KpblcCadCore.ViewModels.Base
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public virtual bool Set<T> (ref T Field, T value, [CallerMemberName] string MemberName = null)
        {
            if (Equals(Field, value))
            {
                return false;
            }

            Field = value;
            OnPropertyChanged(MemberName);
            return true;
        }

    }
}

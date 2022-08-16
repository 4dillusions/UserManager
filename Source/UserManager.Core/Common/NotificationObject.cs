using System.ComponentModel;

namespace UserManager.Core.Common
{
    public abstract class NotificationObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)fff
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

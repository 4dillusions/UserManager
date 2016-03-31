using UserManager.Core.Common;

namespace UserManager.Model
{
    public class UserFilter : NotificationObject
    {
        string addressCity, textInAll;

        public UserFilter()
        {
            AddressCity = new AddressCity().CityName;
            TextInAll = string.Empty;
        }

        public string AddressCity 
        {
            get { return addressCity; }

            set
            {
                NotifyPropertyChanging("AddressCity");
                addressCity = value;
                NotifyPropertyChanged("AddressCity");
            }
        }

        public string TextInAll 
        {
            get { return textInAll; }

            set
            {
                NotifyPropertyChanging("TextInAll");
                textInAll = value;
                NotifyPropertyChanged("TextInAll");
            }
        }
    }
}

using System.ComponentModel;
using UserManager.Core.Common;

namespace UserManager.Model
{
    public class AddressCity : NotificationObject
    {
        public static readonly string DefaultName = "ALL";
        string cityName;

        public AddressCity()
        {
            CityName = DefaultName;
        }

        public string CityName 
        {
            get { return cityName; }
            
            set
            {
                NotifyPropertyChanging("CityName");
                cityName = value;
                NotifyPropertyChanged("CityName");
            }
        }

        public override string ToString()
        {
            return CityName;
        }
    }
}

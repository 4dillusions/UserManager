using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UserManager.Core.Common;
using UserManager.Core.Data;

namespace UserManager.Model
{
    public class AddressCityList
    {
        ObservableCollection<AddressCity> cities;

        public ObservableCollection<AddressCity> Cities
        {
            get { return cities; }
            set { cities = value; }
        }

        public AddressCityList()
        {
            Cities = new ObservableCollection<AddressCity>();
            
            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
            {
                IDataManager<User> dataManager = new XmlDataManager<User>();
                var data = dataManager.Load(Constants.DataFilePath + Constants.XmlDataFileName);

                var citiesList = data.Select(u => u.AddressCity).Distinct().ToList();
                cities.Add(new AddressCity());
                foreach (var item in citiesList)
                {
                    cities.Add(new AddressCity
                    {
                         CityName = item,
                    });
                }
            }
            else
                throw new FileNotFoundException();
        }

    }
}

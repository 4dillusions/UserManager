/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Data;
using System.Collections.ObjectModel;
using System.IO;

namespace App4di.Dotnet.UserManager.Model.Entities;

public class AddressCityList
{
    private ObservableCollection<AddressCity> cities = [];

    public ObservableCollection<AddressCity> Cities
    {
        get { return cities; }
        set { cities = value; }
    }

    public AddressCityList()
    {
        Cities = new ObservableCollection<AddressCity>();

        if (File.Exists(Constants.XmlDataFilePath))
        {
            IDataManager<User> dataManager = new XmlDataManager<User>();
            var data = dataManager.Load(Constants.XmlDataFilePath);

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
        {
            throw new FileNotFoundException();
        }
    }
}

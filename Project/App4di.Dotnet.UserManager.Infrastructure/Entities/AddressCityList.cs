/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using System.Collections.ObjectModel;

namespace App4di.Dotnet.UserManager.Infrastructure.Entities;

public class AddressCityList
{
    private ObservableCollection<AddressCity> cities = [];

    public ObservableCollection<AddressCity> Cities
    {
        get { return cities; }
        set { cities = value; }
    }

    public AddressCityList()
        : this(new XmlAddressCityRepository(new XmlUserRepository()))
    {
    }

    public AddressCityList(IAddressCityRepository addressCityRepository)
    {
        ArgumentNullException.ThrowIfNull(addressCityRepository);

        Cities = new ObservableCollection<AddressCity>();
        cities.Add(new AddressCity());
        foreach (var city in addressCityRepository.LoadAddressCities())
            cities.Add(city);
    }
}

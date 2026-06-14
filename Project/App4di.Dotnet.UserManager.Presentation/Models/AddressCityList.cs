/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Users;
using System.Collections.ObjectModel;

namespace App4di.Dotnet.UserManager.Presentation.Models;

public class AddressCityList
{
    private ObservableCollection<AddressCity> cities = [];

    public ObservableCollection<AddressCity> Cities
    {
        get { return cities; }
        set { cities = value; }
    }

    public AddressCityList(IUserQueryService userQueryService)
    {
        ArgumentNullException.ThrowIfNull(userQueryService);

        Cities = new ObservableCollection<AddressCity>
        {
            new()
        };
        foreach (var cityName in userQueryService.GetAddressCities())
            Cities.Add(new AddressCity { CityName = cityName });
    }
}

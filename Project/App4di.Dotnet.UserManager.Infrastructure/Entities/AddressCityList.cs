/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Users;
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
        : this(CreateQueryService())
    {
    }

    public AddressCityList(IAddressCityRepository addressCityRepository)
        : this(new UserQueryService(new XmlUserRepository(), addressCityRepository))
    {
    }

    public AddressCityList(IUserQueryService userQueryService)
    {
        ArgumentNullException.ThrowIfNull(userQueryService);

        Cities = new ObservableCollection<AddressCity>(userQueryService.GetAddressCities());
    }

    private static IUserQueryService CreateQueryService()
    {
        IUserRepository userRepository = new XmlUserRepository();
        return new UserQueryService(userRepository, new XmlAddressCityRepository(userRepository));
    }
}

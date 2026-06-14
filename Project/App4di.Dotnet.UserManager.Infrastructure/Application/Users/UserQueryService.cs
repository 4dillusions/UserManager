/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;

namespace App4di.Dotnet.UserManager.Infrastructure.Application.Users;

public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository userRepository;
    private readonly IAddressCityRepository addressCityRepository;

    public UserQueryService(IUserRepository userRepository, IAddressCityRepository addressCityRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.addressCityRepository = addressCityRepository ?? throw new ArgumentNullException(nameof(addressCityRepository));
    }

    public List<User> GetUsers(UserFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var users = userRepository.LoadUsers();

        if (filter.AddressCity == AddressCity.DefaultName)
            return users.Where(user => user.ToString().Contains(filter.TextInAll)).ToList();

        return users.Where(user => user.AddressCity == filter.AddressCity &&
            user.ToString().Contains(filter.TextInAll)).ToList();
    }

    public List<AddressCity> GetAddressCities()
    {
        var cities = new List<AddressCity> { new() };
        cities.AddRange(addressCityRepository.LoadAddressCities());
        return cities;
    }
}

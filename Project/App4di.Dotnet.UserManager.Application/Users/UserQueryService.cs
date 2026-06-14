/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Domain;

namespace App4di.Dotnet.UserManager.Application.Users;

public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository userRepository;
    private readonly IAddressCityRepository addressCityRepository;

    public UserQueryService(IUserRepository userRepository, IAddressCityRepository addressCityRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.addressCityRepository = addressCityRepository ?? throw new ArgumentNullException(nameof(addressCityRepository));
    }

    public List<UserData> GetUsers(UserQueryCriteria filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var users = userRepository.LoadUsers();

        if (filter.AddressCity == UserQueryCriteria.AllAddressCities)
            return users.Where(user => ToSearchText(user).Contains(filter.TextInAll)).ToList();

        return users.Where(user => user.AddressCity == filter.AddressCity &&
            ToSearchText(user).Contains(filter.TextInAll)).ToList();
    }

    public List<string> GetAddressCities()
    {
        return addressCityRepository.LoadAddressCities();
    }

    private static string ToSearchText(UserData user)
    {
        return user.UserId + user.LoginName + user.Password + user.FirstName + user.Surname +
            user.BirthDate + user.BirthPlace + user.AddressCity;
    }
}

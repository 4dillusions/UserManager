/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Domain;
using System.Globalization;

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

    public IReadOnlyList<UserData> GetUsers(UserQueryCriteria filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var users = userRepository.LoadUsers();

        if (filter.AddressCity == UserQueryCriteria.AllAddressCities)
            return users.Where(user => MatchesSearch(user, filter.TextInAll)).ToList();

        return users.Where(user => user.AddressCity == filter.AddressCity &&
            MatchesSearch(user, filter.TextInAll)).ToList();
    }

    public IReadOnlyList<string> GetAddressCities()
    {
        return addressCityRepository.LoadAddressCities();
    }

    private static bool MatchesSearch(UserData user, string searchText)
    {
        return GetSearchableFields(user).Any(field => field.Contains(searchText, StringComparison.Ordinal));
    }

    private static IEnumerable<string> GetSearchableFields(UserData user)
    {
        yield return user.UserId.ToString();
        yield return user.LoginName ?? string.Empty;
        yield return user.FirstName ?? string.Empty;
        yield return user.Surname ?? string.Empty;
        yield return user.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        yield return user.BirthPlace ?? string.Empty;
        yield return user.AddressCity ?? string.Empty;
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;

namespace App4di.Dotnet.UserManager.Infrastructure.Repositories;

public class XmlAddressCityRepository : IAddressCityRepository
{
    private readonly IUserRepository userRepository;

    public XmlAddressCityRepository(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public IReadOnlyList<string> LoadAddressCities()
    {
        return userRepository.LoadUsers()
            .Select(user => user.AddressCity)
            .Distinct()
            .ToList();
    }
}

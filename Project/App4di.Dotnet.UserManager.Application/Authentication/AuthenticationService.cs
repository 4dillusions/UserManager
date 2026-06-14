/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;

namespace App4di.Dotnet.UserManager.Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public bool Authenticate(string loginName, string password)
    {
        return userRepository.LoadUsers()
            .Any(user => user.LoginName == loginName && user.Password == password);
    }
}

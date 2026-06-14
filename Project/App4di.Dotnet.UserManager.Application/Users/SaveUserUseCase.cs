/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;

namespace App4di.Dotnet.UserManager.Application.Users;

public class SaveUserUseCase : ISaveUserUseCase
{
    private readonly IUserRepository userRepository;

    public SaveUserUseCase(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public void Execute(IUserSaveSession userSaveSession)
    {
        ArgumentNullException.ThrowIfNull(userSaveSession);

        var users = userSaveSession.CreateSaveSnapshot();
        userRepository.SaveUsers(users);
        userSaveSession.CompleteSave();
    }
}

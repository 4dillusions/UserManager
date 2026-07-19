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

        var userToSave = userSaveSession.UserToSave;
        if (!BirthDateRules.IsValid(userToSave.BirthDate))
        {
            throw new InvalidOperationException(
                $"Birth date must be between {BirthDateRules.MinimumBirthDate:d} and {BirthDateRules.MaximumBirthDate:d}.");
        }

        var users = userSaveSession.CreateSaveSnapshot();
        if (users.Any(user =>
            user.UserId != userToSave.UserId &&
            string.Equals(user.LoginName, userToSave.LoginName, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Login name '{userToSave.LoginName}' already exists.");
        }

        userRepository.SaveUsers(users);
        userSaveSession.CompleteSave();
    }
}

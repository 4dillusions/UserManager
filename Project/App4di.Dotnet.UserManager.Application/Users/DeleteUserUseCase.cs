/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;

namespace App4di.Dotnet.UserManager.Application.Users;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserRepository userRepository;

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public bool Execute(int userId)
    {
        var users = userRepository.LoadUsers().ToList();
        var userIndex = users.FindIndex(user => user.UserId == userId);

        if (userIndex < 0)
            return false;

        if (users.Count == 1)
            throw new InvalidOperationException("The last user cannot be deleted.");

        users.RemoveAt(userIndex);
        userRepository.SaveUsers(users);
        return true;
    }
}

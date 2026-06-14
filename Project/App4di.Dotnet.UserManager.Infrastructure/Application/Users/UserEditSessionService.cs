/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Mapping;

namespace App4di.Dotnet.UserManager.Infrastructure.Application.Users;

public class UserEditSessionService : IUserEditSessionService
{
    private List<User>? users;
    private User? selectedUser;
    private int editingUserId;

    public User? EditingUser { get; private set; }

    public void BeginEdit(User user, List<User> users)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(users);

        this.users = users;
        selectedUser = user;
        editingUserId = user.UserId;
        EditingUser = UserMapper.Copy(user);
    }

    public List<User> Commit()
    {
        if (EditingUser == null || users == null)
            throw new InvalidOperationException("No user edit session is active.");

        var originalUser = users.FirstOrDefault(user => user.UserId == editingUserId)
            ?? throw new InvalidOperationException($"User with ID '{editingUserId}' was not found.");

        UserMapper.Copy(EditingUser, originalUser);
        if (!ReferenceEquals(selectedUser, originalUser) && selectedUser != null)
            UserMapper.Copy(EditingUser, selectedUser);

        var committedUsers = users;
        Clear();
        return committedUsers;
    }

    public void Cancel()
    {
        Clear();
    }

    private void Clear()
    {
        EditingUser = null;
        users = null;
        selectedUser = null;
        editingUserId = default;
    }

}

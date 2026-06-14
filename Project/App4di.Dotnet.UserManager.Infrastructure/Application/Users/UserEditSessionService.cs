/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Entities;

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
        EditingUser = CopyUser(user);
    }

    public List<User> Commit()
    {
        if (EditingUser == null || users == null)
            throw new InvalidOperationException("No user edit session is active.");

        var originalUser = users.FirstOrDefault(user => user.UserId == editingUserId)
            ?? throw new InvalidOperationException($"User with ID '{editingUserId}' was not found.");

        CopyValues(EditingUser, originalUser);
        if (!ReferenceEquals(selectedUser, originalUser) && selectedUser != null)
            CopyValues(EditingUser, selectedUser);

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

    private static User CopyUser(User source)
    {
        var copy = new User();
        CopyValues(source, copy);
        return copy;
    }

    private static void CopyValues(User source, User destination)
    {
        destination.UserId = source.UserId;
        destination.LoginName = source.LoginName;
        destination.Password = source.Password;
        destination.FirstName = source.FirstName;
        destination.Surname = source.Surname;
        destination.BirthDate = source.BirthDate;
        destination.BirthPlace = source.BirthPlace;
        destination.AddressCity = source.AddressCity;
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Mapping;
using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Domain;

namespace App4di.Dotnet.UserManager.Presentation.Users;

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

    public IReadOnlyList<UserData> CreateSaveSnapshot()
    {
        if (EditingUser == null || users == null)
            throw new InvalidOperationException("No user edit session is active.");

        var originalUserIndex = users.FindIndex(user => user.UserId == editingUserId);
        if (originalUserIndex < 0)
            throw new InvalidOperationException($"User with ID '{editingUserId}' was not found.");

        var snapshot = users.Select(UserMapper.ToUserData).ToList();
        snapshot[originalUserIndex] = UserMapper.ToUserData(EditingUser);
        return snapshot;
    }

    public void CompleteSave()
    {
        if (EditingUser == null || users == null)
            throw new InvalidOperationException("No user edit session is active.");

        var originalUser = users.FirstOrDefault(user => user.UserId == editingUserId)
            ?? throw new InvalidOperationException($"User with ID '{editingUserId}' was not found.");

        UserMapper.Copy(EditingUser, originalUser);
        if (!ReferenceEquals(selectedUser, originalUser) && selectedUser != null)
            UserMapper.Copy(EditingUser, selectedUser);

        Clear();
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

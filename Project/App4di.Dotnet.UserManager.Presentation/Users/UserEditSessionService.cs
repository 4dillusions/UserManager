/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Mapping;
using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Domain;
using System.ComponentModel;

namespace App4di.Dotnet.UserManager.Presentation.Users;

public class UserEditSessionService : IUserEditSessionService
{
    private IReadOnlyList<User>? users;
    private User? selectedUser;
    private UserData? originalUser;
    private int editingUserId;

    public User? EditingUser { get; private set; }
    public UserData UserToSave => EditingUser != null
        ? UserMapper.ToUserData(EditingUser)
        : throw new InvalidOperationException("No user edit session is active.");
    public UserEditMode Mode { get; private set; } = UserEditMode.Edit;
    public bool HasChanges => EditingUser != null &&
        (Mode == UserEditMode.Add || originalUser != null && !HasSameValues(EditingUser, originalUser));
    public event EventHandler? EditStateChanged;
    public event EventHandler? SaveCompleted;

    public void BeginEdit(User user, IReadOnlyList<User> users)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(users);

        StartSession(users, UserEditMode.Edit);
        selectedUser = user;
        originalUser = UserMapper.ToUserData(user);
        editingUserId = user.UserId;
        SetEditingUser(UserMapper.Copy(user));
    }

    public void BeginAdd(IReadOnlyList<User> users)
    {
        ArgumentNullException.ThrowIfNull(users);

        var maximumUserId = users.Count == 0 ? 0 : users.Max(user => user.UserId);
        if (maximumUserId == int.MaxValue)
            throw new InvalidOperationException("No user ID is available for a new user.");

        var userId = maximumUserId + 1;
        StartSession(users, UserEditMode.Add);
        SetEditingUser(new User { UserId = userId });
    }

    public IReadOnlyList<UserData> CreateSaveSnapshot()
    {
        if (EditingUser == null || users == null)
            throw new InvalidOperationException("No user edit session is active.");

        if (Mode == UserEditMode.Add)
        {
            var addSnapshot = users.Select(UserMapper.ToUserData).ToList();
            addSnapshot.Add(UserMapper.ToUserData(EditingUser));
            return addSnapshot;
        }

        var originalUserIndex = FindUserIndex(users, editingUserId);
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

        if (Mode == UserEditMode.Add)
        {
            Clear();
            SaveCompleted?.Invoke(this, EventArgs.Empty);
            return;
        }

        var originalUser = users.FirstOrDefault(user => user.UserId == editingUserId)
            ?? throw new InvalidOperationException($"User with ID '{editingUserId}' was not found.");

        UserMapper.Copy(EditingUser, originalUser);
        if (!ReferenceEquals(selectedUser, originalUser) && selectedUser != null)
            UserMapper.Copy(EditingUser, selectedUser);

        Clear();
        SaveCompleted?.Invoke(this, EventArgs.Empty);
    }

    public void Cancel()
    {
        Clear();
    }

    private void Clear()
    {
        if (EditingUser != null)
            EditingUser.PropertyChanged -= EditingUserPropertyChanged;

        EditingUser = null;
        users = null;
        selectedUser = null;
        originalUser = null;
        editingUserId = default;
        EditStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void StartSession(IReadOnlyList<User> users, UserEditMode mode)
    {
        Clear();
        this.users = users;
        Mode = mode;
    }

    private void SetEditingUser(User user)
    {
        EditingUser = user;
        EditingUser.PropertyChanged += EditingUserPropertyChanged;
        EditStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void EditingUserPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        EditStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private static int FindUserIndex(IReadOnlyList<User> users, int userId)
    {
        for (var index = 0; index < users.Count; index++)
        {
            if (users[index].UserId == userId)
                return index;
        }

        return -1;
    }

    private static bool HasSameValues(User user, UserData original)
    {
        return user.UserId == original.UserId &&
            user.LoginName == original.LoginName &&
            user.Password == original.Password &&
            user.FirstName == original.FirstName &&
            user.Surname == original.Surname &&
            user.BirthDate == original.BirthDate &&
            user.BirthPlace == original.BirthPlace &&
            user.AddressCity == original.AddressCity;
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Presentation.Models;

namespace App4di.Dotnet.UserManager.Presentation.Users;

public interface IUserEditSessionService : IUserSaveSession
{
    User? EditingUser { get; }
    bool HasChanges { get; }
    event EventHandler? EditStateChanged;
    void BeginEdit(User user, IReadOnlyList<User> users);
    void Cancel();
}

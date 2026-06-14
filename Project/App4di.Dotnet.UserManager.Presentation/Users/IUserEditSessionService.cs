/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Models;

namespace App4di.Dotnet.UserManager.Presentation.Users;

public interface IUserEditSessionService
{
    User? EditingUser { get; }
    void BeginEdit(User user, List<User> users);
    List<User> Commit();
    void Cancel();
}

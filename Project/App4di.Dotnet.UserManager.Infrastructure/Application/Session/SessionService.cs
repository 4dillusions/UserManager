/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Entities;

namespace App4di.Dotnet.UserManager.Infrastructure.Application.Session;

public class SessionService : ISessionService
{
    public User? CurrentUser { get; set; }
    public List<User>? CurrentUsers { get; set; }

    public void Clear()
    {
        CurrentUser = null;
        CurrentUsers = null;
    }
}

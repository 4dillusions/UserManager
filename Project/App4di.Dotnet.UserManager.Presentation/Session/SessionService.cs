/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Models;

namespace App4di.Dotnet.UserManager.Presentation.Session;

public class SessionService : ISessionService
{
    public User? CurrentUser { get; set; }

    public void Clear()
    {
        CurrentUser = null;
    }
}

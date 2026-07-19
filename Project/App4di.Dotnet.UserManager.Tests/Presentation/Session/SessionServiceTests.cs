/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Presentation.Session;

namespace App4di.Dotnet.UserManager.Tests.Presentation.Session;

[TestClass]
public class SessionServiceTests
{
    [TestMethod]
    public void SessionStoresSelectedUser()
    {
        var sessionService = new SessionService();
        var user = new User { UserId = 1 };

        sessionService.SelectedUser = user;

        Assert.AreSame(user, sessionService.SelectedUser);
    }

    [TestMethod]
    public void SessionReplacesAndClearsCurrentState()
    {
        var sessionService = new SessionService
        {
            SelectedUser = new User { UserId = 1 }
        };
        var replacement = new User { UserId = 2 };

        sessionService.SelectedUser = replacement;
        Assert.AreSame(replacement, sessionService.SelectedUser);

        sessionService.Clear();

        Assert.IsNull(sessionService.SelectedUser);
    }
}

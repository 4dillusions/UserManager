/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Session;
using App4di.Dotnet.UserManager.Infrastructure.Entities;

namespace App4di.Dotnet.UserManager.Tests.Application.Session;

[TestClass]
public class SessionServiceTests
{
    [TestMethod]
    public void SessionStoresCurrentUser()
    {
        var sessionService = new SessionService();
        var user = new User { UserId = 1 };

        sessionService.CurrentUser = user;

        Assert.AreSame(user, sessionService.CurrentUser);
    }

    [TestMethod]
    public void SessionReplacesAndClearsCurrentState()
    {
        var sessionService = new SessionService
        {
            CurrentUser = new User { UserId = 1 }
        };
        var replacement = new User { UserId = 2 };

        sessionService.CurrentUser = replacement;
        Assert.AreSame(replacement, sessionService.CurrentUser);

        sessionService.Clear();

        Assert.IsNull(sessionService.CurrentUser);
    }
}

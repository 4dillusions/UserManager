/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Domain;
using FW4di.Dotnet.MVVM;

namespace App4di.Dotnet.UserManager.Tests.Domain;

[TestClass]
public class UserDataTests
{
    [TestMethod]
    public void UserDataDoesNotExposeMvvmValidationState()
    {
        Assert.IsFalse(typeof(NotificationObject).IsAssignableFrom(typeof(UserData)));
        Assert.IsNull(typeof(UserData).GetProperty("HasErrors"));
        Assert.IsNull(typeof(UserData).GetEvent("PropertyChanged"));
    }
}

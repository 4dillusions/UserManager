/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Models;

namespace App4di.Dotnet.UserManager.Tests.Presentation.Models;

[TestClass]
public class UserTests
{
    [TestMethod]
    public void UserRaisesPropertyChangedForUiBinding()
    {
        var user = new User();
        string? changedProperty = null;
        user.PropertyChanged += (_, args) => changedProperty = args.PropertyName;

        user.Surname = "Updated";

        Assert.AreEqual(nameof(User.Surname), changedProperty);
    }
}

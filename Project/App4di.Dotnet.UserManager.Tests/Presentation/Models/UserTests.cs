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

    [TestMethod]
    public void ToStringDoesNotContainPassword()
    {
        var user = new User
        {
            UserId = 42,
            LoginName = "albert",
            Password = "UniqueSecretPassword",
            FirstName = "Albert",
            Surname = "Einstein"
        };

        var result = user.ToString();

        Assert.DoesNotContain("UniqueSecretPassword", result);
    }

    [TestMethod]
    public void ToStringContainsUsefulIdentityInformation()
    {
        var user = new User
        {
            UserId = 42,
            LoginName = "albert",
            FirstName = "Albert",
            Surname = "Einstein"
        };

        var result = user.ToString();

        Assert.Contains("42", result);
        Assert.Contains("albert", result);
        Assert.Contains("Albert", result);
        Assert.Contains("Einstein", result);
    }
}

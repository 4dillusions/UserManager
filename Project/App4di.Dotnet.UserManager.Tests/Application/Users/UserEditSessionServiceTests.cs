/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Users;
using App4di.Dotnet.UserManager.Infrastructure.Entities;

namespace App4di.Dotnet.UserManager.Tests.Application.Users;

[TestClass]
public class UserEditSessionServiceTests
{
    [TestMethod]
    public void BeginEditCreatesCopyWithoutMutatingOriginal()
    {
        var original = CreateUser(1, "Original");
        var service = new UserEditSessionService();

        service.BeginEdit(original, [original]);
        service.EditingUser!.Surname = "Changed";

        Assert.AreNotSame(original, service.EditingUser);
        Assert.AreEqual("Original", original.Surname);
    }

    [TestMethod]
    public void CancelDiscardsEditableCopy()
    {
        var original = CreateUser(1, "Original");
        var service = new UserEditSessionService();
        service.BeginEdit(original, [original]);
        service.EditingUser!.Surname = "Changed";

        service.Cancel();

        Assert.AreEqual("Original", original.Surname);
        Assert.IsNull(service.EditingUser);
    }

    [TestMethod]
    public void CommitUpdatesMatchingUserWhenSelectedUserIsDifferentInstance()
    {
        var listUser = CreateUser(2, "Original");
        var selectedCopy = CreateUser(2, "Original");
        var users = new List<User> { CreateUser(1, "Other"), listUser };
        var service = new UserEditSessionService();
        service.BeginEdit(selectedCopy, users);
        service.EditingUser!.Surname = "Changed";

        var committedUsers = service.Commit();

        Assert.AreSame(users, committedUsers);
        Assert.AreEqual("Changed", listUser.Surname);
        Assert.AreEqual("Changed", selectedCopy.Surname);
        Assert.AreEqual("Other", users[0].Surname);
        Assert.IsNull(service.EditingUser);
    }

    private static User CreateUser(int userId, string surname)
    {
        return new User
        {
            UserId = userId,
            LoginName = $"User{userId}",
            Password = $"Password{userId}",
            FirstName = $"First{userId}",
            Surname = surname,
            BirthDate = new DateTime(2000, 1, userId),
            BirthPlace = "Budapest",
            AddressCity = "Budapest"
        };
    }
}

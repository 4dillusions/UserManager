/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Presentation.Users;
using App4di.Dotnet.UserManager.Application.Users;

namespace App4di.Dotnet.UserManager.Tests.Presentation.Users;

[TestClass]
public class UserEditSessionServiceTests
{
    [TestMethod]
    public void BeginAddCreatesEmptyUserWithNextId()
    {
        var service = new UserEditSessionService();

        service.BeginAdd([CreateUser(4, "Fourth"), CreateUser(9, "Ninth")]);

        Assert.AreEqual(UserEditMode.Add, service.Mode);
        Assert.AreEqual(10, service.EditingUser?.UserId);
        Assert.AreEqual(string.Empty, service.EditingUser?.LoginName);
        Assert.AreEqual(DateTime.MinValue, service.EditingUser?.BirthDate);
        Assert.IsTrue(service.HasChanges);
    }

    [TestMethod]
    public void BeginAddUsesOneForEmptyList()
    {
        var service = new UserEditSessionService();

        service.BeginAdd([]);

        Assert.AreEqual(1, service.EditingUser?.UserId);
    }

    [TestMethod]
    public void BeginAddRejectsExhaustedUserIdRangeWithoutStartingSession()
    {
        var service = new UserEditSessionService();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            service.BeginAdd([new User { UserId = int.MaxValue, LoginName = "Last" }]));

        StringAssert.Contains(exception.Message, "No user ID");
        Assert.IsNull(service.EditingUser);
    }

    [TestMethod]
    public void AddSnapshotAppendsUserAndCancelDoesNotChangeSource()
    {
        var existing = CreateUser(1, "Existing");
        var users = new List<User> { existing };
        var service = new UserEditSessionService();
        service.BeginAdd(users);
        service.EditingUser!.LoginName = "NewUser";

        var snapshot = service.CreateSaveSnapshot();

        Assert.HasCount(2, snapshot);
        Assert.AreEqual("NewUser", snapshot[1].LoginName);
        Assert.HasCount(1, users);

        service.Cancel();

        Assert.HasCount(1, users);
        Assert.IsNull(service.EditingUser);
    }

    [TestMethod]
    public void CompleteSaveRaisesSaveCompletedButCancelDoesNot()
    {
        var service = new UserEditSessionService();
        var saveCompletedCount = 0;
        service.SaveCompleted += (_, _) => saveCompletedCount++;
        service.BeginAdd([]);

        service.Cancel();

        Assert.AreEqual(0, saveCompletedCount);

        service.BeginAdd([]);
        service.CompleteSave();

        Assert.AreEqual(1, saveCompletedCount);
    }

    [TestMethod]
    public void BeginEditCreatesCopyWithoutMutatingOriginal()
    {
        var original = CreateUser(1, "Original");
        var service = new UserEditSessionService();

        service.BeginEdit(original, [original]);
        service.EditingUser!.Surname = "Changed";

        Assert.AreNotSame(original, service.EditingUser);
        Assert.AreEqual(UserEditMode.Edit, service.Mode);
        Assert.AreEqual("Original", original.Surname);
    }

    [TestMethod]
    public void HasChangesTracksEditsAgainstOriginalValues()
    {
        var original = CreateUser(1, "Original");
        var service = new UserEditSessionService();
        service.BeginEdit(original, [original]);

        Assert.IsFalse(service.HasChanges);

        service.EditingUser!.Surname = "Changed";

        Assert.IsTrue(service.HasChanges);

        service.EditingUser.Surname = "Original";

        Assert.IsFalse(service.HasChanges);
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
    public void SaveSnapshotDoesNotMutateOriginalBeforeSaveCompletes()
    {
        var listUser = CreateUser(2, "Original");
        var selectedCopy = CreateUser(2, "Original");
        var users = new List<User> { CreateUser(1, "Other"), listUser };
        var service = new UserEditSessionService();
        service.BeginEdit(selectedCopy, users);
        service.EditingUser!.Surname = "Changed";

        var snapshot = service.CreateSaveSnapshot();

        Assert.AreEqual("Original", listUser.Surname);
        Assert.AreEqual("Original", selectedCopy.Surname);
        Assert.AreEqual("Changed", snapshot[1].Surname);
        Assert.IsNotNull(service.EditingUser);

        service.CompleteSave();

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
            BirthDate = BirthDateRules.MinimumBirthDate.AddDays(userId),
            BirthPlace = "Budapest",
            AddressCity = "Budapest"
        };
    }
}

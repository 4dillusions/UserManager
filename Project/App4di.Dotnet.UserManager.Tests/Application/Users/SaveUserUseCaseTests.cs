/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Domain;

namespace App4di.Dotnet.UserManager.Tests.Application.Users;

[TestClass]
public class SaveUserUseCaseTests
{
    [TestMethod]
    public void SuccessfulSavePersistsSnapshotBeforeCompletingSession()
    {
        var calls = new List<string>();
        var users = new List<UserData> { new() { UserId = 1, Surname = "Changed", BirthDate = new DateTime(2000, 1, 1) } };
        var session = new UserSaveSessionStub(users, calls);
        var repository = new UserRepositoryStub(calls);
        var useCase = new SaveUserUseCase(repository);

        useCase.Execute(session);

        CollectionAssert.AreEqual(new[] { "snapshot", "save", "complete" }, calls);
        Assert.AreSame(users, repository.SavedUsers);
    }

    [TestMethod]
    public void FailedRepositorySaveDoesNotCompleteSession()
    {
        var calls = new List<string>();
        var session = new UserSaveSessionStub([new UserData { BirthDate = new DateTime(2000, 1, 1) }], calls);
        var repository = new UserRepositoryStub(calls) { SaveException = new IOException("Save failed") };
        var useCase = new SaveUserUseCase(repository);

        Assert.Throws<IOException>(() => useCase.Execute(session));

        CollectionAssert.AreEqual(new[] { "snapshot", "save" }, calls);
    }

    [TestMethod]
    public void DuplicateLoginNameIsRejectedBeforePersistence()
    {
        var calls = new List<string>();
        var session = new UserSaveSessionStub(
        [
            new UserData { UserId = 1, LoginName = "Existing", BirthDate = new DateTime(2000, 1, 1) },
            new UserData { UserId = 2, LoginName = "existing", BirthDate = new DateTime(2000, 1, 1) }
        ], calls);
        var repository = new UserRepositoryStub(calls);
        var useCase = new SaveUserUseCase(repository);

        var exception = Assert.Throws<InvalidOperationException>(() => useCase.Execute(session));

        StringAssert.Contains(exception.Message, "already exists");
        CollectionAssert.AreEqual(new[] { "snapshot" }, calls);
        Assert.IsNull(repository.SavedUsers);
    }

    [TestMethod]
    public void InvalidBirthDateIsRejectedBeforePersistence()
    {
        var calls = new List<string>();
        var session = new UserSaveSessionStub(
            [new UserData { UserId = 1, LoginName = "User", BirthDate = new DateTime(1499, 12, 31) }],
            calls);
        var repository = new UserRepositoryStub(calls);
        var useCase = new SaveUserUseCase(repository);

        var exception = Assert.Throws<InvalidOperationException>(() => useCase.Execute(session));

        StringAssert.Contains(exception.Message, "Birth date");
        CollectionAssert.AreEqual(Array.Empty<string>(), calls);
        Assert.IsNull(repository.SavedUsers);
    }

    [TestMethod]
    public void InvalidDateOnUnchangedUserDoesNotBlockValidUserSave()
    {
        var calls = new List<string>();
        var users = new List<UserData>
        {
            new() { UserId = 1, LoginName = "Legacy", BirthDate = new DateTime(1499, 12, 31) },
            new() { UserId = 2, LoginName = "legacy", BirthDate = new DateTime(2000, 1, 1) },
            new() { UserId = 3, LoginName = "Edited", BirthDate = new DateTime(2000, 1, 1) }
        };
        var repository = new UserRepositoryStub(calls);
        var useCase = new SaveUserUseCase(repository);

        useCase.Execute(new UserSaveSessionStub(users, calls));

        Assert.AreSame(users, repository.SavedUsers);
        CollectionAssert.AreEqual(new[] { "snapshot", "save", "complete" }, calls);
    }

    private sealed class UserSaveSessionStub(List<UserData> users, List<string> calls) : IUserSaveSession
    {
        public UserData UserToSave => users[^1];

        public IReadOnlyList<UserData> CreateSaveSnapshot()
        {
            calls.Add("snapshot");
            return users;
        }

        public void CompleteSave()
        {
            calls.Add("complete");
        }
    }

    private sealed class UserRepositoryStub(List<string> calls) : IUserRepository
    {
        public IEnumerable<UserData>? SavedUsers { get; private set; }
        public Exception? SaveException { get; init; }

        public IReadOnlyList<UserData> LoadUsers()
        {
            throw new NotSupportedException();
        }

        public void SaveUsers(IEnumerable<UserData> users)
        {
            calls.Add("save");
            if (SaveException != null)
                throw SaveException;

            SavedUsers = users;
        }
    }
}

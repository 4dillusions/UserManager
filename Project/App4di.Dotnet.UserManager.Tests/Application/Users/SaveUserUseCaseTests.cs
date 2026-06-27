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
        var users = new List<UserData> { new() { UserId = 1, Surname = "Changed" } };
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
        var session = new UserSaveSessionStub([new UserData()], calls);
        var repository = new UserRepositoryStub(calls) { SaveException = new IOException("Save failed") };
        var useCase = new SaveUserUseCase(repository);

        Assert.Throws<IOException>(() => useCase.Execute(session));

        CollectionAssert.AreEqual(new[] { "snapshot", "save" }, calls);
    }

    private sealed class UserSaveSessionStub(List<UserData> users, List<string> calls) : IUserSaveSession
    {
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

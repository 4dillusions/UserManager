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
public class DeleteUserUseCaseTests
{
    [TestMethod]
    public void ExecuteDeletesSelectedUser()
    {
        var repository = CreateRepository();
        var useCase = new DeleteUserUseCase(repository);

        var deleted = useCase.Execute(1);

        Assert.IsTrue(deleted);
        CollectionAssert.AreEqual(new[] { 2 }, repository.SavedUsers.Select(user => user.UserId).ToArray());
    }

    [TestMethod]
    public void ExecutePersistsUpdatedUserList()
    {
        var repository = CreateRepository();
        var useCase = new DeleteUserUseCase(repository);

        useCase.Execute(2);

        Assert.AreEqual(1, repository.SaveCallCount);
        CollectionAssert.AreEqual(new[] { 1 }, repository.SavedUsers.Select(user => user.UserId).ToArray());
    }

    [TestMethod]
    public void ExecuteDoesNotDeleteLastUser()
    {
        var repository = new UserRepositoryStub([new UserData { UserId = 1 }]);
        var useCase = new DeleteUserUseCase(repository);

        var exception = Assert.Throws<InvalidOperationException>(() => useCase.Execute(1));

        Assert.AreEqual("The last user cannot be deleted.", exception.Message);
        Assert.AreEqual(0, repository.SaveCallCount);
    }

    [TestMethod]
    public void ExecuteReturnsFalseForUnknownUserId()
    {
        var repository = CreateRepository();
        var useCase = new DeleteUserUseCase(repository);

        var deleted = useCase.Execute(99);

        Assert.IsFalse(deleted);
        Assert.AreEqual(0, repository.SaveCallCount);
    }

    private static UserRepositoryStub CreateRepository()
    {
        return new UserRepositoryStub(
        [
            new UserData { UserId = 1 },
            new UserData { UserId = 2 }
        ]);
    }

    private sealed class UserRepositoryStub(IEnumerable<UserData> users) : IUserRepository
    {
        private readonly List<UserData> users = users.ToList();

        public int SaveCallCount { get; private set; }
        public List<UserData> SavedUsers { get; private set; } = [];

        public IReadOnlyList<UserData> LoadUsers()
        {
            return users;
        }

        public void SaveUsers(IEnumerable<UserData> users)
        {
            SaveCallCount++;
            SavedUsers = users.ToList();
        }
    }
}

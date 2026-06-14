/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Authentication;
using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;

namespace App4di.Dotnet.UserManager.Tests.Application.Authentication;

[TestClass]
public class AuthenticationServiceTests
{
    private readonly AuthenticationService authenticationService = new(
        new UserRepositoryStub(
        [
            new User { LoginName = "Albert", Password = "Albert1" }
        ]));

    [TestMethod]
    public void AuthenticateReturnsTrueForMatchingCredentials()
    {
        Assert.IsTrue(authenticationService.Authenticate("Albert", "Albert1"));
    }

    [TestMethod]
    public void AuthenticateReturnsFalseForInvalidPassword()
    {
        Assert.IsFalse(authenticationService.Authenticate("Albert", "WrongPassword"));
    }

    [TestMethod]
    public void AuthenticateReturnsFalseForUnknownUser()
    {
        Assert.IsFalse(authenticationService.Authenticate("Unknown", "Albert1"));
    }

    private sealed class UserRepositoryStub(List<User> users) : IUserRepository
    {
        public List<User> LoadUsers() => users;

        public void SaveUsers(List<User> users)
        {
            throw new NotSupportedException();
        }
    }
}

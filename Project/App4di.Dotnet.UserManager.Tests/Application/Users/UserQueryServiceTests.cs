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
public class UserQueryServiceTests
{
    private readonly UserQueryService userQueryService;

    public UserQueryServiceTests()
    {
        var users = CreateUsers();
        userQueryService = new UserQueryService(
            new UserRepositoryStub(users),
            new AddressCityRepositoryStub(
            [
                "Budapest",
                "Kecskemet"
            ]));
    }

    [TestMethod]
    public void EmptyFilterReturnsAllUsers()
    {
        var users = userQueryService.GetUsers(new UserQueryCriteria());

        Assert.HasCount(3, users);
    }

    [TestMethod]
    public void TextSearchMatchesConcatenatedUserDataCaseSensitively()
    {
        var matchingUsers = userQueryService.GetUsers(new UserQueryCriteria { TextInAll = "Albert" });
        var differentCaseUsers = userQueryService.GetUsers(new UserQueryCriteria { TextInAll = "albert" });

        Assert.HasCount(1, matchingUsers);
        Assert.AreEqual(1, matchingUsers[0].UserId);
        Assert.IsEmpty(differentCaseUsers);
    }

    [TestMethod]
    public void CityFilterReturnsUsersFromExactCity()
    {
        var users = userQueryService.GetUsers(new UserQueryCriteria { AddressCity = "Budapest" });

        Assert.HasCount(2, users);
        Assert.IsTrue(users.All(user => user.AddressCity == "Budapest"));
    }

    [TestMethod]
    public void CombinedFiltersApplyCityAndTextSearch()
    {
        var users = userQueryService.GetUsers(new UserQueryCriteria
        {
            AddressCity = "Budapest",
            TextInAll = "Rubik"
        });

        Assert.HasCount(1, users);
        Assert.AreEqual(2, users[0].UserId);
    }

    [TestMethod]
    public void AddressCityOptionsPreserveRepositoryOrder()
    {
        var cities = userQueryService.GetAddressCities();

        Assert.HasCount(2, cities);
        Assert.AreEqual("Budapest", cities[0]);
        Assert.AreEqual("Kecskemet", cities[1]);
    }

    private static List<UserData> CreateUsers()
    {
        return
        [
            new UserData { UserId = 1, LoginName = "Albert", FirstName = "Albert", Surname = "Einstein", AddressCity = "Kecskemet" },
            new UserData { UserId = 2, LoginName = "Erno", FirstName = "Erno", Surname = "Rubik", AddressCity = "Budapest" },
            new UserData { UserId = 3, LoginName = "Zoltan", FirstName = "Zoltan", Surname = "Kodaly", AddressCity = "Budapest" }
        ];
    }

    private sealed class UserRepositoryStub(List<UserData> users) : IUserRepository
    {
        public List<UserData> LoadUsers() => users;

        public void SaveUsers(List<UserData> users)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class AddressCityRepositoryStub(List<string> cities) : IAddressCityRepository
    {
        public List<string> LoadAddressCities() => cities;
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Users;
using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;

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
                new AddressCity { CityName = "Budapest" },
                new AddressCity { CityName = "Kecskemet" }
            ]));
    }

    [TestMethod]
    public void EmptyFilterReturnsAllUsers()
    {
        var users = userQueryService.GetUsers(new UserFilter());

        Assert.HasCount(3, users);
    }

    [TestMethod]
    public void TextSearchMatchesConcatenatedUserDataCaseSensitively()
    {
        var matchingUsers = userQueryService.GetUsers(new UserFilter { TextInAll = "Albert" });
        var differentCaseUsers = userQueryService.GetUsers(new UserFilter { TextInAll = "albert" });

        Assert.HasCount(1, matchingUsers);
        Assert.AreEqual(1, matchingUsers[0].UserId);
        Assert.IsEmpty(differentCaseUsers);
    }

    [TestMethod]
    public void CityFilterReturnsUsersFromExactCity()
    {
        var users = userQueryService.GetUsers(new UserFilter { AddressCity = "Budapest" });

        Assert.HasCount(2, users);
        Assert.IsTrue(users.All(user => user.AddressCity == "Budapest"));
    }

    [TestMethod]
    public void CombinedFiltersApplyCityAndTextSearch()
    {
        var users = userQueryService.GetUsers(new UserFilter
        {
            AddressCity = "Budapest",
            TextInAll = "Rubik"
        });

        Assert.HasCount(1, users);
        Assert.AreEqual(2, users[0].UserId);
    }

    [TestMethod]
    public void AddressCityOptionsStartWithAllAndPreserveRepositoryOrder()
    {
        var cities = userQueryService.GetAddressCities();

        Assert.HasCount(3, cities);
        Assert.AreEqual(AddressCity.DefaultName, cities[0].CityName);
        Assert.AreEqual("Budapest", cities[1].CityName);
        Assert.AreEqual("Kecskemet", cities[2].CityName);
    }

    private static List<User> CreateUsers()
    {
        return
        [
            new User { UserId = 1, LoginName = "Albert", FirstName = "Albert", Surname = "Einstein", AddressCity = "Kecskemet" },
            new User { UserId = 2, LoginName = "Erno", FirstName = "Erno", Surname = "Rubik", AddressCity = "Budapest" },
            new User { UserId = 3, LoginName = "Zoltan", FirstName = "Zoltan", Surname = "Kodaly", AddressCity = "Budapest" }
        ];
    }

    private sealed class UserRepositoryStub(List<User> users) : IUserRepository
    {
        public List<User> LoadUsers() => users;

        public void SaveUsers(List<User> users)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class AddressCityRepositoryStub(List<AddressCity> cities) : IAddressCityRepository
    {
        public List<AddressCity> LoadAddressCities() => cities;
    }
}

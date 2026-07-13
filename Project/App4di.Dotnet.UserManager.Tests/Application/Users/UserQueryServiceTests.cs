/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Domain;
using System.Globalization;

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
    [DataRow("Albert", 1)]
    [DataRow("Einstein", 1)]
    [DataRow("Ulm", 1)]
    [DataRow("Kecskemet", 1)]
    [DataRow("Rubik", 2)]
    public void TextSearchMatchesVisibleFields(string searchText, int expectedUserId)
    {
        var users = userQueryService.GetUsers(new UserQueryCriteria { TextInAll = searchText });

        Assert.HasCount(1, users);
        Assert.AreEqual(expectedUserId, users[0].UserId);
    }

    [TestMethod]
    public void TextSearchRemainsCaseSensitive()
    {
        var users = userQueryService.GetUsers(new UserQueryCriteria { TextInAll = "albert" });

        Assert.IsEmpty(users);
    }

    [TestMethod]
    public void TextSearchMatchesUserId()
    {
        var service = new UserQueryService(
            new UserRepositoryStub(
            [
                new UserData
                {
                    UserId = 42,
                    LoginName = "User",
                    BirthDate = BirthDateRules.MinimumBirthDate
                }
            ]),
            new AddressCityRepositoryStub([]));

        var users = service.GetUsers(new UserQueryCriteria { TextInAll = "42" });

        Assert.HasCount(1, users);
        Assert.AreEqual(42, users[0].UserId);
    }

    [TestMethod]
    public void TextSearchMatchesInvariantBirthDateRegardlessOfCurrentCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("hu-HU");
            var users = userQueryService.GetUsers(new UserQueryCriteria { TextInAll = "1879-03-14" });

            Assert.HasCount(1, users);
            Assert.AreEqual(1, users[0].UserId);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [TestMethod]
    public void TextSearchDoesNotMatchPassword()
    {
        var users = userQueryService.GetUsers(new UserQueryCriteria { TextInAll = "SecretPassword" });

        Assert.IsEmpty(users);
    }

    [TestMethod]
    public void TextSearchDoesNotUseUserToString()
    {
        var service = new UserQueryService(
            new UserRepositoryStub([new UserDataWithMisleadingToString()]),
            new AddressCityRepositoryStub([]));

        var users = service.GetUsers(new UserQueryCriteria { TextInAll = "ToStringOnlyValue" });

        Assert.IsEmpty(users);
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
            new UserData
            {
                UserId = 1,
                LoginName = "Albert",
                Password = "SecretPassword",
                FirstName = "Albert",
                Surname = "Einstein",
                BirthDate = new DateTime(1879, 3, 14),
                BirthPlace = "Ulm",
                AddressCity = "Kecskemet"
            },
            new UserData { UserId = 2, LoginName = "Erno", FirstName = "Erno", Surname = "Rubik", AddressCity = "Budapest" },
            new UserData { UserId = 3, LoginName = "Zoltan", FirstName = "Zoltan", Surname = "Kodaly", AddressCity = "Budapest" }
        ];
    }

    private sealed class UserDataWithMisleadingToString : UserData
    {
        public override string ToString() => "ToStringOnlyValue";
    }

    private sealed class UserRepositoryStub(List<UserData> users) : IUserRepository
    {
        public IReadOnlyList<UserData> LoadUsers() => users;

        public void SaveUsers(IEnumerable<UserData> users)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class AddressCityRepositoryStub(List<string> cities) : IAddressCityRepository
    {
        public IReadOnlyList<string> LoadAddressCities() => cities;
    }
}

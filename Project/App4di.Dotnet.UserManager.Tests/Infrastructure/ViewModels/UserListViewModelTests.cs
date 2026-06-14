/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Users;
using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Service;
using App4di.Dotnet.UserManager.Infrastructure.ViewModels;

namespace App4di.Dotnet.UserManager.Tests.Infrastructure.ViewModels;

[TestClass]
[DoNotParallelize]
public class UserListViewModelTests
{
    [TestCleanup]
    public void TestCleanup()
    {
        User.CurrentUser = null;
        UserList.CurrentUsers = null;
    }

    [TestMethod]
    public void UserListViewModelUsesUserQueryService()
    {
        var queryService = new UserQueryServiceStub();
        var viewModel = new UserListViewModel(
            new MainViewModel(),
            new MessageServiceStub(),
            queryService);

        viewModel.TextInAll = "Rubik";
        viewModel.SelectedAddressCity = viewModel.AddressCities[1];
        viewModel.FindCommand.Execute(null);

        Assert.AreEqual(2, queryService.GetUsersCallCount);
        Assert.AreEqual(1, queryService.GetAddressCitiesCallCount);
        Assert.AreEqual("Rubik", queryService.LastFilter?.TextInAll);
        Assert.AreEqual("Budapest", queryService.LastFilter?.AddressCity);
        Assert.HasCount(1, viewModel.Users);
    }

    private sealed class UserQueryServiceStub : IUserQueryService
    {
        public int GetUsersCallCount { get; private set; }
        public int GetAddressCitiesCallCount { get; private set; }
        public UserFilter? LastFilter { get; private set; }

        public List<User> GetUsers(UserFilter filter)
        {
            GetUsersCallCount++;
            LastFilter = new UserFilter
            {
                AddressCity = filter.AddressCity,
                TextInAll = filter.TextInAll
            };

            return
            [
                new User { UserId = GetUsersCallCount, Surname = "Rubik", AddressCity = "Budapest" }
            ];
        }

        public List<AddressCity> GetAddressCities()
        {
            GetAddressCitiesCallCount++;
            return
            [
                new AddressCity(),
                new AddressCity { CityName = "Budapest" }
            ];
        }
    }

    private sealed class MessageServiceStub : IMessageService
    {
        public void ShowMessage(string message, string? title = null)
        {
        }
    }
}

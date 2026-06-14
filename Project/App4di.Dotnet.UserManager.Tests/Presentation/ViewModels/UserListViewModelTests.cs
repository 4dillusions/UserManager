/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Export;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Services;
using App4di.Dotnet.UserManager.Presentation.Session;
using App4di.Dotnet.UserManager.Presentation.Users;
using App4di.Dotnet.UserManager.Presentation.ViewModels;

namespace App4di.Dotnet.UserManager.Tests.Presentation.ViewModels;

[TestClass]
public class UserListViewModelTests
{
    [TestMethod]
    public void UserListViewModelUsesUserQueryService()
    {
        var queryService = new UserQueryServiceStub();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new MessageServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new SessionService(),
            new UserEditSessionService());

        viewModel.TextInAll = "Rubik";
        viewModel.SelectedAddressCity = viewModel.AddressCities[1];
        viewModel.FindCommand.Execute(null);

        Assert.AreEqual(2, queryService.GetUsersCallCount);
        Assert.AreEqual(1, queryService.GetAddressCitiesCallCount);
        Assert.AreEqual("Rubik", queryService.LastFilter?.TextInAll);
        Assert.AreEqual("Budapest", queryService.LastFilter?.AddressCity);
        Assert.HasCount(1, viewModel.Users);
    }

    [TestMethod]
    public void ExportCommandPassesCurrentlyVisibleUsersToExportService()
    {
        var queryService = new UserQueryServiceStub();
        var exportService = new UserExportServiceStub();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new MessageServiceStub(),
            queryService,
            exportService,
            new SessionService(),
            new UserEditSessionService());

        viewModel.TextInAll = "Rubik";
        viewModel.FindCommand.Execute(null);
        var visibleUsers = viewModel.Users.ToList();

        viewModel.ExportCommand.Execute(null);

        Assert.AreEqual(1, exportService.CallCount);
        CollectionAssert.AreEqual(
            visibleUsers.Select(user => user.UserId).ToArray(),
            exportService.ExportedUsers.Select(user => user.UserId).ToArray());
    }

    [TestMethod]
    public void SelectionAndEditStateAreStoredInSessionService()
    {
        var queryService = new UserQueryServiceStub();
        var sessionService = new SessionService();
        var editSessionService = new UserEditSessionService();
        var navigationService = new NavigationService();
        var viewModel = new UserListViewModel(
            navigationService,
            new MessageServiceStub(),
            queryService,
            new UserExportServiceStub(),
            sessionService,
            editSessionService);

        var selectedUser = viewModel.Users[0];
        viewModel.SelectedUser = selectedUser;
        viewModel.EditCommand.Execute(null);

        Assert.AreSame(selectedUser, sessionService.CurrentUser);
        Assert.IsNotNull(editSessionService.EditingUser);
        Assert.AreNotSame(selectedUser, editSessionService.EditingUser);
        Assert.AreEqual(selectedUser.UserId, editSessionService.EditingUser.UserId);
        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
    }

    private sealed class UserQueryServiceStub : IUserQueryService
    {
        public int GetUsersCallCount { get; private set; }
        public int GetAddressCitiesCallCount { get; private set; }
        public UserQueryCriteria? LastFilter { get; private set; }

        public List<UserData> GetUsers(UserQueryCriteria filter)
        {
            GetUsersCallCount++;
            LastFilter = new UserQueryCriteria
            {
                AddressCity = filter.AddressCity,
                TextInAll = filter.TextInAll
            };

            return
            [
                new UserData { UserId = GetUsersCallCount, Surname = "Rubik", AddressCity = "Budapest" }
            ];
        }

        public List<string> GetAddressCities()
        {
            GetAddressCitiesCallCount++;
            return
            [
                "Budapest"
            ];
        }
    }

    private sealed class MessageServiceStub : IMessageService
    {
        public void ShowMessage(string message, string? title = null)
        {
        }
    }

    private sealed class UserExportServiceStub : IUserExportService
    {
        public int CallCount { get; private set; }
        public List<UserData> ExportedUsers { get; private set; } = [];

        public bool ExportUsers(List<UserData> users)
        {
            CallCount++;
            ExportedUsers = users;
            return true;
        }
    }
}

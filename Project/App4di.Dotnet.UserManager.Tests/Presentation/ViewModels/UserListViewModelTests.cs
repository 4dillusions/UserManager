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
using System.Collections.ObjectModel;

namespace App4di.Dotnet.UserManager.Tests.Presentation.ViewModels;

[TestClass]
public class UserListViewModelTests
{
    [TestMethod]
    public void AddCommandStartsAddSessionAndNavigatesToEditor()
    {
        var navigationService = new NavigationService();
        var editSessionService = new UserEditSessionService();
        var viewModel = new UserListViewModel(
            navigationService,
            new UserNotificationServiceStub(),
            new MutableUserQueryService(
            [
                new UserData { UserId = 2, LoginName = "First" },
                new UserData { UserId = 7, LoginName = "Second" }
            ]),
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            editSessionService);

        viewModel.AddCommand.Execute(null);

        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
        Assert.AreEqual(UserEditMode.Add, editSessionService.Mode);
        Assert.AreEqual(8, editSessionService.EditingUser?.UserId);
    }

    [TestMethod]
    public void AddCommandReportsLoadFailureWithoutStartingSessionOrNavigating()
    {
        var navigationService = new NavigationService();
        var editSessionService = new UserEditSessionService();
        var userNotificationService = new UserNotificationServiceStub();
        var queryService = new MutableUserQueryService(
            [new UserData { UserId = 1, LoginName = "First" }]);
        var viewModel = new UserListViewModel(
            navigationService,
            userNotificationService,
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            editSessionService);
        queryService.GetUsersException = new IOException("Load failed");

        viewModel.AddCommand.Execute(null);

        Assert.AreEqual(ViewType.Login, navigationService.CurrentView);
        Assert.IsNull(editSessionService.EditingUser);
        Assert.AreEqual("Add error", userNotificationService.Title);
        StringAssert.Contains(userNotificationService.Message, "Load failed");
    }

    [TestMethod]
    public void ReturningWithoutSavePreservesSearchState()
    {
        var navigationService = new NavigationService();
        var queryService = new UserQueryServiceStub();
        var viewModel = new UserListViewModel(
            navigationService,
            new UserNotificationServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            new UserEditSessionService());
        viewModel.TextInAll = "Rubik";
        viewModel.SelectedAddressCity = viewModel.AddressCities[1];

        navigationService.Navigate(ViewType.User);
        navigationService.Navigate(ViewType.UserList);

        Assert.AreEqual("Rubik", viewModel.TextInAll);
        Assert.AreEqual("Budapest", viewModel.SelectedAddressCity?.CityName);
        Assert.AreEqual(1, queryService.GetUsersCallCount);
    }

    [TestMethod]
    public void SuccessfulSaveRefreshesUsersWithoutClearingFilter()
    {
        var queryService = new MutableUserQueryService(
            [new UserData { UserId = 1, LoginName = "First" }]);
        var editSessionService = new UserEditSessionService();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            editSessionService);
        viewModel.TextInAll = "First";
        editSessionService.BeginAdd(viewModel.Users);
        queryService.Users.Add(new UserData { UserId = 2, LoginName = "Second" });

        editSessionService.CompleteSave();

        Assert.AreEqual("First", viewModel.TextInAll);
        Assert.HasCount(2, viewModel.Users);
    }

    [TestMethod]
    public void SuccessfulSaveRefreshesAddressCitiesAndPreservesSelectedCity()
    {
        var queryService = new MutableUserQueryService(
            [new UserData { UserId = 1, LoginName = "First", AddressCity = "Budapest" }]);
        var editSessionService = new UserEditSessionService();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            editSessionService);
        viewModel.SelectedAddressCity = viewModel.AddressCities.Single(city => city.CityName == "Budapest");
        editSessionService.BeginAdd(viewModel.Users);
        queryService.Users.Add(new UserData { UserId = 2, LoginName = "Second", AddressCity = "Szeged" });

        editSessionService.CompleteSave();

        CollectionAssert.AreEquivalent(
            new[] { AddressCity.DefaultName, "Budapest", "Szeged" },
            viewModel.AddressCities.Select(city => city.CityName).ToArray());
        Assert.AreEqual("Budapest", viewModel.SelectedAddressCity?.CityName);
    }

    [TestMethod]
    public void RefreshFailureAfterSaveIsReportedWithoutFailingCompletedSession()
    {
        var queryService = new MutableUserQueryService(
            [new UserData { UserId = 1, LoginName = "First", AddressCity = "Budapest" }]);
        var editSessionService = new UserEditSessionService();
        var userNotificationService = new UserNotificationServiceStub();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            userNotificationService,
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            editSessionService);
        viewModel.SelectedAddressCity = viewModel.AddressCities.Single(city => city.CityName == "Budapest");
        var addressCitiesBeforeRefresh = viewModel.AddressCities;
        var selectedAddressCityBeforeRefresh = viewModel.SelectedAddressCity;
        editSessionService.BeginAdd(viewModel.Users);
        queryService.GetAddressCitiesException = new IOException("Refresh failed");

        editSessionService.CompleteSave();

        Assert.IsNull(editSessionService.EditingUser);
        Assert.AreSame(addressCitiesBeforeRefresh, viewModel.AddressCities);
        Assert.AreSame(selectedAddressCityBeforeRefresh, viewModel.SelectedAddressCity);
        StringAssert.Contains(userNotificationService.Message, "Refresh failed");
    }

    [TestMethod]
    public void UserListViewModelUsesUserQueryService()
    {
        var queryService = new UserQueryServiceStub();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
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
    public void ClearSearchCommandClearsTextAndRefreshesUsers()
    {
        var queryService = new UserQueryServiceStub();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            new UserEditSessionService());
        viewModel.TextInAll = "Rubik";

        Assert.IsTrue(viewModel.ClearSearchCommand.CanExecute(null));

        viewModel.ClearSearchCommand.Execute(null);

        Assert.AreEqual(string.Empty, viewModel.TextInAll);
        Assert.AreEqual(2, queryService.GetUsersCallCount);
        Assert.AreEqual(string.Empty, queryService.LastFilter?.TextInAll);
        Assert.IsFalse(viewModel.ClearSearchCommand.CanExecute(null));
    }

    [TestMethod]
    public void ExportCommandPassesCurrentlyVisibleUsersToExportService()
    {
        var queryService = new UserQueryServiceStub();
        var exportService = new UserExportServiceStub();
        var userNotificationService = new UserNotificationServiceStub();
        var viewModel = new UserListViewModel(
            new NavigationService(),
            userNotificationService,
            queryService,
            exportService,
            new DeleteUserUseCaseStub(),
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
        Assert.AreEqual(
            $"Data exported to JSON file:{Environment.NewLine}{UserExportServiceStub.ExportFilePath}",
            userNotificationService.Message);
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
            new UserNotificationServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            sessionService,
            editSessionService);

        var selectedUser = viewModel.Users[0];
        viewModel.SelectedUser = selectedUser;
        viewModel.EditCommand.Execute(null);

        Assert.AreSame(selectedUser, sessionService.SelectedUser);
        Assert.IsNotNull(editSessionService.EditingUser);
        Assert.AreNotSame(selectedUser, editSessionService.EditingUser);
        Assert.AreEqual(selectedUser.UserId, editSessionService.EditingUser.UserId);
        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
    }

    [TestMethod]
    public void EditCommandRequiresSelectedUserAndNotifiesWhenSelectionChanges()
    {
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            new UserQueryServiceStub(),
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            new UserEditSessionService());
        var editCommand = viewModel.EditCommand;
        var canExecuteChangedCount = 0;
        editCommand.CanExecuteChanged += (_, _) => canExecuteChangedCount++;

        Assert.IsTrue(editCommand.CanExecute(null));

        var user = viewModel.SelectedUser;
        viewModel.SelectedUser = null;

        Assert.IsFalse(editCommand.CanExecute(null));

        viewModel.SelectedUser = user;

        Assert.IsTrue(editCommand.CanExecute(null));
        Assert.AreEqual(2, canExecuteChangedCount);
    }

    [TestMethod]
    public void ExportCommandRequiresVisibleUsersAndNotifiesWhenUsersChange()
    {
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            new UserQueryServiceStub(),
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            new UserEditSessionService());
        var exportCommand = viewModel.ExportCommand;
        var canExecuteChangedCount = 0;
        exportCommand.CanExecuteChanged += (_, _) => canExecuteChangedCount++;

        Assert.IsTrue(exportCommand.CanExecute(null));

        viewModel.Users = [];

        Assert.IsFalse(exportCommand.CanExecute(null));

        viewModel.Users = new ObservableCollection<User> { new() };

        Assert.IsTrue(exportCommand.CanExecute(null));
        Assert.AreEqual(2, canExecuteChangedCount);
    }

    [TestMethod]
    public void DeleteCommandRequiresSelectionAndMoreThanOneUser()
    {
        var queryService = new MutableUserQueryService(
        [
            new UserData { UserId = 1, LoginName = "First" },
            new UserData { UserId = 2, LoginName = "Second" }
        ]);
        var viewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            queryService,
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            new UserEditSessionService());

        Assert.IsTrue(viewModel.DeleteCommand.CanExecute(null));

        var selectedUser = viewModel.SelectedUser;
        viewModel.SelectedUser = null;

        Assert.IsFalse(viewModel.DeleteCommand.CanExecute(null));

        viewModel.SelectedUser = selectedUser;

        Assert.IsTrue(viewModel.DeleteCommand.CanExecute(null));

        var singleUserViewModel = new UserListViewModel(
            new NavigationService(),
            new UserNotificationServiceStub(),
            new MutableUserQueryService([new UserData { UserId = 1, LoginName = "Only" }]),
            new UserExportServiceStub(),
            new DeleteUserUseCaseStub(),
            new SessionService(),
            new UserEditSessionService());

        Assert.IsFalse(singleUserViewModel.DeleteCommand.CanExecute(null));
    }

    [TestMethod]
    public void DeleteCommandDoesNotDeleteWhenConfirmationIsDeclined()
    {
        var queryService = new MutableUserQueryService(
        [
            new UserData { UserId = 1, LoginName = "First" },
            new UserData { UserId = 2, LoginName = "Second" }
        ]);
        var deleteUseCase = new DeleteUserUseCaseStub();
        var userNotificationService = new UserNotificationServiceStub { ConfirmationResult = false };
        var viewModel = new UserListViewModel(
            new NavigationService(),
            userNotificationService,
            queryService,
            new UserExportServiceStub(),
            deleteUseCase,
            new SessionService(),
            new UserEditSessionService());

        viewModel.DeleteCommand.Execute(null);

        Assert.AreEqual(1, userNotificationService.ConfirmationCallCount);
        Assert.AreEqual(0, deleteUseCase.CallCount);
        Assert.HasCount(2, viewModel.Users);
    }

    [TestMethod]
    public void DeleteCommandDeletesAndRefreshesUsersWhenConfirmationIsAccepted()
    {
        var queryService = new MutableUserQueryService(
        [
            new UserData { UserId = 1, LoginName = "First" },
            new UserData { UserId = 2, LoginName = "Second" }
        ]);
        var deleteUseCase = new DeleteUserUseCaseStub
        {
            OnExecute = userId => queryService.Users.RemoveAll(user => user.UserId == userId)
        };
        var userNotificationService = new UserNotificationServiceStub { ConfirmationResult = true };
        var viewModel = new UserListViewModel(
            new NavigationService(),
            userNotificationService,
            queryService,
            new UserExportServiceStub(),
            deleteUseCase,
            new SessionService(),
            new UserEditSessionService());

        viewModel.DeleteCommand.Execute(null);

        Assert.AreEqual(1, deleteUseCase.DeletedUserId);
        Assert.HasCount(1, viewModel.Users);
        Assert.AreEqual(2, viewModel.Users[0].UserId);
        Assert.AreEqual(2, viewModel.SelectedUser?.UserId);
        Assert.IsFalse(viewModel.DeleteCommand.CanExecute(null));
    }

    private sealed class UserQueryServiceStub : IUserQueryService
    {
        public int GetUsersCallCount { get; private set; }
        public int GetAddressCitiesCallCount { get; private set; }
        public UserQueryCriteria? LastFilter { get; private set; }

        public IReadOnlyList<UserData> GetUsers(UserQueryCriteria filter)
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

        public IReadOnlyList<string> GetAddressCities()
        {
            GetAddressCitiesCallCount++;
            return
            [
                "Budapest"
            ];
        }
    }

    private sealed class UserNotificationServiceStub : IUserNotificationService
    {
        public string? Message { get; private set; }
        public string? Title { get; private set; }
        public bool ConfirmationResult { get; init; }
        public int ConfirmationCallCount { get; private set; }

        public void ShowMessage(string message, string? title = null)
        {
            Message = message;
            Title = title;
        }

        public bool ShowConfirmation(string message, string? title = null)
        {
            ConfirmationCallCount++;
            return ConfirmationResult;
        }
    }

    private sealed class DeleteUserUseCaseStub : IDeleteUserUseCase
    {
        public int CallCount { get; private set; }
        public int? DeletedUserId { get; private set; }
        public Action<int>? OnExecute { get; init; }

        public bool Execute(int userId)
        {
            CallCount++;
            DeletedUserId = userId;
            OnExecute?.Invoke(userId);
            return true;
        }
    }

    private sealed class MutableUserQueryService(IEnumerable<UserData> users) : IUserQueryService
    {
        public List<UserData> Users { get; } = users.ToList();
        public Exception? GetUsersException { get; set; }
        public Exception? GetAddressCitiesException { get; set; }

        public IReadOnlyList<UserData> GetUsers(UserQueryCriteria filter)
        {
            if (GetUsersException != null)
                throw GetUsersException;

            return Users.ToList();
        }

        public IReadOnlyList<string> GetAddressCities()
        {
            if (GetAddressCitiesException != null)
                throw GetAddressCitiesException;

            return Users.Select(user => user.AddressCity)
                .Where(city => !string.IsNullOrEmpty(city))
                .Distinct()
                .ToList();
        }
    }

    private sealed class UserExportServiceStub : IUserExportService
    {
        public const string ExportFilePath = @"C:\Exports\data.json";

        public int CallCount { get; private set; }
        public List<UserData> ExportedUsers { get; private set; } = [];

        public string ExportUsers(IEnumerable<UserData> users)
        {
            CallCount++;
            ExportedUsers = users.ToList();
            return ExportFilePath;
        }
    }
}

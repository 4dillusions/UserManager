/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Services;
using App4di.Dotnet.UserManager.Presentation.Users;
using App4di.Dotnet.UserManager.Presentation.ViewModels;

namespace App4di.Dotnet.UserManager.Tests.Presentation.ViewModels;

[TestClass]
public class UserViewModelTests
{
    private static readonly DateTime ValidBirthDate = BirthDateRules.MinimumBirthDate;
    private static readonly DateTime InvalidBirthDate = BirthDateRules.MinimumBirthDate.AddDays(-1);

    [TestMethod]
    public void HeaderTextReflectsAddAndEditMode()
    {
        var editSessionService = new UserEditSessionService();
        editSessionService.BeginAdd([]);
        var viewModel = CreateViewModel(new NavigationService(), new UserRepositoryStub(), editSessionService);

        Assert.AreEqual("Please Add a User", viewModel.HeaderText);

        var user = CreateUser(1, "Original");
        editSessionService.BeginEdit(user, [user]);

        Assert.AreEqual("Please Edit a User", viewModel.HeaderText);
    }

    [TestMethod]
    public void SaveInAddModePersistsAppendedUserAndNavigatesBack()
    {
        var existing = CreateUser(3, "Existing");
        var editSessionService = new UserEditSessionService();
        editSessionService.BeginAdd([existing]);
        editSessionService.EditingUser!.LoginName = "NewUser";
        editSessionService.EditingUser.BirthDate = ValidBirthDate;
        var repository = new UserRepositoryStub();
        var navigationService = new NavigationService();
        navigationService.Navigate(ViewType.User);
        var viewModel = CreateViewModel(navigationService, repository, editSessionService);

        viewModel.SaveCommand.Execute(null);

        Assert.HasCount(2, repository.SavedUsers!);
        Assert.AreEqual(4, repository.SavedUsers![1].UserId);
        Assert.AreEqual("NewUser", repository.SavedUsers[1].LoginName);
        Assert.IsNull(editSessionService.EditingUser);
        Assert.AreEqual(ViewType.UserList, navigationService.CurrentView);
    }

    [TestMethod]
    public void DuplicateLoginNameInAddModeDoesNotPersistOrNavigate()
    {
        var existing = CreateUser(1, "Existing");
        existing.LoginName = "Duplicate";
        var editSessionService = new UserEditSessionService();
        editSessionService.BeginAdd([existing]);
        editSessionService.EditingUser!.LoginName = "duplicate";
        var repository = new UserRepositoryStub();
        var navigationService = new NavigationService();
        navigationService.Navigate(ViewType.User);
        var userNotificationService = new UserNotificationServiceStub();
        var viewModel = CreateViewModel(navigationService, repository, editSessionService, userNotificationService);

        viewModel.SaveCommand.Execute(null);

        Assert.IsNull(repository.SavedUsers);
        Assert.IsNotNull(editSessionService.EditingUser);
        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
        Assert.AreEqual("Save error", userNotificationService.Title);
    }

    [TestMethod]
    public void InvalidBirthDateInAddModeDoesNotPersistOrNavigate()
    {
        var existing = CreateUser(1, "Existing");
        var editSessionService = new UserEditSessionService();
        editSessionService.BeginAdd([existing]);
        editSessionService.EditingUser!.LoginName = "NewUser";
        editSessionService.EditingUser.BirthDate = InvalidBirthDate;
        var repository = new UserRepositoryStub();
        var navigationService = new NavigationService();
        navigationService.Navigate(ViewType.User);
        var userNotificationService = new UserNotificationServiceStub();
        var viewModel = CreateViewModel(navigationService, repository, editSessionService, userNotificationService);

        viewModel.SaveCommand.Execute(null);

        Assert.IsNull(repository.SavedUsers);
        Assert.IsNotNull(editSessionService.EditingUser);
        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
        Assert.AreEqual("Save error", userNotificationService.Title);
    }

    [TestMethod]
    public void InvalidBirthDateInEditModeDoesNotPersistOrNavigate()
    {
        var original = CreateUser(1, "Original");
        var editSessionService = CreateEditSession(original, [original]);
        editSessionService.EditingUser!.BirthDate = BirthDateRules.MaximumBirthDate.AddDays(1);
        var repository = new UserRepositoryStub();
        var navigationService = new NavigationService();
        navigationService.Navigate(ViewType.User);
        var userNotificationService = new UserNotificationServiceStub();
        var viewModel = CreateViewModel(navigationService, repository, editSessionService, userNotificationService);

        viewModel.SaveCommand.Execute(null);

        Assert.IsNull(repository.SavedUsers);
        Assert.AreEqual(ValidBirthDate.AddDays(1), original.BirthDate);
        Assert.IsNotNull(editSessionService.EditingUser);
        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
        Assert.AreEqual("Save error", userNotificationService.Title);
    }

    [TestMethod]
    public void BirthDateRangePropertiesUseCentralRules()
    {
        var user = CreateUser(1, "Original");
        var viewModel = CreateViewModel(
            new NavigationService(),
            new UserRepositoryStub(),
            CreateEditSession(user, [user]));

        Assert.AreEqual(BirthDateRules.MinimumBirthDate, viewModel.MinimumBirthDate);
        Assert.AreEqual(BirthDateRules.MaximumBirthDate, viewModel.MaximumBirthDate);
    }

    [TestMethod]
    public void UserViewModelLoadsEditableCopyFromEditSession()
    {
        var original = CreateUser(1, "Original");
        var editSessionService = CreateEditSession(original, [original]);

        var viewModel = CreateViewModel(
            new NavigationService(),
            new UserRepositoryStub(),
            editSessionService);

        Assert.AreSame(editSessionService.EditingUser, viewModel.User);
        Assert.AreNotSame(original, viewModel.User);
    }

    [TestMethod]
    public void SaveCommandRequiresChangesAndNotifiesWhenEditStateChanges()
    {
        var original = CreateUser(1, "Original");
        var editSessionService = CreateEditSession(original, [original]);
        var viewModel = CreateViewModel(
            new NavigationService(),
            new UserRepositoryStub(),
            editSessionService);
        var saveCommand = viewModel.SaveCommand;
        var canExecuteChangedCount = 0;
        saveCommand.CanExecuteChanged += (_, _) => canExecuteChangedCount++;

        Assert.IsFalse(saveCommand.CanExecute(null));

        viewModel.User!.Surname = "Changed";

        Assert.IsTrue(saveCommand.CanExecute(null));

        viewModel.User.Surname = "Original";

        Assert.IsFalse(saveCommand.CanExecute(null));
        Assert.AreEqual(2, canExecuteChangedCount);
    }

    [TestMethod]
    public void CancelDiscardsChangesAndNavigatesBackWithoutPersisting()
    {
        var original = CreateUser(1, "Original");
        var editSessionService = CreateEditSession(original, [original]);
        var repository = new UserRepositoryStub();
        var navigationService = new NavigationService();
        navigationService.Navigate(ViewType.User);
        var viewModel = CreateViewModel(navigationService, repository, editSessionService);
        viewModel.User!.Surname = "Changed";

        viewModel.CancelCommand.Execute(null);

        Assert.AreEqual("Original", original.Surname);
        Assert.IsNull(repository.SavedUsers);
        Assert.IsNull(editSessionService.EditingUser);
        Assert.AreEqual(ViewType.UserList, navigationService.CurrentView);
    }

    [TestMethod]
    public void SaveUpdatesCorrectUserPersistsListAndNavigatesBack()
    {
        var firstUser = CreateUser(1, "First");
        var targetUser = CreateUser(2, "Original");
        var users = new List<User> { firstUser, targetUser };
        var selectedDifferentInstance = CreateUser(2, "Original");
        var editSessionService = CreateEditSession(selectedDifferentInstance, users);
        var repository = new UserRepositoryStub();
        var navigationService = new NavigationService();
        navigationService.Navigate(ViewType.User);
        var viewModel = CreateViewModel(navigationService, repository, editSessionService);
        viewModel.User!.Surname = "Changed";
        viewModel.User.AddressCity = "Szeged";

        Assert.AreEqual("Original", targetUser.Surname);

        viewModel.SaveCommand.Execute(null);

        Assert.AreEqual("First", firstUser.Surname);
        Assert.AreEqual("Changed", targetUser.Surname);
        Assert.AreEqual("Szeged", targetUser.AddressCity);
        Assert.AreEqual("Changed", selectedDifferentInstance.Surname);
        Assert.AreEqual("Szeged", selectedDifferentInstance.AddressCity);
        Assert.HasCount(2, repository.SavedUsers!);
        Assert.AreEqual("Changed", repository.SavedUsers![1].Surname);
        Assert.IsNull(editSessionService.EditingUser);
        Assert.AreEqual(ViewType.UserList, navigationService.CurrentView);
    }

    [TestMethod]
    public void FailedSaveKeepsEditSessionAndDoesNotNavigateOrMutateOriginal()
    {
        var original = CreateUser(1, "Original");
        var editSessionService = CreateEditSession(original, [original]);
        var repository = new UserRepositoryStub { SaveException = new IOException("Save failed") };
        var navigationService = new NavigationService();
        navigationService.Navigate(ViewType.User);
        var userNotificationService = new UserNotificationServiceStub();
        var viewModel = CreateViewModel(navigationService, repository, editSessionService, userNotificationService);
        viewModel.User!.Surname = "Changed";

        viewModel.SaveCommand.Execute(null);

        Assert.AreEqual("Original", original.Surname);
        Assert.AreEqual("Changed", editSessionService.EditingUser?.Surname);
        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
        Assert.AreEqual("Save error", userNotificationService.Title);
    }

    private static UserViewModel CreateViewModel(
        INavigationService navigationService,
        IUserRepository repository,
        IUserEditSessionService editSessionService,
        IUserNotificationService? userNotificationService = null)
    {
        return new UserViewModel(
            navigationService,
            userNotificationService ?? new UserNotificationServiceStub(),
            new SaveUserUseCase(repository),
            editSessionService);
    }

    private static UserEditSessionService CreateEditSession(User selectedUser, List<User> users)
    {
        var editSessionService = new UserEditSessionService();
        editSessionService.BeginEdit(selectedUser, users);
        return editSessionService;
    }

    private static User CreateUser(int userId, string surname)
    {
        return new User
        {
            UserId = userId,
            LoginName = $"User{userId}",
            Password = $"Password{userId}",
            FirstName = $"First{userId}",
            Surname = surname,
            BirthDate = ValidBirthDate.AddDays(userId),
            BirthPlace = "Budapest",
            AddressCity = "Budapest"
        };
    }

    private sealed class UserRepositoryStub : IUserRepository
    {
        public List<UserData>? SavedUsers { get; private set; }
        public Exception? SaveException { get; init; }

        public IReadOnlyList<UserData> LoadUsers()
        {
            throw new NotSupportedException();
        }

        public void SaveUsers(IEnumerable<UserData> users)
        {
            if (SaveException != null)
                throw SaveException;

            SavedUsers = users.ToList();
        }
    }

    private sealed class UserNotificationServiceStub : IUserNotificationService
    {
        public string? Title { get; private set; }

        public void ShowMessage(string message, string? title = null)
        {
            Title = title;
        }

        public bool ShowConfirmation(string message, string? title = null)
        {
            return false;
        }
    }
}

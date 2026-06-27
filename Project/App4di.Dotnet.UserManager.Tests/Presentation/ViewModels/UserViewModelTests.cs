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
        var messageService = new MessageServiceStub();
        var viewModel = CreateViewModel(navigationService, repository, editSessionService, messageService);
        viewModel.User!.Surname = "Changed";

        viewModel.SaveCommand.Execute(null);

        Assert.AreEqual("Original", original.Surname);
        Assert.AreEqual("Changed", editSessionService.EditingUser?.Surname);
        Assert.AreEqual(ViewType.User, navigationService.CurrentView);
        Assert.AreEqual("Save error", messageService.Title);
    }

    private static UserViewModel CreateViewModel(
        INavigationService navigationService,
        IUserRepository repository,
        IUserEditSessionService editSessionService,
        IMessageService? messageService = null)
    {
        return new UserViewModel(
            navigationService,
            messageService ?? new MessageServiceStub(),
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
            BirthDate = new DateTime(2000, 1, userId),
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

    private sealed class MessageServiceStub : IMessageService
    {
        public string? Title { get; private set; }

        public void ShowMessage(string message, string? title = null)
        {
            Title = title;
        }
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
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
            new MainViewModel(),
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
        var mainViewModel = new MainViewModel { ViewType = ViewType.User };
        var viewModel = CreateViewModel(mainViewModel, repository, editSessionService);
        viewModel.User!.Surname = "Changed";

        viewModel.CancelCommand.Execute(null);

        Assert.AreEqual("Original", original.Surname);
        Assert.IsNull(repository.SavedUsers);
        Assert.IsNull(editSessionService.EditingUser);
        Assert.AreEqual(ViewType.UserList, mainViewModel.ViewType);
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
        var mainViewModel = new MainViewModel { ViewType = ViewType.User };
        var viewModel = CreateViewModel(mainViewModel, repository, editSessionService);
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
        Assert.AreEqual(ViewType.UserList, mainViewModel.ViewType);
    }

    private static UserViewModel CreateViewModel(
        MainViewModel mainViewModel,
        IUserRepository repository,
        IUserEditSessionService editSessionService)
    {
        return new UserViewModel(
            mainViewModel,
            new MessageServiceStub(),
            repository,
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

        public List<UserData> LoadUsers()
        {
            throw new NotSupportedException();
        }

        public void SaveUsers(List<UserData> users)
        {
            SavedUsers = users;
        }
    }

    private sealed class MessageServiceStub : IMessageService
    {
        public void ShowMessage(string message, string? title = null)
        {
        }
    }
}

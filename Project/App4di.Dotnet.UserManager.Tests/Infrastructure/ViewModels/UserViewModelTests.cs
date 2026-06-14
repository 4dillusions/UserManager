/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Session;
using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Navigation;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using App4di.Dotnet.UserManager.Infrastructure.Service;
using App4di.Dotnet.UserManager.Infrastructure.ViewModels;

namespace App4di.Dotnet.UserManager.Tests.Infrastructure.ViewModels;

[TestClass]
public class UserViewModelTests
{
    [TestMethod]
    public void UserViewModelLoadsEditingUserFromSession()
    {
        var editingUser = new User { UserId = 1 };
        var sessionService = new SessionService { CurrentUser = editingUser };

        var viewModel = new UserViewModel(
            new MainViewModel(),
            new MessageServiceStub(),
            new UserRepositoryStub(),
            sessionService);

        Assert.AreSame(editingUser, viewModel.User);
    }

    [TestMethod]
    public void SaveCommandPersistsSessionUserListAndNavigatesBack()
    {
        var users = new List<User> { new() { UserId = 1 } };
        var sessionService = new SessionService
        {
            CurrentUser = users[0],
            CurrentUsers = users
        };
        var repository = new UserRepositoryStub();
        var mainViewModel = new MainViewModel { ViewType = ViewType.User };
        var viewModel = new UserViewModel(
            mainViewModel,
            new MessageServiceStub(),
            repository,
            sessionService);

        viewModel.SaveCommand.Execute(null);

        Assert.AreSame(users, repository.SavedUsers);
        Assert.AreEqual(ViewType.UserList, mainViewModel.ViewType);
    }

    private sealed class UserRepositoryStub : IUserRepository
    {
        public List<User>? SavedUsers { get; private set; }

        public List<User> LoadUsers()
        {
            throw new NotSupportedException();
        }

        public void SaveUsers(List<User> users)
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

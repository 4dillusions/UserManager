/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Authentication;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Services;
using App4di.Dotnet.UserManager.Presentation.ViewModels;

namespace App4di.Dotnet.UserManager.Tests.Presentation.ViewModels;

[TestClass]
public class LoginViewModelTests
{
    [TestMethod]
    public void LoginCommandCallsAuthenticationService()
    {
        var authenticationService = new AuthenticationServiceStub(true);
        var navigationService = new NavigationService();
        var viewModel = new LoginViewModel(
            navigationService,
            new MessageServiceStub(),
            new ApplicationServiceStub(),
            authenticationService)
        {
            LoginName = "Albert",
            Password = "Albert1"
        };

        viewModel.LoginCommand.Execute(null);

        Assert.AreEqual(1, authenticationService.CallCount);
        Assert.AreEqual("Albert", authenticationService.LoginName);
        Assert.AreEqual("Albert1", authenticationService.Password);
        Assert.AreEqual(ViewType.UserList, navigationService.CurrentView);
    }

    [TestMethod]
    public void FailedLoginDoesNotNavigate()
    {
        var navigationService = new NavigationService();
        var viewModel = new LoginViewModel(
            navigationService,
            new MessageServiceStub(),
            new ApplicationServiceStub(),
            new AuthenticationServiceStub(false))
        {
            LoginName = "Albert",
            Password = "WrongPassword"
        };

        viewModel.LoginCommand.Execute(null);

        Assert.AreEqual(ViewType.Login, navigationService.CurrentView);
    }

    private sealed class AuthenticationServiceStub(bool result) : IAuthenticationService
    {
        public int CallCount { get; private set; }
        public string? LoginName { get; private set; }
        public string? Password { get; private set; }

        public bool Authenticate(string loginName, string password)
        {
            CallCount++;
            LoginName = loginName;
            Password = password;
            return result;
        }
    }

    private sealed class MessageServiceStub : IMessageService
    {
        public void ShowMessage(string message, string? title = null)
        {
        }
    }

    private sealed class ApplicationServiceStub : IApplicationService
    {
        public void Shutdown()
        {
        }
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Authentication;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Services;
using FW4di.Dotnet.MVVM;

namespace App4di.Dotnet.UserManager.Presentation.ViewModels;

public class LoginViewModel : NotificationObject
{
    private bool isCanLogin = true;
    private readonly INavigationService navigationService;
    private readonly IMessageService messageService;
    private readonly IApplicationService applicationService;
    private readonly IAuthenticationService authenticationService;

    public LoginViewModel(
        INavigationService navigationService,
        IMessageService messageService,
        IApplicationService applicationService,
        IAuthenticationService authenticationService)
    {
        this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        this.applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
        this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    public string LoginName
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;

    public string Password
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;

    private RelayCommand? loginCommand;
    public FW4di.Dotnet.MVVM.ICommand LoginCommand
    {
        get
        {
            loginCommand ??= new RelayCommand(_ => Login(), _ => CanLogin());
            return loginCommand;
        }
    }

    private void Login()
    {
        try
        {
            isCanLogin = false;

            if (authenticationService.Authenticate(LoginName, Password))
                navigationService.Navigate(ViewType.UserList);
            else
                messageService.ShowMessage("Wrong LoginName or Password!");
        }
        catch (Exception ex)
        {
            messageService.ShowMessage(ex.Message + "\n " + ex.InnerException, "Refresh error");
        }
        finally
        {
            isCanLogin = true;
        }
    }

    private bool CanLogin()
    {
        return isCanLogin;
    }

    private RelayCommand? cancelCommand;
    public FW4di.Dotnet.MVVM.ICommand CancelCommand
    {
        get
        {
            cancelCommand ??= new RelayCommand(_ => Cancel(), _ => CanCancel());
            return cancelCommand;
        }
    }

    private void Cancel()
    {
        applicationService.Shutdown();
    }

    private bool CanCancel()
    {
        return true;
    }
}

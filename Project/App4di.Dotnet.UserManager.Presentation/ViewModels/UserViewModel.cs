/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Services;
using App4di.Dotnet.UserManager.Presentation.Users;
using FW4di.Dotnet.MVVM;

namespace App4di.Dotnet.UserManager.Presentation.ViewModels;

public class UserViewModel : NotificationObject
{
    private readonly INavigationService navigationService;
    private readonly IMessageService messageService;
    private readonly ISaveUserUseCase saveUserUseCase;
    private readonly IUserEditSessionService userEditSessionService;

    public User? User => userEditSessionService.EditingUser;

    public UserViewModel(
        INavigationService navigationService,
        IMessageService messageService,
        ISaveUserUseCase saveUserUseCase,
        IUserEditSessionService userEditSessionService)
    {
        this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        this.saveUserUseCase = saveUserUseCase ?? throw new ArgumentNullException(nameof(saveUserUseCase));
        this.userEditSessionService = userEditSessionService ?? throw new ArgumentNullException(nameof(userEditSessionService));
    }

    private RelayCommand? saveCommand;
    public FW4di.Dotnet.MVVM.ICommand SaveCommand
    {
        get
        {
            saveCommand ??= new RelayCommand(_ => Save(), _ => CanSave());
            return saveCommand;
        }
    }

    private void Save()
    {
        try
        {
            saveUserUseCase.Execute(userEditSessionService);
            navigationService.Navigate(ViewType.UserList);
        }
        catch (Exception ex)
        {
            messageService.ShowMessage(ex.Message + "\n " + ex.InnerException, "Save error");
        }
    }

    private bool CanSave()
    {
        return true;
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
        userEditSessionService.Cancel();
        navigationService.Navigate(ViewType.UserList);
    }

    private bool CanCancel()
    {
        return true;
    }
}

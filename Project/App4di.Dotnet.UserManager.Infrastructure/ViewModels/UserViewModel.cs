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
using FW4di.Dotnet.MVVM;

namespace App4di.Dotnet.UserManager.Infrastructure.ViewModels;

public class UserViewModel : NotificationObject
{
    private MainViewModel mainViewModel;
    private readonly IMessageService messageService;
    private readonly IUserRepository userRepository;
    private readonly ISessionService sessionService;

    public User? User
    {
        get;
        set => SetProperty(ref field, value);
    }

    public UserViewModel(
        MainViewModel mainViewModel,
        IMessageService messageService,
        IUserRepository userRepository,
        ISessionService sessionService)
    {
        this.mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        User = sessionService.CurrentUser;
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
            if (sessionService.CurrentUsers != null)
                userRepository.SaveUsers(sessionService.CurrentUsers);
            mainViewModel.ViewType = ViewType.UserList;
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
        mainViewModel.ViewType = ViewType.UserList;
    }

    private bool CanCancel()
    {
        return true;
    }
}

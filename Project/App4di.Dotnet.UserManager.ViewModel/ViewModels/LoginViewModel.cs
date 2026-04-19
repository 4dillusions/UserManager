/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Model.Entities;
using App4di.Dotnet.UserManager.ViewModel.Navigation;
using FW4di.Dotnet.MVVM;
using System.Windows;

namespace App4di.Dotnet.UserManager.ViewModel.ViewModels;

public class LoginViewModel : NotificationObject
{
    #region Fields
    private bool isCanLogin = true;

    private readonly MainViewModel mainViewModel;
    #endregion

    #region Constructor
    public LoginViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
    }
    #endregion

    #region Properties
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
    #endregion

    #region Commands
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

            if (User.IsUserExist(LoginName, Password))
                mainViewModel.ViewType = ViewType.UserList;
            else
                MessageBox.Show("Wrong LoginName or Password!");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message + "\n " + ex.InnerException, "Refresh error");
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
        Application.Current.Shutdown();
    }

    private bool CanCancel()
    {
        return true;
    }
    #endregion
}

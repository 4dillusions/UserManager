/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Windows;
using System.Windows.Input;
using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Factory;
using App4di.Dotnet.UserManager.Model;
using App4di.Dotnet.UserManager.ViewModel.ViewManagement;

namespace App4di.Dotnet.UserManager.ViewModel;

public class LoginViewModel : NotificationObject
{
    #region Fields
    private bool isCanLogin = true;

    private string loginName = string.Empty;
    private string password = string.Empty;
    #endregion

    #region Properties
    public string LoginName
    {
        get { return loginName; }
        set
        {
            loginName = value;
            NotifyPropertyChanged("LoginName");
        }
    }

    public string Password
    {
        get { return password; }
        set
        {
            password = value;
            NotifyPropertyChanged("Password");
        }
    }
    #endregion

    #region Commands
    private RelayCommand? loginCommand;
    public ICommand LoginCommand
    {
        get
        {
            loginCommand ??= new RelayCommand(Login, CanLogin);
            return loginCommand;
        }
    }

    private void Login()
    {
        try
        {
            isCanLogin = false;

            if (User.IsUserExist(LoginName, Password))
                Ioc<MainViewModel>.Instance.ViewType = ViewType.UserList;
            else
                MessageBox.Show("Wrong LoginName or Password!");

            isCanLogin = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Refresh error", ex.Message + "\n " + ex.InnerException);
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
    public ICommand CancelCommand
    {
        get
        {
            cancelCommand ??= new RelayCommand(Cancel, CanCancel);
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

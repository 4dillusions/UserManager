/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Model.Entities;
using App4di.Dotnet.UserManager.ViewModel.Navigation;
using System.Windows;
using System.Windows.Input;

namespace App4di.Dotnet.UserManager.ViewModel.ViewModels;

public class LoginViewModel : NotificationObject
{
    #region Fields
    private bool isCanLogin = true;

    private string loginName = string.Empty;
    private string password = string.Empty;

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
        get { return loginName; }
        set
        {
            loginName = value;
            NotifyPropertyChanged();
        }
    }

    public string Password
    {
        get { return password; }
        set
        {
            password = value;
            NotifyPropertyChanged();
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

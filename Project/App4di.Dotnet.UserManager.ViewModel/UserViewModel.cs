/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Windows.Input;
using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Factory;
using App4di.Dotnet.UserManager.Model;
using App4di.Dotnet.UserManager.ViewModel.ViewManagement;

namespace App4di.Dotnet.UserManager.ViewModel;

public class UserViewModel : NotificationObject
{
    #region Fields
    private User? user;
    private string? userValidator;
    #endregion

    #region Properties
    public User? User
    {
        get { return user; }

        set
        {
            user = value;
            NotifyPropertyChanged("User");
        }
    }

    public string? UserValidator
    {
        get { return userValidator; }

        set
        {
            userValidator = value;
            NotifyPropertyChanged("UserValidator");
        }
    }
    #endregion

    #region Constructor
    public UserViewModel()
    {
        UserValidator = null;
        user = Model.User.CurrentUser;
    }
    #endregion

    #region Commands
    private RelayCommand? saveCommand;
    public ICommand SaveCommand
    {
        get
        {
            saveCommand ??= new RelayCommand(Save, CanSave);
            return saveCommand;
        }
    }

    private void Save()
    {
        UserList.SaveCurrentUsers();
        Ioc<MainViewModel>.Instance.ViewType = ViewType.UserList;
    }

    private bool CanSave()
    {
        return UserValidator == null;
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
        Ioc<MainViewModel>.Instance.ViewType = ViewType.UserList;
    }

    private bool CanCancel()
    {
        return true;
    }
    #endregion
}

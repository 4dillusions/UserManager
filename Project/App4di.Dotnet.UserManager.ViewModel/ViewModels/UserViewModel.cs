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

public class UserViewModel : NotificationObject
{
    #region Fields
    private User? user;
    private MainViewModel mainViewModel;
    #endregion

    #region Properties
    public User? User
    {
        get { return user; }

        set
        {
            user = value;
            NotifyPropertyChanged();
        }
    }
    #endregion

    #region Constructor
    public UserViewModel(MainViewModel mainViewModel)
    {
        user = User.CurrentUser;
        this.mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
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
        try
        {
            UserList.SaveCurrentUsers();
            mainViewModel.ViewType = ViewType.UserList;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message + "\n " + ex.InnerException, "Save error");
        }
    }

    private bool CanSave()
    {
        return true;
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
        mainViewModel.ViewType = ViewType.UserList;
    }

    private bool CanCancel()
    {
        return true;
    }
    #endregion
}

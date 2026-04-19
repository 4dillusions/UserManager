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

public class UserViewModel : NotificationObject
{
    #region Fields
    private MainViewModel mainViewModel;
    #endregion

    #region Properties
    public User? User
    {
        get;
        set => SetProperty(ref field, value);
    }
    #endregion

    #region Constructor
    public UserViewModel(MainViewModel mainViewModel)
    {
        User = User.CurrentUser;
        this.mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
    }
    #endregion

    #region Commands
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
    #endregion
}

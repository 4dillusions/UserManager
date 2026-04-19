/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Data;
using App4di.Dotnet.UserManager.Core.Factory;
using App4di.Dotnet.UserManager.Model.Entities;
using App4di.Dotnet.UserManager.ViewModel.Navigation;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace App4di.Dotnet.UserManager.ViewModel.ViewModels;

public class UserListViewModel : NotificationObject
{
    #region Fields
    private AddressCity? selectedAddressCity;
    private ObservableCollection<AddressCity> addressCities = [];

    private User? selectedUser;
    private ObservableCollection<User> users = [];

    private UserFilter filter = new();
    #endregion

    #region Methods
    public UserListViewModel()
    {
        Reset();
    }

    private void Reset()
    {
        filter = new UserFilter();

        AddressCities = new AddressCityList().Cities;
        SelectedAddressCity = AddressCities.FirstOrDefault();

        Users = new UserList(filter).Users;
        if (User.CurrentUser == null)
            SelectedUser = Users.FirstOrDefault();
        else
            SelectedUser = Users.FirstOrDefault(u => u.UserId == User.CurrentUser.UserId) ?? Users.FirstOrDefault();
    }
    #endregion

    #region Properties
    public AddressCity? SelectedAddressCity
    {
        get { return selectedAddressCity; }
        set
        {
            selectedAddressCity = value;
            filter.AddressCity = value?.CityName ?? AddressCity.DefaultName;
            NotifyPropertyChanged();
        }
    }

    public ObservableCollection<AddressCity> AddressCities
    {
        get { return addressCities; }
        set
        {
            addressCities = value;
            NotifyPropertyChanged();
        }
    }

    public string TextInAll
    {
        get { return filter.TextInAll; }
        set
        {
            filter.TextInAll = value;
            NotifyPropertyChanged();
        }
    }

    public User? SelectedUser
    {
        get { return selectedUser; }
        set
        {
            User.CurrentUser = value;

            selectedUser = value;
            NotifyPropertyChanged();
        }
    }

    public ObservableCollection<User> Users
    {
        get { return users; }
        set
        {
            users = value;
            NotifyPropertyChanged();
        }
    }
    #endregion

    #region Commands
    private RelayCommand? findCommand;
    public ICommand FindCommand
    {
        get
        {
            findCommand ??= new RelayCommand(Find, CanFind);
            return findCommand;
        }
    }

    private void Find()
    {
        try
        {
            Users = new UserList(filter).Users;
            SelectedUser = Users.FirstOrDefault();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message + "\n " + ex.InnerException, "Find error");
        }
    }

    private bool CanFind()
    {
        return true;
    }

    private RelayCommand? editCommand;
    public ICommand EditCommand
    {
        get
        {
            editCommand ??= new RelayCommand(Edit, CanEdit);
            return editCommand;
        }
    }

    private void Edit()
    {
        if (SelectedUser != null)
        {
            UserList.CurrentUsers = new UserList(new UserFilter()).Users;
            Ioc<MainViewModel>.Instance.ViewType = ViewType.User;
        }
    }

    private bool CanEdit()
    {
        return true;
    }

    private RelayCommand? exportCommand;
    public ICommand ExportCommand
    {
        get
        {
            exportCommand ??= new RelayCommand(Export, CanExport);
            return exportCommand;
        }
    }

    private void Export()
    {
        try
        {
            IDataManager<User> dataManager = new JsonDataManager<User>();

            if (File.Exists(Constants.JsonDataFilePath))
                File.Delete(Constants.JsonDataFilePath);

            dataManager.Save(users.ToList(), Constants.JsonDataFilePath);

            if (File.Exists(Constants.JsonDataFilePath))
                MessageBox.Show("Data exported to Json file");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message + "\n " + ex.InnerException, "Export error");
        }
    }

    private bool CanExport()
    {
        return true;
    }
    #endregion
}

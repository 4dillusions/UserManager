/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;
using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Navigation;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using App4di.Dotnet.UserManager.Infrastructure.Service;
using FW4di.Dotnet.MVVM;
using System.Collections.ObjectModel;
using System.IO;

namespace App4di.Dotnet.UserManager.Infrastructure.ViewModels;

public class UserListViewModel : NotificationObject
{
    private AddressCity? selectedAddressCity;
    private User? selectedUser;
    private UserFilter filter = new();
    private MainViewModel mainViewModel;
    private readonly IMessageService messageService;
    private readonly IUserRepository userRepository;
    private readonly IAddressCityRepository addressCityRepository;

    public UserListViewModel(
        MainViewModel mainViewModel,
        IMessageService messageService,
        IUserRepository userRepository,
        IAddressCityRepository addressCityRepository)
    {
        this.mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.addressCityRepository = addressCityRepository ?? throw new ArgumentNullException(nameof(addressCityRepository));
        Reset();
    }

    private void Reset()
    {
        filter = new UserFilter();

        AddressCities = new AddressCityList(addressCityRepository).Cities;
        SelectedAddressCity = AddressCities.FirstOrDefault();

        Users = new UserList(filter, userRepository).Users;
        if (User.CurrentUser == null)
            SelectedUser = Users.FirstOrDefault();
        else
            SelectedUser = Users.FirstOrDefault(u => u.UserId == User.CurrentUser.UserId) ?? Users.FirstOrDefault();
    }

    public AddressCity? SelectedAddressCity
    {
        get { return selectedAddressCity; }
        set
        {
            if (!SetProperty(ref selectedAddressCity, value))
                return;

            filter.AddressCity = value?.CityName ?? AddressCity.DefaultName;
        }
    }

    public ObservableCollection<AddressCity> AddressCities
    {
        get;
        set => SetProperty(ref field, value);
    } = [];

    public string TextInAll
    {
        get { return filter.TextInAll; }
        set
        {
            filter.TextInAll = value;
            RaisePropertyChanged();
        }
    }

    public User? SelectedUser
    {
        get { return selectedUser; }
        set
        {
            if (EqualityComparer<User?>.Default.Equals(selectedUser, value))
                return;

            User.CurrentUser = value;
            selectedUser = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<User> Users
    {
        get;
        set => SetProperty(ref field, value);
    } = [];

    private RelayCommand? findCommand;
    public FW4di.Dotnet.MVVM.ICommand FindCommand
    {
        get
        {
            findCommand ??= new RelayCommand(_ => Find(), _ => CanFind());
            return findCommand;
        }
    }

    private void Find()
    {
        try
        {
            Users = new UserList(filter, userRepository).Users;
            SelectedUser = Users.FirstOrDefault();
        }
        catch (Exception ex)
        {
            messageService.ShowMessage(ex.Message + "\n " + ex.InnerException, "Find error");
        }
    }

    private bool CanFind()
    {
        return true;
    }

    private RelayCommand? editCommand;
    public FW4di.Dotnet.MVVM.ICommand EditCommand
    {
        get
        {
            editCommand ??= new RelayCommand(_ => Edit(), _ => CanEdit());
            return editCommand;
        }
    }

    private void Edit()
    {
        if (SelectedUser != null)
        {
            UserList.CurrentUsers = new UserList(new UserFilter(), userRepository).Users;
            mainViewModel.ViewType = ViewType.User;
        }
    }

    private bool CanEdit()
    {
        return true;
    }

    private RelayCommand? exportCommand;
    public FW4di.Dotnet.MVVM.ICommand ExportCommand
    {
        get
        {
            exportCommand ??= new RelayCommand(_ => Export(), _ => CanExport());
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

            dataManager.Save(Users.ToList(), Constants.JsonDataFilePath);

            if (File.Exists(Constants.JsonDataFilePath))
                messageService.ShowMessage("Data exported to Json file");
        }
        catch (Exception ex)
        {
            messageService.ShowMessage(ex.Message + "\n " + ex.InnerException, "Export error");
        }
    }

    private bool CanExport()
    {
        return true;
    }
}

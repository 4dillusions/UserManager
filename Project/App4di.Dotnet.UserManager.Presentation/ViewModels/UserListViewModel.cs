/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Export;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Presentation.Common;
using App4di.Dotnet.UserManager.Presentation.Mapping;
using App4di.Dotnet.UserManager.Presentation.Models;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Services;
using App4di.Dotnet.UserManager.Presentation.Session;
using App4di.Dotnet.UserManager.Presentation.Users;
using FW4di.Dotnet.MVVM;
using System.Collections.ObjectModel;

namespace App4di.Dotnet.UserManager.Presentation.ViewModels;

public class UserListViewModel : NotificationObject
{
    private AddressCity? selectedAddressCity;
    private User? selectedUser;
    private UserFilter filter = new();
    private readonly INavigationService navigationService;
    private readonly IMessageService messageService;
    private readonly IUserQueryService userQueryService;
    private readonly IUserExportService userExportService;
    private readonly ISessionService sessionService;
    private readonly IUserEditSessionService userEditSessionService;

    public UserListViewModel(
        INavigationService navigationService,
        IMessageService messageService,
        IUserQueryService userQueryService,
        IUserExportService userExportService,
        ISessionService sessionService,
        IUserEditSessionService userEditSessionService)
    {
        this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        this.userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
        this.userExportService = userExportService ?? throw new ArgumentNullException(nameof(userExportService));
        this.sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        this.userEditSessionService = userEditSessionService ?? throw new ArgumentNullException(nameof(userEditSessionService));
        Reset();
    }

    private void Reset()
    {
        filter = new UserFilter();

        AddressCities = new ObservableCollection<AddressCity> { new() };
        foreach (var cityName in userQueryService.GetAddressCities())
            AddressCities.Add(new AddressCity { CityName = cityName });
        SelectedAddressCity = AddressCities.FirstOrDefault();

        Users = LoadUsers(filter);
        if (sessionService.SelectedUser == null)
            SelectedUser = Users.FirstOrDefault();
        else
            SelectedUser = Users.FirstOrDefault(u => u.UserId == sessionService.SelectedUser.UserId) ?? Users.FirstOrDefault();
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
            clearSearchCommand?.RaiseCanExecuteChanged();
        }
    }

    public User? SelectedUser
    {
        get { return selectedUser; }
        set
        {
            if (EqualityComparer<User?>.Default.Equals(selectedUser, value))
                return;

            sessionService.SelectedUser = value;
            selectedUser = value;
            RaisePropertyChanged();
            editCommand?.RaiseCanExecuteChanged();
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
            Users = LoadUsers(filter);
            SelectedUser = Users.FirstOrDefault();
        }
        catch (Exception ex)
        {
            messageService.ShowMessage(ExceptionMessageFormatter.Format(ex), "Find error");
        }
    }

    private bool CanFind()
    {
        return true;
    }

    private RelayCommand? clearSearchCommand;
    public FW4di.Dotnet.MVVM.ICommand ClearSearchCommand
    {
        get
        {
            clearSearchCommand ??= new RelayCommand(_ => ClearSearch(), _ => CanClearSearch());
            return clearSearchCommand;
        }
    }

    private void ClearSearch()
    {
        TextInAll = string.Empty;
        Find();
    }

    private bool CanClearSearch()
    {
        return !string.IsNullOrEmpty(TextInAll);
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
            userEditSessionService.BeginEdit(SelectedUser, LoadUsers(new UserFilter()).ToList());
            navigationService.Navigate(ViewType.User);
        }
    }

    private bool CanEdit()
    {
        return SelectedUser != null;
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
            var exportFilePath = userExportService.ExportUsers(Users.Select(UserMapper.ToUserData));
            messageService.ShowMessage($"Data exported to JSON file:{Environment.NewLine}{exportFilePath}");
        }
        catch (Exception ex)
        {
            messageService.ShowMessage(ExceptionMessageFormatter.Format(ex), "Export error");
        }
    }

    private bool CanExport()
    {
        return true;
    }

    private ObservableCollection<User> LoadUsers(UserFilter userFilter)
    {
        var criteria = new UserQueryCriteria
        {
            AddressCity = userFilter.AddressCity,
            TextInAll = userFilter.TextInAll
        };

        return new ObservableCollection<User>(userQueryService.GetUsers(criteria).Select(UserMapper.ToUser));
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Users;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using System.Collections.ObjectModel;

namespace App4di.Dotnet.UserManager.Infrastructure.Entities;

public class UserList
{
    private ObservableCollection<User> users = [];

    public ObservableCollection<User> Users
    {
        get { return users; }
        set { users = value; }
    }

    public static ObservableCollection<User>? CurrentUsers { get; set; }

    /// <summary> Mocking </summary>
    public UserList()
    {
        Users = new ObservableCollection<User>();

        Users.Add(new User
        {
            UserId = 1,
            LoginName = "Albert",
            Password = "albert",
            FirstName = "Albert",
            Surname = "Einstein",
            BirthDate = new System.DateTime(2879, 3, 14),
            BirthPlace = "German",
            AddressCity = "Württemberg"
        });

        Users.Add(new User
        {
            UserId = 2,
            LoginName = "Zoltan",
            Password = "zoltan",
            FirstName = "Zoltan",
            Surname = "Kodaly",
            BirthDate = new System.DateTime(1882, 12, 16),
            BirthPlace = "Hungary",
            AddressCity = "Kecskemet"
        });

        Users.Add(new User
        {
            UserId = 3,
            LoginName = "Erno",
            Password = "erno",
            FirstName = "Erno",
            Surname = "Rubik",
            BirthDate = new System.DateTime(1944, 7, 13),
            BirthPlace = "Hungary",
            AddressCity = "Budapest"
        });
    }

    /// <summary> Usefilter without mock </summary>
    public UserList(UserFilter filter)
        : this(filter, CreateQueryService())
    {
    }

    public UserList(UserFilter filter, IUserRepository userRepository)
        : this(filter, new UserQueryService(userRepository, new XmlAddressCityRepository(userRepository)))
    {
    }

    public UserList(UserFilter filter, IUserQueryService userQueryService)
    {
        ArgumentNullException.ThrowIfNull(filter);
        ArgumentNullException.ThrowIfNull(userQueryService);

        users = new ObservableCollection<User>(userQueryService.GetUsers(filter));
    }

    public static void SaveCurrentUsers()
    {
        SaveCurrentUsers(new XmlUserRepository());
    }

    public static void SaveCurrentUsers(IUserRepository userRepository)
    {
        if (CurrentUsers == null)
            return;

        ArgumentNullException.ThrowIfNull(userRepository);
        userRepository.SaveUsers(CurrentUsers.ToList());
    }

    private static IUserQueryService CreateQueryService()
    {
        IUserRepository userRepository = new XmlUserRepository();
        return new UserQueryService(userRepository, new XmlAddressCityRepository(userRepository));
    }
}

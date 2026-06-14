/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Entities;

namespace App4di.Dotnet.UserManager.Infrastructure.Mapping;

public static class UserMapper
{
    public static User ToUser(UserData source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var destination = new User();
        Copy(source, destination);
        return destination;
    }

    public static UserData ToUserData(User source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new UserData
        {
            UserId = source.UserId,
            LoginName = source.LoginName,
            Password = source.Password,
            FirstName = source.FirstName,
            Surname = source.Surname,
            BirthDate = source.BirthDate,
            BirthPlace = source.BirthPlace,
            AddressCity = source.AddressCity
        };
    }

    public static User Copy(User source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var destination = new User();
        Copy(source, destination);
        return destination;
    }

    public static void Copy(User source, User destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.UserId = source.UserId;
        destination.LoginName = source.LoginName;
        destination.Password = source.Password;
        destination.FirstName = source.FirstName;
        destination.Surname = source.Surname;
        destination.BirthDate = source.BirthDate;
        destination.BirthPlace = source.BirthPlace;
        destination.AddressCity = source.AddressCity;
    }

    private static void Copy(UserData source, User destination)
    {
        destination.UserId = source.UserId;
        destination.LoginName = source.LoginName;
        destination.Password = source.Password;
        destination.FirstName = source.FirstName;
        destination.Surname = source.Surname;
        destination.BirthDate = source.BirthDate;
        destination.BirthPlace = source.BirthPlace;
        destination.AddressCity = source.AddressCity;
    }
}

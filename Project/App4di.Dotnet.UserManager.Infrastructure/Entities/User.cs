/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Authentication;
using App4di.Dotnet.UserManager.Infrastructure.DTO;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using FW4di.Dotnet.MVVM;
using System.Runtime.CompilerServices;

namespace App4di.Dotnet.UserManager.Infrastructure.Entities;

[Serializable]
public class User : NotificationObject
{
    private UserDTO data;

    public User()
    {
        data = new UserDTO
        {
            LoginName = string.Empty,
            Password = string.Empty,
            FirstName = string.Empty,
            Surname = string.Empty,
            BirthPlace = string.Empty,
            AddressCity = string.Empty
        };
    }

    [Obsolete("Use IAuthenticationService.Authenticate instead.")]
    public static bool IsUserExist(string loginName, string password)
    {
        return new AuthenticationService(new XmlUserRepository()).Authenticate(loginName, password);
    }

    [Obsolete("Use IAuthenticationService.Authenticate instead.")]
    public static bool IsUserExist(string loginName, string password, IUserRepository userRepository)
    {
        return new AuthenticationService(userRepository).Authenticate(loginName, password);
    }

    public override string ToString()
    {
        return UserId + LoginName + Password + FirstName + Surname + BirthDate + BirthPlace + AddressCity;
    }

    private void SetDataProperty<T>(T value, Func<T> getter, Action<T> setter, [CallerMemberName] string propertyName = null!)
    {
        if (EqualityComparer<T>.Default.Equals(getter(), value))
            return;

        setter(value);
        RaisePropertyChanged(propertyName);
    }

    #region Properties
    public int UserId
    {
        get { return data.UserId; }
        set => SetDataProperty(value, () => data.UserId, v => data.UserId = v);
    }

    public string LoginName
    {
        get { return data.LoginName; }
        set => SetDataProperty(value, () => data.LoginName, v => data.LoginName = v);
    }

    public string Password
    {
        get { return data.Password; }
        set => SetDataProperty(value, () => data.Password, v => data.Password = v);
    }

    public string FirstName
    {
        get { return data.FirstName; }
        set => SetDataProperty(value, () => data.FirstName, v => data.FirstName = v);
    }

    public string Surname
    {
        get { return data.Surname; }
        set => SetDataProperty(value, () => data.Surname, v => data.Surname = v);
    }

    public DateTime BirthDate
    {
        get { return data.BirthDate; }
        set => SetDataProperty(value, () => data.BirthDate, v => data.BirthDate = v);
    }

    public string BirthPlace
    {
        get { return data.BirthPlace; }
        set => SetDataProperty(value, () => data.BirthPlace, v => data.BirthPlace = v);
    }

    public string AddressCity
    {
        get { return data.AddressCity; }
        set => SetDataProperty(value, () => data.AddressCity, v => data.AddressCity = v);
    }
    #endregion
}

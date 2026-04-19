/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Contracts.DTO;
using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Data;
using System.IO;

namespace App4di.Dotnet.UserManager.Model.Entities;

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

    public static bool IsUserExist(string loginName, string password)
    {
        if (File.Exists(Constants.XmlDataFilePath))
        {
            IDataManager<User> dataManager = new XmlDataManager<User>();
            var data = dataManager.Load(Constants.XmlDataFilePath);

            User? user = data.FirstOrDefault(u => u.LoginName == loginName && u.Password == password);
            return user != null;
        }

        throw new FileNotFoundException();
    }

    public override string ToString()
    {
        return UserId + LoginName + Password + FirstName + Surname + BirthDate + BirthPlace + AddressCity;
    }

    #region Properties
    public static User? CurrentUser { get; set; }

    public int UserId
    {
        get { return data.UserId; }

        set
        {
            NotifyPropertyChanging();
            data.UserId = value;
            NotifyPropertyChanged();
        }
    }

    public string LoginName
    {
        get { return data.LoginName; }

        set
        {
            NotifyPropertyChanging();
            data.LoginName = value;
            NotifyPropertyChanged();
        }
    }

    public string Password
    {
        get { return data.Password; }

        set
        {
            NotifyPropertyChanging();
            data.Password = value;
            NotifyPropertyChanged();
        }
    }

    public string FirstName
    {
        get { return data.FirstName; }

        set
        {
            NotifyPropertyChanging();
            data.FirstName = value;
            NotifyPropertyChanged();
        }
    }

    public string Surname
    {
        get { return data.Surname; }

        set
        {
            NotifyPropertyChanging();
            data.Surname = value;
            NotifyPropertyChanged();
        }
    }

    public DateTime BirthDate
    {
        get { return data.BirthDate; }

        set
        {
            NotifyPropertyChanging();
            data.BirthDate = value;
            NotifyPropertyChanged();
        }
    }

    public string BirthPlace
    {
        get { return data.BirthPlace; }

        set
        {
            NotifyPropertyChanging();
            data.BirthPlace = value;
            NotifyPropertyChanged();
        }
    }

    public string AddressCity
    {
        get { return data.AddressCity; }

        set
        {
            NotifyPropertyChanging();
            data.AddressCity = value;
            NotifyPropertyChanged();
        }
    }
    #endregion
}

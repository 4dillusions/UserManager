/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;

namespace App4di.Dotnet.UserManager.Model.Entities;

public class UserFilter : NotificationObject
{
    private string addressCity = string.Empty;
    private string textInAll = string.Empty;

    public UserFilter()
    {
        AddressCity = new AddressCity().CityName;
        TextInAll = string.Empty;
    }

    public string AddressCity
    {
        get { return addressCity; }

        set
        {
            NotifyPropertyChanging("AddressCity");
            addressCity = value;
            NotifyPropertyChanged("AddressCity");
        }
    }

    public string TextInAll
    {
        get { return textInAll; }

        set
        {
            NotifyPropertyChanging("TextInAll");
            textInAll = value;
            NotifyPropertyChanged("TextInAll");
        }
    }
}

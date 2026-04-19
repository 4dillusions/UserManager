/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;
using AddressCityEntity = App4di.Dotnet.UserManager.Model.Entities.AddressCity;

namespace App4di.Dotnet.UserManager.Model.Entities;

public class UserFilter : NotificationObject
{
    private string addressCity = string.Empty;
    private string textInAll = string.Empty;

    public UserFilter()
    {
        AddressCity = AddressCityEntity.DefaultName;
        TextInAll = string.Empty;
    }

    public string AddressCity
    {
        get { return addressCity; }

        set
        {
            NotifyPropertyChanging();
            addressCity = value;
            NotifyPropertyChanged();
        }
    }

    public string TextInAll
    {
        get { return textInAll; }

        set
        {
            NotifyPropertyChanging();
            textInAll = value;
            NotifyPropertyChanged();
        }
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;

namespace App4di.Dotnet.UserManager.Model.Entities;

public class AddressCity : NotificationObject
{
    public static readonly string DefaultName = "ALL";
    private string cityName = string.Empty;

    public AddressCity()
    {
        CityName = DefaultName;
    }

    public string CityName
    {
        get { return cityName; }

        set
        {
            NotifyPropertyChanging();
            cityName = value;
            NotifyPropertyChanged();
        }
    }

    public override string ToString()
    {
        return CityName;
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using FW4di.Dotnet.MVVM;

namespace App4di.Dotnet.UserManager.Infrastructure.Entities;

public class AddressCity : NotificationObject
{
    public static readonly string DefaultName = "ALL";

    public AddressCity()
    {
        CityName = DefaultName;
    }

    public string CityName
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;

    public override string ToString()
    {
        return CityName;
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using FW4di.Dotnet.MVVM;
using AddressCityEntity = App4di.Dotnet.UserManager.Presentation.Models.AddressCity;

namespace App4di.Dotnet.UserManager.Presentation.Models;

public class UserFilter : NotificationObject
{
    public UserFilter()
    {
        AddressCity = AddressCityEntity.DefaultName;
        TextInAll = string.Empty;
    }

    public string AddressCity
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;

    public string TextInAll
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;
}

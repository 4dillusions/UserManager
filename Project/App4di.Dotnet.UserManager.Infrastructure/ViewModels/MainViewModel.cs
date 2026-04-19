/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Navigation;
using FW4di.Dotnet.MVVM;

namespace App4di.Dotnet.UserManager.Infrastructure.ViewModels;

public class MainViewModel : NotificationObject
{
    public ViewType ViewType
    {
        get;
        set => SetProperty(ref field, value);
    }
}

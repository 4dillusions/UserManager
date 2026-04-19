/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Factory;
using App4di.Dotnet.UserManager.ViewModel.Navigation;

namespace App4di.Dotnet.UserManager.ViewModel.ViewModels;

public class MainViewModel : NotificationObject
{
    private ViewType viewType;

    public ViewType ViewType
    {
        get { return viewType; }

        set
        {
            viewType = value;
            NotifyPropertyChanged();
        }
    }

    public MainViewModel()
    {
        Ioc<MainViewModel>.Register<MainViewModel>(() => this);
        Ioc<MainViewModel>.Create();
    }
}

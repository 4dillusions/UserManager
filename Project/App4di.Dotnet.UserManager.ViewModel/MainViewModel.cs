/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Factory;
using App4di.Dotnet.UserManager.ViewModel.ViewManagement;

namespace App4di.Dotnet.UserManager.ViewModel;

public class MainViewModel : NotificationObject
{
    private ViewType viewType;

    public ViewType ViewType
    {
        get { return viewType; }

        set
        {
            viewType = value;
            NotifyPropertyChanged("ViewType");
        }
    }

    public MainViewModel()
    {
        Ioc<MainViewModel>.Register<MainViewModel>(() => this);
        Ioc<MainViewModel>.Create();
    }
}

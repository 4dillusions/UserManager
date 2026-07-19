/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Navigation;
using FW4di.Dotnet.MVVM;

namespace App4di.Dotnet.UserManager.Presentation.ViewModels;

public class MainViewModel : NotificationObject
{
    private readonly INavigationService navigationService;

    public MainViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        navigationService.CurrentViewChanged += (_, _) => RaisePropertyChanged(nameof(ViewType));
    }

    public ViewType ViewType => navigationService.CurrentView;
}

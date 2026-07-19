/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Presentation.Navigation;

public class NavigationService : INavigationService
{
    public ViewType CurrentView { get; private set; } = ViewType.Login;

    public event EventHandler? CurrentViewChanged;

    public void Navigate(ViewType viewType)
    {
        if (CurrentView == viewType)
            return;

        CurrentView = viewType;
        CurrentViewChanged?.Invoke(this, EventArgs.Empty);
    }
}

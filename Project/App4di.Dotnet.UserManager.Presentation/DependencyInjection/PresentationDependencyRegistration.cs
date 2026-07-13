/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Session;
using App4di.Dotnet.UserManager.Presentation.Users;
using App4di.Dotnet.UserManager.Presentation.ViewModels;
using App4di.Dotnet.UserManager.Presentation.Mapping;
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.UserManager.Presentation.DependencyInjection;

public static class PresentationDependencyRegistration
{
    public static void BindPresentation(this IDIManager di)
    {
        ArgumentNullException.ThrowIfNull(di);

        di.Bind<UserAutoMapperManager, UserAutoMapperManager>(DILifetimeScopes.Singleton);
        di.Bind<ISessionService, SessionService>(DILifetimeScopes.Singleton);
        di.Bind<IUserEditSessionService, UserEditSessionService>(DILifetimeScopes.Singleton);
        di.Bind<INavigationService, NavigationService>(DILifetimeScopes.Singleton);
        di.Bind<MainViewModel, MainViewModel>(DILifetimeScopes.Singleton);
        di.Bind<LoginViewModel, LoginViewModel>(DILifetimeScopes.Singleton);
        di.Bind<UserViewModel, UserViewModel>(DILifetimeScopes.Singleton);
        di.Bind<UserListViewModel, UserListViewModel>(DILifetimeScopes.Singleton);
    }
}

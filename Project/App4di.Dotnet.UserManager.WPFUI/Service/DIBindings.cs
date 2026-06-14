/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Authentication;
using App4di.Dotnet.UserManager.Application.Export;
using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Infrastructure.Export;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using App4di.Dotnet.UserManager.Presentation.Session;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Users;
using App4di.Dotnet.UserManager.Presentation.ViewModels;
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.UserManager.WPFUI.Service;

public class DIBindings
{
    private readonly IDIManager di = new DIManager();

    public void BindAllDependencies()
    {
        di.Init(
            () =>
            {
                di.Bind<IUserRepository, XmlUserRepository>(DILifetimeScopes.Singleton);
                di.Bind<IAddressCityRepository, XmlAddressCityRepository>(DILifetimeScopes.Singleton);
                di.Bind<IAuthenticationService, AuthenticationService>(DILifetimeScopes.Singleton);
                di.Bind<IUserQueryService, UserQueryService>(DILifetimeScopes.Singleton);
                di.Bind<IUserExportService, JsonUserExportService>(DILifetimeScopes.Singleton);
                di.Bind<ISessionService, SessionService>(DILifetimeScopes.Singleton);
                di.Bind<IUserEditSessionService, UserEditSessionService>(DILifetimeScopes.Singleton);
                di.Bind<INavigationService, NavigationService>(DILifetimeScopes.Singleton);
                di.Bind<MainViewModel, MainViewModel>(DILifetimeScopes.Singleton);
                di.Bind<LoginViewModel, LoginViewModel>(DILifetimeScopes.Singleton);
                di.Bind<UserViewModel, UserViewModel>(DILifetimeScopes.Singleton);
                di.Bind<UserListViewModel, UserListViewModel>(DILifetimeScopes.Singleton);
            });
    }

    public void Bind<TContract, TImplementation>(DILifetimeScopes lifetimeScope)
        where TImplementation : TContract
    {
        di.Bind<TContract, TImplementation>(lifetimeScope);
    }

    public T GetDependency<T>() => di.GetDependency<T>();

}

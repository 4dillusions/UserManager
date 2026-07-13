/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Authentication;
using App4di.Dotnet.UserManager.Application.Users;
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.UserManager.Application.DependencyInjection;

public static class ApplicationDependencyRegistration
{
    public static void BindApplication(this IDIManager di)
    {
        ArgumentNullException.ThrowIfNull(di);

        di.Bind<IAuthenticationService, AuthenticationService>(DILifetimeScopes.Singleton);
        di.Bind<IUserQueryService, UserQueryService>(DILifetimeScopes.Singleton);
        di.Bind<ISaveUserUseCase, SaveUserUseCase>(DILifetimeScopes.Singleton);
        di.Bind<IDeleteUserUseCase, DeleteUserUseCase>(DILifetimeScopes.Singleton);
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Export;
using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Data;
using App4di.Dotnet.UserManager.Infrastructure.Export;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.UserManager.Infrastructure.DependencyInjection;

public static class InfrastructureDependencyRegistration
{
    public static void BindInfrastructure(this IDIManager di)
    {
        ArgumentNullException.ThrowIfNull(di);

        di.Bind<XmlDataManager<UserData>, XmlDataManager<UserData>>(DILifetimeScopes.Singleton);
        di.Bind<JsonDataManager<UserData>, JsonDataManager<UserData>>(DILifetimeScopes.Singleton);
        di.Bind<IUserRepository, XmlUserRepository>(DILifetimeScopes.Singleton);
        di.Bind<IAddressCityRepository, XmlAddressCityRepository>(DILifetimeScopes.Singleton);
        di.Bind<IUserExportService, JsonUserExportService>(DILifetimeScopes.Singleton);
    }
}

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.DependencyInjection;
using App4di.Dotnet.UserManager.Infrastructure.DependencyInjection;
using App4di.Dotnet.UserManager.Presentation.DependencyInjection;
using App4di.Dotnet.UserManager.Presentation.Services;
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.UserManager.WPFUI.Service;

public class DIBindings : IDIManager
{
    private readonly IDIManager di = new DIManager();

    public void Init(Action bindings)
    {
        di.Init(bindings);
    }

    public void BindApplication() => di.BindApplication();

    public void BindPresentation() => di.BindPresentation();

    public void BindInfrastructure() => di.BindInfrastructure();

    public void BindWpfUi()
    {
        di.Bind<IUserNotificationService, WpfUserNotificationService>(DILifetimeScopes.Singleton);
        di.Bind<IApplicationService, WpfApplicationService>(DILifetimeScopes.Singleton);
    }

    public void Bind<TContract, TImplementation>(DILifetimeScopes lifetimeScope) where TImplementation : TContract
        => di.Bind<TContract, TImplementation>(lifetimeScope);

    public T GetDependency<T>() => di.GetDependency<T>();

    public void Bind<TInterface, TImplementation>(DILifetimeScopes scope, TImplementation instance) where TImplementation : ICloneable, TInterface
        => di.Bind<TInterface, TImplementation>(scope, instance);
}

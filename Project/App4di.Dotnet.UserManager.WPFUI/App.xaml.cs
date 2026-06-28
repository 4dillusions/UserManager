/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Services;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.ViewModels;
using App4di.Dotnet.UserManager.WPFUI.Converters;
using App4di.Dotnet.UserManager.WPFUI.Service;
using FW4di.Dotnet.Core.DependencyInjection;
using System.Globalization;
using System.Windows;

namespace App4di.Dotnet.UserManager.WPFUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public DIBindings DIBindings { get; } = new();

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        DIBindings.BindAllDependencies();
        DIBindings.Bind<IMessageService, WpfMessageService>(DILifetimeScopes.Singleton);
        DIBindings.Bind<IApplicationService, WpfApplicationService>(DILifetimeScopes.Singleton);

        ViewTypeConverter.Configure(new Dictionary<ViewType, Func<FrameworkElement>>
        {
            [ViewType.Login] = () => CreateView<LoginView, LoginViewModel>(),
            [ViewType.UserList] = () => CreateView<UserListView, UserListViewModel>(),
            [ViewType.User] = () => CreateView<UserView, UserViewModel>()
        });

        // Use a fixed culture so WPF parsing, validation, and formatting stay consistent across machines.
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        var mainView = new MainView
        {
            DataContext = DIBindings.GetDependency<MainViewModel>()
        };

        MainWindow = mainView;
        mainView.Show();
    }

    private TView CreateView<TView, TViewModel>()
        where TView : FrameworkElement, new()
    {
        return new TView
        {
            DataContext = DIBindings.GetDependency<TViewModel>()
        };
    }
}

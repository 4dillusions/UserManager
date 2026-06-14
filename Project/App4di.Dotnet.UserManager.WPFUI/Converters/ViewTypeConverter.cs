/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.ViewModels;
using App4di.Dotnet.UserManager.WPFUI.ViewManagement;
using System.Globalization;
using System.Windows;

namespace App4di.Dotnet.UserManager.WPFUI.Converters;

public class ViewTypeConverter : ConverterMarkupExtension<ViewTypeConverter>
{
    private static Dictionary<ViewType, ViewTypePair>? logics;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (logics == null)
        {
            logics = new Dictionary<ViewType, ViewTypePair>
            {
                { ViewType.Login, new ViewTypePair(typeof(LoginView), typeof(LoginViewModel)) },
                { ViewType.UserList, new ViewTypePair(typeof(UserListView), typeof(UserListViewModel)) },
                { ViewType.User, new ViewTypePair(typeof(UserView), typeof(UserViewModel)) }
            };
        }

        return base.ProvideValue(serviceProvider);
    }

    public override object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        var logic = logics![ConverterHelper.GetValue<ViewType>(value)];

        logic.CachedView = null;
        logic.CachedView = (FrameworkElement)Activator.CreateInstance(logic.ViewType)!;

        if (logic.ViewModelType != null)
        {
            try
            {
                var app = (App)System.Windows.Application.Current;
                logic.CachedView.DataContext = app.DIBindings.GetDependency(logic.ViewModelType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Navigation error");
            }
        }

        return logic.CachedView;
    }

    public override object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

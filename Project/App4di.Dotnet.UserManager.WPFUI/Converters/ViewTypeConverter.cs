/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.WPFUI.ViewManagement;
using System.Globalization;
using System.Windows;

namespace App4di.Dotnet.UserManager.WPFUI.Converters;

public class ViewTypeConverter : ConverterMarkupExtension<ViewTypeConverter>
{
    private static IReadOnlyDictionary<ViewType, Func<FrameworkElement>>? viewFactories;

    public static void Configure(IReadOnlyDictionary<ViewType, Func<FrameworkElement>> factories)
    {
        viewFactories = factories ?? throw new ArgumentNullException(nameof(factories));
    }

    public override object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (viewFactories == null)
            throw new InvalidOperationException("View factories have not been configured.");

        try
        {
            return viewFactories[ConverterHelper.GetValue<ViewType>(value)]();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Navigation error");
            return new FrameworkElement();
        }
    }

    public override object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

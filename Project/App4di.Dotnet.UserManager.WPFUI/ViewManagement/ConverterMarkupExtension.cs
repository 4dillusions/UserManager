/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace App4di.Dotnet.UserManager.WPFUI.ViewManagement;

[MarkupExtensionReturnType(typeof(IValueConverter))]
public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
    where T : class, IValueConverter, new()
{
    private static T? converter;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        converter ??= new T();
        return converter;
    }

    public abstract object Convert(object value, Type targetType, object? parameter, CultureInfo culture);

    public abstract object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture);
}

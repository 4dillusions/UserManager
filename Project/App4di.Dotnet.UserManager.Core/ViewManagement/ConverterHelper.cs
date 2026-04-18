/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Windows;

namespace App4di.Dotnet.UserManager.Core.ViewManagement;

public static class ConverterHelper
{
    public static TValueType GetValue<TValueType>(object value, TValueType defaultValue)
        where TValueType : struct
    {
        if (value == DependencyProperty.UnsetValue)
            return defaultValue;

        TValueType ret = defaultValue;

        if (value is TValueType)
        {
            ret = (TValueType)value;
        }
        else if (value is TValueType?)
        {
            var nullable = (TValueType?)value;
            ret = nullable ?? defaultValue;
        }

        return ret;
    }

    public static TValueType GetValue<TValueType>(object value) where TValueType : struct
    {
        return GetValue<TValueType>(value, default(TValueType));
    }
}

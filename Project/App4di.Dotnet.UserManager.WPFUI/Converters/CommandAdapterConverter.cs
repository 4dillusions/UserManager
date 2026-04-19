/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Collections.Concurrent;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using FWCommand = FW4di.Dotnet.MVVM.ICommand;
using WpfCommand = System.Windows.Input.ICommand;

namespace App4di.Dotnet.UserManager.WPFUI.Converters;

public sealed class CommandAdapterConverter : MarkupExtension, IValueConverter
{
    private static readonly ConcurrentDictionary<FWCommand, WpfCommand> Cache = new();

    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not FWCommand command)
            return null;

        return Cache.GetOrAdd(command, static c => new WpfCommandAdapter(c));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

internal sealed class WpfCommandAdapter(FWCommand innerCommand) : System.Windows.Input.ICommand
{
    public bool CanExecute(object? parameter) => innerCommand.CanExecute(parameter);

    public void Execute(object? parameter) => innerCommand.Execute(parameter);

    public event EventHandler? CanExecuteChanged
    {
        add => innerCommand.CanExecuteChanged += value;
        remove => innerCommand.CanExecuteChanged -= value;
    }
}

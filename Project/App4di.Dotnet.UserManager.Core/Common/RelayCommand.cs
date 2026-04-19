/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Windows.Input;

namespace App4di.Dotnet.UserManager.Core.Common;

public class RelayCommand : ICommand
{
    private readonly Action action;
    private readonly Func<bool> canExecuteMethod;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action action, Func<bool> canExecuteMethod)
    {
        this.action = action;
        this.canExecuteMethod = canExecuteMethod;
    }

    #region ICommand
    public bool CanExecute(object? parameter)
    {
        return canExecuteMethod();
    }

    public void Execute(object? parameter)
    {
        action();
    }
    #endregion
}

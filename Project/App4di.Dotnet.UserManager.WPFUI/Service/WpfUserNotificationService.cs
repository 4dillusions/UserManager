/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Services;
using System.Windows;

namespace App4di.Dotnet.UserManager.WPFUI.Service;

public class WpfUserNotificationService : IUserNotificationService
{
    public void ShowMessage(string message, string? title = null)
    {
        MessageBox.Show(message, title ?? "User Manager");
    }

    public bool ShowConfirmation(string message, string? title = null)
    {
        return MessageBox.Show(
            message,
            title ?? "User Manager",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }
}

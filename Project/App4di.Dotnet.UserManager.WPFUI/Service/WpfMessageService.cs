/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Service;
using System.Windows;

namespace App4di.Dotnet.UserManager.WPFUI.Service;

public class WpfMessageService : IMessageService
{
    public void ShowMessage(string message, string? title = null)
    {
        MessageBox.Show(message, title ?? "User Manager");
    }
}

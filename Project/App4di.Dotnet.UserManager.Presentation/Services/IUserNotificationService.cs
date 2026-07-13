/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Presentation.Services;

public interface IUserNotificationService
{
    void ShowMessage(string message, string? title = null);
    bool ShowConfirmation(string message, string? title = null);
}

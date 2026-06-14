/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Services;
using System.Windows;

namespace App4di.Dotnet.UserManager.WPFUI.Service;

public class WpfApplicationService : IApplicationService
{
    public void Shutdown()
    {
        System.Windows.Application.Current.Shutdown();
    }
}

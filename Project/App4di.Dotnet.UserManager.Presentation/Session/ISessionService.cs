/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Models;

namespace App4di.Dotnet.UserManager.Presentation.Session;

public interface ISessionService
{
    User? SelectedUser { get; set; }
    void Clear();
}

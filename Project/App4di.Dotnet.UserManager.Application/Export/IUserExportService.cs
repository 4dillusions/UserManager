/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Domain;

namespace App4di.Dotnet.UserManager.Application.Export;

public interface IUserExportService
{
    string ExportUsers(IEnumerable<UserData> users);
}

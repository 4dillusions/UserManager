/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Domain;

namespace App4di.Dotnet.UserManager.Application.Users;

public interface IUserQueryService
{
    IReadOnlyList<UserData> GetUsers(UserQueryCriteria filter);
    IReadOnlyList<string> GetAddressCities();
}

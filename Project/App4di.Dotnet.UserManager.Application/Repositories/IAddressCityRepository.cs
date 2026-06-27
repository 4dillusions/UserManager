/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Application.Repositories;

public interface IAddressCityRepository
{
    IReadOnlyList<string> LoadAddressCities();
}

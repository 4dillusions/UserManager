/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Application.Users;

public class UserQueryCriteria
{
    public const string AllAddressCities = "ALL";

    public string AddressCity { get; set; } = AllAddressCities;
    public string TextInAll { get; set; } = string.Empty;
}

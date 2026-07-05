/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Application.Users;

public static class BirthDateRules
{
    public static DateTime MinimumBirthDate { get; } = new(1500, 1, 1);
    public static DateTime MaximumBirthDate => DateTime.Today.AddYears(-18);

    public static bool IsValid(DateTime birthDate)
    {
        return birthDate >= MinimumBirthDate && birthDate <= MaximumBirthDate;
    }
}

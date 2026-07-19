/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Application.Users;

public static class BirthDateRules
{
    public const int MinimumBirthYear = 1500;
    public const int MinimumAgeInYears = 18;

    public static DateTime MinimumBirthDate { get; } = new(MinimumBirthYear, 1, 1);
    public static DateTime MaximumBirthDate => DateTime.Today.AddYears(-MinimumAgeInYears);

    public static bool IsValid(DateTime birthDate)
    {
        return birthDate >= MinimumBirthDate && birthDate <= MaximumBirthDate;
    }
}

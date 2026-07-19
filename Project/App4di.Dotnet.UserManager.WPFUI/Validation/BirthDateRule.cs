/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Users;
using System.Globalization;
using System.Windows.Controls;

namespace App4di.Dotnet.UserManager.WPFUI.Validation;

public class BirthDateRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is DateTime birthDate && BirthDateRules.IsValid(birthDate))
            return ValidationResult.ValidResult;

        return new ValidationResult(
            false,
            $"Please enter a birth date between {BirthDateRules.MinimumBirthDate:d} and {BirthDateRules.MaximumBirthDate:d}!");
    }
}

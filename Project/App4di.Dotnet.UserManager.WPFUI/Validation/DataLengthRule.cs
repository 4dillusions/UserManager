/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Globalization;
using System.Windows.Controls;

namespace App4di.Dotnet.UserManager.WPFUI.Validation;

public class DataLengthRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()) || value.ToString()!.Length < 4)
            return new ValidationResult(false, "Please enter at least 4 characters!");

        return new ValidationResult(true, null);
    }
}

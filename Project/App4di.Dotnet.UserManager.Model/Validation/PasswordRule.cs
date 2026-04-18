/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace App4di.Dotnet.UserManager.Model.Validation;

public class PasswordRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (string.IsNullOrEmpty(value.ToString()) || !Regex.IsMatch(value.ToString()!, "^(?=.{6,20}$)(?=.*[A-Z])(?=.*[0-9])"))
            return new ValidationResult(false, "Please enter data 6-20 characters, a lower-case letter and a number!");

        return new ValidationResult(true, null);
    }
}

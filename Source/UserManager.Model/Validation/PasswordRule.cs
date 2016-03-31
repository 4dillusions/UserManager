using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace UserManager.Model.Validation
{
    public class PasswordRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (String.IsNullOrEmpty(value.ToString()) || !Regex.IsMatch(value.ToString(), "^(?=.{6,20}$)(?=.*[A-Z])(?=.*[0-9])"))
                return new ValidationResult(false, "Please enter data 6-20 characters, a lower-case letter and a number!");
            else
                return new ValidationResult(true, null);
        }
    }
}

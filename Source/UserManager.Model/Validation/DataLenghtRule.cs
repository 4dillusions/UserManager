using System;
using System.Globalization;
using System.Windows.Controls;

namespace UserManager.Model.Validation
{
    public class DataLenghtRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (String.IsNullOrEmpty(value.ToString()) || value.ToString().Length < 4)
                return new ValidationResult(false, "Please enter data more than 4 characters!");
            else
                return new ValidationResult(true, null);
        }
    }
}

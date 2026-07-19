/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Presentation.Common;

public static class ExceptionMessageFormatter
{
    public static string Format(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception.InnerException == null
            ? exception.Message
            : $"{exception.Message}{Environment.NewLine}{exception.InnerException.Message}";
    }
}

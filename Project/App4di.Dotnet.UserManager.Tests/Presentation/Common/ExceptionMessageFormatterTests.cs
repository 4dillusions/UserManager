/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Common;

namespace App4di.Dotnet.UserManager.Tests.Presentation.Common;

[TestClass]
public class ExceptionMessageFormatterTests
{
    [TestMethod]
    public void FormatReturnsOnlyOuterMessageWithoutInnerException()
    {
        var message = ExceptionMessageFormatter.Format(new InvalidOperationException("Operation failed."));

        Assert.AreEqual("Operation failed.", message);
        Assert.IsFalse(message.Contains("null", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void FormatAppendsInnerExceptionMessageOnNewLine()
    {
        var exception = new InvalidOperationException(
            "Operation failed.",
            new IOException("Storage is unavailable."));

        var message = ExceptionMessageFormatter.Format(exception);

        Assert.AreEqual(
            $"Operation failed.{Environment.NewLine}Storage is unavailable.",
            message);
    }
}

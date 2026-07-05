/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Users;

namespace App4di.Dotnet.UserManager.Tests.Application.Users;

[TestClass]
public class BirthDateRulesTests
{
    [TestMethod]
    public void MinimumBirthDateIsValid()
    {
        Assert.IsTrue(BirthDateRules.IsValid(BirthDateRules.MinimumBirthDate));
    }

    [TestMethod]
    public void DateBeforeMinimumIsInvalid()
    {
        Assert.IsFalse(BirthDateRules.IsValid(BirthDateRules.MinimumBirthDate.AddDays(-1)));
    }

    [TestMethod]
    public void EighteenthBirthdayIsValid()
    {
        Assert.IsTrue(BirthDateRules.IsValid(DateTime.Today.AddYears(-18)));
    }

    [TestMethod]
    public void DateAfterEighteenthBirthdayIsInvalid()
    {
        Assert.IsFalse(BirthDateRules.IsValid(DateTime.Today.AddYears(-18).AddDays(1)));
    }
}

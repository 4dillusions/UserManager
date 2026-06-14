/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Application.Export;
using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Entities;
using System.Text.Json;

namespace App4di.Dotnet.UserManager.Tests.Application.Export;

[TestClass]
[DoNotParallelize]
public class JsonUserExportServiceTests
{
    private readonly IUserExportService exportService = new JsonUserExportService();

    [TestInitialize]
    public void TestInitialize()
    {
        Directory.CreateDirectory(Constants.DataFilePath);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (File.Exists(Constants.JsonDataFilePath))
            File.Delete(Constants.JsonDataFilePath);
    }

    [TestMethod]
    public void ExportUsersWritesExpectedJsonAndOverwritesExistingFile()
    {
        File.WriteAllText(Constants.JsonDataFilePath, "existing content");

        var exported = exportService.ExportUsers(CreateUsers());

        Assert.IsTrue(exported);
        Assert.IsTrue(File.Exists(Constants.JsonDataFilePath));

        using var document = JsonDocument.Parse(File.ReadAllText(Constants.JsonDataFilePath));
        Assert.AreEqual(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.AreEqual(2, document.RootElement.GetArrayLength());
        Assert.AreEqual("Albert", document.RootElement[0].GetProperty("LoginName").GetString());
        Assert.AreEqual("Budapest", document.RootElement[1].GetProperty("AddressCity").GetString());
    }

    [TestMethod]
    public void ExportUsersPreservesExistingJsonStructure()
    {
        exportService.ExportUsers(CreateUsers());

        using var document = JsonDocument.Parse(File.ReadAllText(Constants.JsonDataFilePath));
        var propertyNames = document.RootElement[0]
            .EnumerateObject()
            .Select(property => property.Name)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                "UserId",
                "LoginName",
                "Password",
                "FirstName",
                "Surname",
                "BirthDate",
                "BirthPlace",
                "AddressCity"
            },
            propertyNames);

        Assert.IsFalse(document.RootElement[0].TryGetProperty("HasErrors", out _));
    }

    private static List<User> CreateUsers()
    {
        return
        [
            new User
            {
                UserId = 1,
                LoginName = "Albert",
                Password = "Albert1",
                FirstName = "Albert",
                Surname = "Einstein",
                BirthDate = new DateTime(2879, 3, 14),
                BirthPlace = "German",
                AddressCity = "Württemberg"
            },
            new User
            {
                UserId = 2,
                LoginName = "Erno",
                Password = "Erno1234",
                FirstName = "Erno",
                Surname = "Rubik",
                BirthDate = new DateTime(1944, 7, 13),
                BirthPlace = "Hungary",
                AddressCity = "Budapest"
            }
        ];
    }
}

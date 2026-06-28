/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Export;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Export;
using System.Text.Json;

namespace App4di.Dotnet.UserManager.Tests.Infrastructure.Export;

[TestClass]
[DoNotParallelize]
public class JsonUserExportServiceTests
{
    private readonly IUserExportService exportService = new JsonUserExportService();

    [TestInitialize]
    public void TestInitialize()
    {
        Directory.CreateDirectory(DataFilePaths.DataDirectoryPath);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (File.Exists(DataFilePaths.JsonDataFilePath))
            File.Delete(DataFilePaths.JsonDataFilePath);

        foreach (var temporaryFilePath in Directory.GetFiles(DataFilePaths.DataDirectoryPath, ".data.json.*.tmp"))
            File.Delete(temporaryFilePath);
    }

    [TestMethod]
    public void ExportUsersWritesExpectedJsonAndOverwritesExistingFile()
    {
        File.WriteAllText(DataFilePaths.JsonDataFilePath, "existing content");

        var exportedFilePath = exportService.ExportUsers(CreateUsers());

        Assert.AreEqual(Path.GetFullPath(DataFilePaths.JsonDataFilePath), exportedFilePath);
        Assert.IsTrue(File.Exists(DataFilePaths.JsonDataFilePath));

        using var document = JsonDocument.Parse(File.ReadAllText(DataFilePaths.JsonDataFilePath));
        Assert.AreEqual(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.AreEqual(2, document.RootElement.GetArrayLength());
        Assert.AreEqual("Albert", document.RootElement[0].GetProperty("LoginName").GetString());
        Assert.AreEqual("Budapest", document.RootElement[1].GetProperty("AddressCity").GetString());
    }

    [TestMethod]
    public void ExportUsersPreservesExistingJsonStructure()
    {
        exportService.ExportUsers(CreateUsers());

        using var document = JsonDocument.Parse(File.ReadAllText(DataFilePaths.JsonDataFilePath));
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

    [TestMethod]
    public void FailedExportPreservesExistingFile()
    {
        const string existingContent = "existing content";
        File.WriteAllText(DataFilePaths.JsonDataFilePath, existingContent);

        Assert.Throws<InvalidOperationException>(() => exportService.ExportUsers(CreateFailingSequence()));

        Assert.AreEqual(existingContent, File.ReadAllText(DataFilePaths.JsonDataFilePath));
        Assert.IsEmpty(Directory.GetFiles(DataFilePaths.DataDirectoryPath, ".data.json.*.tmp"));
    }

    private static IEnumerable<UserData> CreateFailingSequence()
    {
        yield return new UserData();
        throw new InvalidOperationException("Enumeration failed.");
    }

    private static List<UserData> CreateUsers()
    {
        return
        [
            new UserData
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
            new UserData
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

/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using System.Xml.Linq;

namespace App4di.Dotnet.UserManager.Tests.Infrastructure.Repositories;

[TestClass]
[DoNotParallelize]
public class XmlRepositoryTests
{
    private readonly IUserRepository userRepository = new XmlUserRepository();

    [TestInitialize]
    public void TestInitialize()
    {
        Directory.CreateDirectory(Constants.DataFilePath);
        WriteUsers(CreateUsers());
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (File.Exists(Constants.XmlDataFilePath))
            File.Delete(Constants.XmlDataFilePath);
    }

    [TestMethod]
    public void LoadUsersFromXml()
    {
        var users = userRepository.LoadUsers();

        Assert.HasCount(2, users);
        Assert.AreEqual(1, users[0].UserId);
        Assert.AreEqual("Albert", users[0].LoginName);
        Assert.AreEqual("Budapest", users[1].AddressCity);
    }

    [TestMethod]
    public void SaveUsersToXml()
    {
        var users = CreateUsers();
        users[0].Surname = "Updated";

        userRepository.SaveUsers(users);

        var loadedUsers = new XmlDataManager<UserData>().Load(Constants.XmlDataFilePath);
        Assert.HasCount(2, loadedUsers);
        Assert.AreEqual("Updated", loadedUsers[0].Surname);
    }

    [TestMethod]
    public void LoadAddressCitiesFromXml()
    {
        IAddressCityRepository addressCityRepository = new XmlAddressCityRepository(userRepository);

        var cities = addressCityRepository.LoadAddressCities();

        Assert.HasCount(2, cities);
        Assert.AreEqual("Württemberg", cities[0]);
        Assert.AreEqual("Budapest", cities[1]);
    }

    [TestMethod]
    public void SaveUsersPreservesExistingXmlStructure()
    {
        userRepository.SaveUsers(CreateUsers());

        var document = XDocument.Load(Constants.XmlDataFilePath);
        var root = document.Root;
        var userElement = root?.Elements("User").FirstOrDefault();

        Assert.IsNotNull(root);
        Assert.AreEqual("ArrayOfUser", root.Name.LocalName);
        Assert.IsNotNull(userElement);
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
            userElement.Elements().Select(element => element.Name.LocalName).ToArray());
    }

    private static void WriteUsers(List<UserData> users)
    {
        new XmlDataManager<UserData>().Save(users, Constants.XmlDataFilePath);
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

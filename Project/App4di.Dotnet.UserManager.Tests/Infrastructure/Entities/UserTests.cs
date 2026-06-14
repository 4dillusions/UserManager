/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Authentication;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;
using App4di.Dotnet.UserManager.Infrastructure.Repositories;
using App4di.Dotnet.UserManager.Presentation.Mapping;
using App4di.Dotnet.UserManager.Presentation.Models;

namespace App4di.Dotnet.UserManager.Tests.Infrastructure.Entities
{
    [TestClass]
    public class UserTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Directory.CreateDirectory(Constants.DataFilePath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
                File.Delete(Constants.DataFilePath + Constants.XmlDataFileName);
        }

        List<User> CreateUsersData()
        {
            var result = new List<User>();

            result.Add(new User
            {
                UserId = 1,
                LoginName = "Albert",
                Password = "albert",
                FirstName = "Albert",
                Surname = "Einstein",
                BirthDate = new System.DateTime(2879, 3, 14),
                BirthPlace = "German",
                AddressCity = "Württemberg"
            });

            result.Add(new User
            {
                UserId = 2,
                LoginName = "Zoltan",
                Password = "zoltan",
                FirstName = "Zoltan",
                Surname = "Kodaly",
                BirthDate = new System.DateTime(1882, 12, 16),
                BirthPlace = "Hungary",
                AddressCity = "Kecskemet"
            });

            result.Add(new User
            {
                UserId = 3,
                LoginName = "Erno",
                Password = "erno",
                FirstName = "Erno",
                Surname = "Rubik",
                BirthDate = new System.DateTime(1944, 7, 13),
                BirthPlace = "Hungary",
                AddressCity = "Budapest"
            });

            result.Add(new User
            {
                UserId = 4,
                LoginName = "Nikola",
                Password = "nikola",
                FirstName = "Nikola",
                Surname = "Tesla",
                BirthDate = new System.DateTime(1856, 7, 10),
                BirthPlace = "Hungary Kingdom",
                AddressCity = "Smiljan"
            });

            result.Add(new User
            {
                UserId = 5,
                LoginName = "Charles",
                Password = "charles",
                FirstName = "Charles",
                Surname = "Simonyi",
                BirthDate = new System.DateTime(1948, 9, 10),
                BirthPlace = "Hungary",
                AddressCity = "Washington"
            });

            return result;
        }

        void CreateXmlData()
        {
            IDataManager<UserData> dataManager = new XmlDataManager<UserData>();

            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
                File.Delete(Constants.DataFilePath + Constants.XmlDataFileName);

            dataManager.Save(CreateUsersData().Select(UserMapper.ToUserData).ToList(), Constants.DataFilePath + Constants.XmlDataFileName);
        }

        [TestMethod]
        public void UserExist()
        {
            CreateXmlData();

            var data = CreateUsersData();
            var authenticationService = new AuthenticationService(new XmlUserRepository());
            Assert.IsTrue(authenticationService.Authenticate(data[2].LoginName, data[2].Password), "User exist");
        }

        [TestMethod]
        public void UserRaisesPropertyChangedForUiBinding()
        {
            var user = new User();
            string? changedProperty = null;
            user.PropertyChanged += (_, args) => changedProperty = args.PropertyName;

            user.Surname = "Updated";

            Assert.AreEqual(nameof(User.Surname), changedProperty);
        }

        [TestMethod]
        public void AddressCityListTest()
        {
            IDataManager<UserData> dataManager = new XmlDataManager<UserData>();

            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
                File.Delete(Constants.DataFilePath + Constants.XmlDataFileName);

            var createData = CreateUsersData();
            createData.Add(new User { AddressCity = createData[1].AddressCity });
            createData.Add(new User { AddressCity = createData[2].AddressCity });
            dataManager.Save(createData.Select(UserMapper.ToUserData).ToList(), Constants.DataFilePath + Constants.XmlDataFileName);

            var data = CreateUsersData();
            var queryService = new UserQueryService(
                new XmlUserRepository(),
                new XmlAddressCityRepository(new XmlUserRepository()));
            Assert.AreEqual(data.Count, new AddressCityList(queryService).Cities.Count - 1, "Address cities");

            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
                File.Delete(Constants.DataFilePath + Constants.XmlDataFileName);

            dataManager.Save(CreateUsersData().Select(UserMapper.ToUserData).ToList(), Constants.DataFilePath + Constants.XmlDataFileName);
        }

        [TestMethod]
        public void UserListTest()
        {
            CreateXmlData();

            var data = CreateUsersData();
            var queryService = new UserQueryService(
                new XmlUserRepository(),
                new XmlAddressCityRepository(new XmlUserRepository()));
            Assert.HasCount(data.Count, new UserList(new UserFilter(), queryService).Users, "User list count default filter");
            Assert.HasCount(1, new UserList(new UserFilter() { AddressCity = data[0].AddressCity }, queryService).Users, "User list count 1 city filter");
            Assert.HasCount(1, new UserList(new UserFilter() { TextInAll = data[0].FirstName }, queryService).Users, "User list count 1 city in the text filter");
            Assert.IsEmpty(new UserList(new UserFilter() { TextInAll = "Texas" }, queryService).Users, "User list count 0 word in the text filter");
        }
    }
}

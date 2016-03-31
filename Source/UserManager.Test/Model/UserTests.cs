using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using UserManager.Core.Common;
using UserManager.Core.Data;
using UserManager.Model;

namespace UserManager.Test.Model
{
    [TestClass]
    public class UserTests
    {
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
            IDataManager<User> dataManager = new XmlDataManager<User>();

            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
                File.Delete(Constants.DataFilePath + Constants.XmlDataFileName);

            dataManager.Save(CreateUsersData(), Constants.DataFilePath + Constants.XmlDataFileName);
        }

        [TestMethod]
        public void UserExist()
        {
            CreateXmlData();   

            var data = CreateUsersData();
            Assert.IsTrue(User.IsUserExist(data[2].LoginName, data[2].Password), "User exist");
        }

        [TestMethod]
        public void AddressCityListTest()
        {
            IDataManager<User> dataManager = new XmlDataManager<User>();

            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
                File.Delete(Constants.DataFilePath + Constants.XmlDataFileName);

            var createData = CreateUsersData();
            createData.Add(new User { AddressCity = createData[1].AddressCity });
            createData.Add(new User { AddressCity = createData[2].AddressCity });
            dataManager.Save(createData, Constants.DataFilePath + Constants.XmlDataFileName);

            var data = CreateUsersData();
            Assert.IsTrue(new AddressCityList().Cities.Count - 1 == data.Count, "Address cities");

            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
                File.Delete(Constants.DataFilePath + Constants.XmlDataFileName);

            dataManager.Save(CreateUsersData(), Constants.DataFilePath + Constants.XmlDataFileName);
        }

        [TestMethod]
        public void UserListTest()
        {
            CreateXmlData();

            var data = CreateUsersData();
            Assert.IsTrue(new UserList(new UserFilter()).Users.Count == data.Count, "User list count default filter");
            Assert.IsTrue(new UserList(new UserFilter() { AddressCity = data[0].AddressCity }).Users.Count == 1, "User list count 1 city filter");
            Assert.IsTrue(new UserList(new UserFilter() { TextInAll = data[0].FirstName }).Users.Count == 1, "User list count 1 city in the text filter");
            Assert.IsFalse(new UserList(new UserFilter() { TextInAll = "Texas" }).Users.Count == 1, "User list count 0 word in the text filter");
        }
    }
}

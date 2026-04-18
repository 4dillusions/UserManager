/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Common;
using App4di.Dotnet.UserManager.Core.Data;

namespace App4di.Dotnet.UserManager.Tests.Core.Data
{
    [Serializable]
    public class DataTest
    {
        public int Test1 { get; set; }
        public string Test2 { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is DataTest other &&
                Test1 == other.Test1 &&
                Test2 == other.Test2;
        }

        public override int GetHashCode()
        {
            return (Test1.GetHashCode() + Test2.GetHashCode());
        }
    }

    [TestClass]
    public class DataManagerTests
    {
        static readonly string XmlPathAndFileName = Constants.DataFilePath + @"Test.xml";
        static readonly string JsonPathAndFileName = Constants.DataFilePath + @"Test.json";

        [TestInitialize]
        public void TestInitialize()
        {
            Directory.CreateDirectory(Constants.DataFilePath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(XmlPathAndFileName))
                File.Delete(XmlPathAndFileName);

            if (File.Exists(JsonPathAndFileName))
                File.Delete(JsonPathAndFileName);
        }

        List<DataTest> CreateTestData()
        {
            List<DataTest> result = new List<DataTest>();

            DataTest t1 = new DataTest { Test1 = 1, Test2 = "test1" };
            DataTest t2 = new DataTest { Test1 = 2, Test2 = "test2" };
            result.Add(t1);
            result.Add(t2);

            return result;
        }

        [TestMethod]
        public void XmlTest()
        {
            IDataManager<DataTest> dataManager = new XmlDataManager<DataTest>();

            if (File.Exists(XmlPathAndFileName))
                File.Delete(XmlPathAndFileName);

            dataManager.Save(CreateTestData(), XmlPathAndFileName);
            Assert.IsTrue(File.Exists(XmlPathAndFileName), "XML file created");

            var loadedData = dataManager.Load(XmlPathAndFileName);
            var createdData = CreateTestData();
            Assert.IsTrue(loadedData.Count == 2 && loadedData[0].Equals(createdData[0]) && loadedData[1].Equals(createdData[1]), "XML file load");
        }

        [TestMethod]
        public void JsonTest()
        {
            IDataManager<DataTest> dataManager = new JsonDataManager<DataTest>();

            if (File.Exists(JsonPathAndFileName))
                File.Delete(JsonPathAndFileName);

            dataManager.Save(CreateTestData(), JsonPathAndFileName);
            Assert.IsTrue(File.Exists(JsonPathAndFileName), "Json file created");

            var loadedData = dataManager.Load(JsonPathAndFileName);
            var createdData = CreateTestData();
            Assert.IsTrue(loadedData.Count == 2 && loadedData[0].Equals(createdData[0]) && loadedData[1].Equals(createdData[1]), "Json file load");
        }
    }
}

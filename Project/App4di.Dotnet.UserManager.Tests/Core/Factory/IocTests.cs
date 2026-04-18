/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Core.Factory;

namespace App4di.Dotnet.UserManager.Tests.Core.Factory
{
    public class Singleton
    {
        public int Test { get; set; }
    }

    public interface IFactory
    {
        int Test { get; set; }
    }

    public class IocTest1 : IFactory
    {
        int test;

        public int Test
        {
            get { return test; }
            set { test = value; }
        }
    }

    public class IocTest2 : IFactory
    {
        int test;

        public int Test
        {
            get { return test; }
            set { test = value; }
        }
    }

    [TestClass]
    public class IocTests
    {
        [TestMethod]
        public void IocSingletonTest()
        {
            Ioc<Singleton>.Register<Singleton>();
            Ioc<Singleton>.Create();

            Ioc<Singleton>.Instance.Test = 1;
            Ioc<Singleton>.Instance.Test = 2;

            Assert.AreEqual(2, Ioc<Singleton>.Instance.Test, "Singleton data equal");
            Assert.IsTrue(Object.ReferenceEquals(Ioc<Singleton>.Instance, Ioc<Singleton>.Instance), "Singleton reference equal");
        }

        [TestMethod]
        public void IocFactoryTest()
        {
            Ioc<IFactory>.Register<IocTest1>(() => new IocTest1() { Test = 2 });
            Ioc<IFactory>.Create();

            Assert.AreEqual(2, Ioc<IFactory>.Instance.Test, "IocTest1 data");
        }
    }
}

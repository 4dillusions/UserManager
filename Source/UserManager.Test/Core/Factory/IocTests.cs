using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UserManager.Core.Factory;

namespace UserManager.Test.Core.Factory
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

            Assert.IsTrue(Ioc<Singleton>.Instance.Test == 2, "Singleton data equal");
            Assert.IsTrue(Object.ReferenceEquals(Ioc<Singleton>.Instance, Ioc<Singleton>.Instance), "Singleton reference equal");
        }

        [TestMethod]
        public void IocFactoryTest()
        {
            Ioc<IFactory>.Register<IocTest1>(() => new IocTest1() { Test = 2 });
            Ioc<IFactory>.Create();

            Assert.IsTrue(Ioc<IFactory>.Instance.Test == 2, "IocTest1 data");
        }
    }
}

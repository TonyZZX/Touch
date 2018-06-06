
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touch.Models;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = new Category();
            Assert.AreEqual(a.Get(0), "Building");
        }
    }
}

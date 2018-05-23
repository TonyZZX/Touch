#region

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touch.Models;

#endregion

namespace Touch.UnitTest.Models
{
    [TestClass]
    internal class CategoryTest
    {
        [TestMethod]
        public void GetTest()
        {
            var category = new Category();
            Assert.AreEqual(category.Get(-1), "");
            Assert.AreEqual(category.Get(0), "Building");
            Assert.AreEqual(category.Get(24), "");
        }
    }
}
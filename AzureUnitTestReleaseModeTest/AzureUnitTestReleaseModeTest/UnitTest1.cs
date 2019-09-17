using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureUnitTestReleaseModeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
#if DEBUG
            Console.WriteLine("Running in debug");
#else
            Console.WriteLine("Running in release");
#endif

            Assert.IsTrue(1 < 2);
        }
    }
}

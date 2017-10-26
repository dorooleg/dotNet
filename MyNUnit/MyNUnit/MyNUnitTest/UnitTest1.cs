using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNUnit;

namespace MyNUnitTest
{    
    public class UnitTest1
    {
        [BeforeClass]
        public int BeforeClass()
        {
            return 5;
        }

        [AfterClass]
        public void AfterClass()
        {
        }

        [Before]
        public void Before()
        {
        }

        [After]
        public void After()
        {
        }

        [Test]
        public void TestMethod1()
        {
            Assert.IsTrue(5 < 3);
        }

        [Test(Ignore = "just skip")]
        public void TestMethod2()
        {
            Assert.IsTrue(5 < 3);
        }

        [Test(Expected = typeof(ArgumentException))]
        public void TestMethod3()
        {
            throw new ArgumentException();
        }
    }
}

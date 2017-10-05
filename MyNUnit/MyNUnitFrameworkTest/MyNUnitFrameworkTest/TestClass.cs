namespace MyNUnitFrameworkTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyNUnit;

    public class TestClass
    {
        [Before]
        [ExpectedException(typeof(TestClass))]
        public void Before(int a)
        {
        }

        [Before]
        public void Before0(int a)
        {
        }

        [Before]
        public void Before1()
        {
        }

        [Before]
        public void Before2()
        {
        }

        [Test]
        [ExpectedException(typeof(TestClass))]
        public void Test(int a)
        {
        }

        [Test]
        public void Test0(int a)
        {
        }

        [Test]
        public void Test1()
        {
        }

        [Test]
        public void Test2()
        {
        }

        [After]
        [ExpectedException(typeof(TestClass))]
        public void After(int a)
        {
        }

        [After]
        public void After0(int a)
        {
        }

        [After]
        public void After1()
        {
        }

        [After]
        public void After2()
        {
        }

        [AfterClass]
        [ExpectedException(typeof(TestClass))]
        public void AfterClass(int a)
        {
        }

        [AfterClass]
        public void AfterClass0(int a)
        {
        }

        [AfterClass]
        public void AfterClass1()
        {
        }

        [AfterClass]
        public void AfterClass2()
        {
        }

        [BeforeClass]
        [ExpectedException(typeof(TestClass))]
        public void BeforeClass(int a)
        {
        }

        [BeforeClass]
        public void BeforeClass0(int a)
        {
        }

        [BeforeClass]
        public void BeforeClass1()
        {
        }

        [BeforeClass]
        public void BeforeClass2()
        {
        }
    }
}

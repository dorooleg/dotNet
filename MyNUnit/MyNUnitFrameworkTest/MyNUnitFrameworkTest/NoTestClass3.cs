namespace MyNUnitFrameworkTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyNUnit;

    public class NoTestClass3
    {
        [Test]
        [ExpectedException(typeof(TestClass))]
        public void Test(int a)
        {
        }

        [Test]
        public void Test0(int a)
        {
        }
    }
}

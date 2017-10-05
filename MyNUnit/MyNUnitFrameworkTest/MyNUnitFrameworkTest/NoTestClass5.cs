namespace MyNUnitFrameworkTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyNUnit;

    public class NoTestClass5
    {
        [AfterClass]
        [ExpectedException(typeof(TestClass))]
        public void AfterClass(int a)
        {
        }

        [AfterClass]
        public void AfterClass0(int a)
        {
        }
    }
}

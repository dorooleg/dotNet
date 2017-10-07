namespace MyNUnitFrameworkTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyNUnit;

    public class AfterClassArgs
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

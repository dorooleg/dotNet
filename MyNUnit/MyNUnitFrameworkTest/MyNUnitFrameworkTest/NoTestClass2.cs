namespace MyNUnitFrameworkTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyNUnit;

    public class NoTestClass2
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
    }
}

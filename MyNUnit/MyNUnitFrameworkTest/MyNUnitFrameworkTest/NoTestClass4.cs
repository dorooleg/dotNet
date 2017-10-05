namespace MyNUnitFrameworkTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyNUnit;

    public class NoTestClass4
    {
        [After]
        [ExpectedException(typeof(TestClass))]
        public void After(int a)
        {
        }

        [After]
        public void After0(int a)
        {
        }
    }
}

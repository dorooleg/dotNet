using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNUnit;

namespace MyNUnitFrameworkTest
{
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNUnit;

namespace MyNUnitFrameworkTest
{
    public class BeforeArgs
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

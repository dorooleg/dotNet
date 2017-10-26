using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNUnit;

namespace MyNUnitFrameworkTest
{
    public class TestWithArgs
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

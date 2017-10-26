using System;
using MyNUnit;

namespace MyNUnitTest
{    
    public class AfterClassException
    {
        [AfterClass]
        public void SetUp()
        {
            throw new ArgumentException();
        }

        [Test]
        public void Test()
        {
        }
    }
}

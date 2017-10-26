using System;
using MyNUnit;

namespace MyNUnitTest
{    
    public class BeforeClassException
    {
        [BeforeClass]
        public void SetUp()
        {
            throw new ArgumentException();
        }

        [Test]
        public void Test1()
        {
        }
    }
}

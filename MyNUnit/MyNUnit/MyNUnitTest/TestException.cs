using System;
using MyNUnit;

namespace MyNUnitTest
{    
    public class TestException
    {
        [Test]
        public void Test()
        {
            throw new ArgumentException();
        }
    }
}

using System;
using MyNUnit;

namespace MyNUnitTest
{
    public class ConstructorException
    {
        public ConstructorException()
        {
            throw new ArgumentException();
        }

        [Test]
        public void TestMethod1()
        {
        }
    }
}

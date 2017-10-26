using System;
using MyNUnit;

namespace MyNUnitTest
{
    public class BeforeException
    {
        [Before]
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

namespace MyNUnitTest
{
    using System;
    using MyNUnit;

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

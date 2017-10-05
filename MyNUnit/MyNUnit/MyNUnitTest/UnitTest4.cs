namespace MyNUnitTest
{
    using System;
    using MyNUnit;

    public class UnitTest4
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

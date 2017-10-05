namespace MyNUnitTest
{
    using System;
    using MyNUnit;
    
    public class UnitTest6
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

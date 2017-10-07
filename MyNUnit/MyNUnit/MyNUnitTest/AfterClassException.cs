namespace MyNUnitTest
{
    using System;
    using MyNUnit;
    
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

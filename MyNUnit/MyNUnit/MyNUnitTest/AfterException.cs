namespace MyNUnitTest
{
    using System;
    using MyNUnit;
    
    public class AfterException
    {
        [After]
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

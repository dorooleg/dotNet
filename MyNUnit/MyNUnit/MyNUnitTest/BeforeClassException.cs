namespace MyNUnitTest
{
    using System;
    using MyNUnit;
    
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

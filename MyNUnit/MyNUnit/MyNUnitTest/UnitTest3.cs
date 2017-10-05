namespace MyNUnitTest
{
    using System;
    using MyNUnit;
    
    public class UnitTest3
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

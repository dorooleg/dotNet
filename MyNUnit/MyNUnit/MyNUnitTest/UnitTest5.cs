namespace MyNUnitTest
{
    using System;
    using MyNUnit;
    
    public class UnitTest5
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

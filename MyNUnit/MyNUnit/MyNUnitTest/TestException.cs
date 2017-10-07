namespace MyNUnitTest
{
    using System;
    using MyNUnit;
    
    public class TestException
    {
        [Test]
        public void Test()
        {
            throw new ArgumentException();
        }
    }
}

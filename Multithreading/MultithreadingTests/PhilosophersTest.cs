using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multithreading;

namespace MultithreadingTests
{
    [TestClass]
    public class PhilosophersTest
    {
        [TestMethod]
        [Timeout(50000)]
        public void SimpleWaitTest()
        {
            var p = new Philosophers();
            Assert.IsTrue(p.Simulate());
        }
    }
}
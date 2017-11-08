using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multithreading;

namespace MultithreadingTests
{
    [TestClass]
    public class LockBaseArrayQueueTest
    {
        [DataTestMethod]
        [Timeout(3000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        public void DequeueTest(Type clazz)
        {
            var queue = (IBlockingArrayQueue<int>) Activator.CreateInstance(clazz);
            queue.Enqueue(5);
            Assert.AreEqual(5, queue.Dequeue());
        }

        [DataTestMethod]
        [Timeout(3000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        public void ClearTest(Type clazz)
        {
            var queue = (IBlockingArrayQueue<int>) Activator.CreateInstance(clazz);
            queue.Enqueue(5);
            queue.Enqueue(3);
            queue.Enqueue(17);
            queue.Enqueue(21);
            queue.Clear();
            var res = 0;
            Assert.IsFalse(queue.TryDequeue(ref res));
        }

        [DataTestMethod]
        [Timeout(3000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        public void TryDequeueTest(Type clazz)
        {
            var queue = (IBlockingArrayQueue<int>) Activator.CreateInstance(clazz);
            queue.Enqueue(5);
            var res = 0;
            Assert.IsTrue(queue.TryDequeue(ref res));
            Assert.AreEqual(5, res);
            Assert.IsFalse(queue.TryDequeue(ref res));
        }

        [DataTestMethod]
        [Timeout(3000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        public void WaitDequeueTest(Type clazz)
        {
            var queue = (IBlockingArrayQueue<int>) Activator.CreateInstance(clazz);
            var wait = Task.Run(() => { Assert.AreEqual(10, queue.Dequeue()); });
            Thread.Sleep(1000);
            var notify = Task.Run(() => { queue.Enqueue(10); });
            wait.Wait();
            notify.Wait();
        }

        [DataTestMethod]
        [Timeout(3000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        public void MultipleThreadsTest(Type clazz)
        {
            var queue = (IBlockingArrayQueue<int>) Activator.CreateInstance(clazz);
            var listInserters = Enumerable
                .Range(0, 1000)
                .Select(x => Task.Run(() => queue.Enqueue(x)))
                .ToList();

            var listRemovers = Enumerable
                .Range(0, 1000)
                .Select(x => Task.Run(() => queue.Dequeue()))
                .ToList();

            listInserters.ForEach(t => t.Wait());
            listRemovers.ForEach(t => t.Wait());
        }
    }
}
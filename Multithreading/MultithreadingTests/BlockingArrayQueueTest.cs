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
        private IBlockingArrayQueue<int> CreateInstanceQueue(Type clazz)
        {
            var count = clazz.GetConstructors()[0].GetParameters().Length;
            return count == 0
                ? (IBlockingArrayQueue<int>) Activator.CreateInstance(clazz)
                : (IBlockingArrayQueue<int>) Activator.CreateInstance(clazz, 20);
        }

        [DataTestMethod]
        [Timeout(50000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(NewLockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        [DataRow(typeof(NewLockFreeArrayQueue<int>))]
        public void DequeueTest(Type clazz)
        {
            var queue = CreateInstanceQueue(clazz);
            queue.Enqueue(5);
            Assert.AreEqual(5, queue.Dequeue());
        }

        [DataTestMethod]
        [Timeout(50000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(NewLockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        [DataRow(typeof(NewLockFreeArrayQueue<int>))]
        public void ClearTest(Type clazz)
        {
            var queue = CreateInstanceQueue(clazz);
            queue.Enqueue(5);
            queue.Enqueue(3);
            queue.Enqueue(17);
            queue.Enqueue(21);
            queue.Clear();
            var res = 0;
            Assert.IsFalse(queue.TryDequeue(ref res));
        }

        [DataTestMethod]
        [Timeout(50000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(NewLockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        [DataRow(typeof(NewLockFreeArrayQueue<int>))]
        public void TryDequeueTest(Type clazz)
        {
            var queue = CreateInstanceQueue(clazz);
            queue.Enqueue(5);
            var res = 0;
            Assert.IsTrue(queue.TryDequeue(ref res));
            Assert.AreEqual(5, res);
            Assert.IsFalse(queue.TryDequeue(ref res));
        }

        [DataTestMethod]
        [Timeout(50000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(NewLockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        [DataRow(typeof(NewLockFreeArrayQueue<int>))]
        public void WaitDequeueTest(Type clazz)
        {
            var queue = CreateInstanceQueue(clazz);
            var wait = Task.Factory.StartNew(() => { Assert.AreEqual(10, queue.Dequeue()); });
            Thread.Sleep(1000);
            var notify = Task.Factory.StartNew(() => { queue.Enqueue(10); });
            wait.Wait();
            notify.Wait();
        }

        [DataTestMethod]
        [Timeout(50000)]
        [DataRow(typeof(LockBaseArrayQueue<int>))]
        [DataRow(typeof(NewLockBaseArrayQueue<int>))]
        [DataRow(typeof(LockFreeArrayQueue<int>))]
        [DataRow(typeof(NewLockFreeArrayQueue<int>))]
        public void MultipleThreadsTest(Type clazz)
        {
            var queue = CreateInstanceQueue(clazz);
            var listInserters = Enumerable
                .Range(0, 39)
                .Select(x => Task.Factory.StartNew(() => queue.Enqueue(x)))
                .ToList();

            var listRemovers = Enumerable
                .Range(0, 20)
                .Select(x => Task.Factory.StartNew(() => queue.Dequeue()))
                .ToList();

            listInserters.ForEach(t => t.Wait());
            listRemovers.ForEach(t => t.Wait());
        }
    }
}
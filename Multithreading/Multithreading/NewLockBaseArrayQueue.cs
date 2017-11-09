using System.Threading;

namespace Multithreading
{
    public class NewLockBaseArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly object _mutex = new object();
        private readonly T[] _queue;
        private int _head;
        private int _tail;

        public NewLockBaseArrayQueue(int n)
        {
            _queue = new T[n];
        }

        public void Enqueue(T e)
        {
            while (!TryEnqueue(e))
            {
                lock (_mutex)
                {
                    Monitor.Wait(_mutex);
                }
            }
        }

        public T Dequeue()
        {
            var ret = default(T);
            while (!TryDequeue(ref ret))
            {
                lock (_mutex)
                {
                    Monitor.Wait(_mutex);
                }
            }
            return ret;
        }

        public bool TryDequeue(ref T e)
        {
            lock (_mutex)
            {
                if (ToIndex(_head) == ToIndex(_tail))
                    return false;
                e = _queue[_tail++];
                Monitor.PulseAll(_mutex);
                return true;
            }
        }

        public bool TryEnqueue(T e)
        {
            lock (_mutex)
            {
                if (ToIndex(_head + 1) == ToIndex(_tail))
                    return false;
                _queue[ToIndex(_head++)] = e;
                Monitor.PulseAll(_mutex);
                return true;
            }
        }

        public void Clear()
        {
            lock (_mutex)
            {
                _tail = _head = 0;
            }
        }

        private int ToIndex(int c) => c % _queue.Length;
    }
}
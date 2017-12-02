using System.Threading;

namespace Multithreading
{
    public class NewLockBaseArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly object _mutex = new object();
        private readonly T[] _queue;
        private int _head;
        private SpinWait _spinner = new SpinWait();
        private int _tail;

        public NewLockBaseArrayQueue(int n) => _queue = new T[n];

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
            T ret;
            while (!TryDequeue(out ret))
            {
                lock (_mutex)
                {
                    Monitor.Wait(_mutex);
                }
            }
            return ret;
        }

        public bool TryDequeue(out T e)
        {
            lock (_mutex)
            {
                if (ToIndex(_head) == ToIndex(_tail))
                {
                    e = default(T);
                    return false;
                }

                e = _queue[ToIndex(_tail)];
                _queue[ToIndex(_tail++)] = default(T);
                Monitor.Pulse(_mutex);
                return true;
            }
        }

        public bool TryEnqueue(T e)
        {
            lock (_mutex)
            {
                if (ToIndex(_head + 1) == ToIndex(_tail))
                {
                    return false;
                }
                _queue[ToIndex(_head++)] = e;
                Monitor.Pulse(_mutex);
                return true;
            }
        }

        public void Clear()
        {
            while (TryDequeue(out var _))
            {
                _spinner.SpinOnce();
            }
        }

        private int ToIndex(int c) => c % _queue.Length;
    }
}
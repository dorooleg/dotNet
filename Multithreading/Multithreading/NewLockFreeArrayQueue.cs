using System.Threading;

namespace Multithreading
{
    public class NewLockFreeArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly T[] _queue;
        private int _head;
        private int _maxHead;
        private int _tail;

        public NewLockFreeArrayQueue(int n) => _queue = new T[n];

        public void Enqueue(T e)
        {
            while (!TryEnqueue(e))
            {
                Thread.Yield();
            }
        }

        public T Dequeue()
        {
            var ret = default(T);
            while (!TryDequeue(ref ret))
            {
                Thread.Yield();
            }
            return ret;
        }

        public bool TryDequeue(ref T e)
        {
            do
            {
                var tail = _tail;
                var maxHead = _maxHead;

                if (ToIndex(tail) == ToIndex(maxHead))
                {
                    return false;
                }

                e = _queue[ToIndex(tail)];

                if (Interlocked.CompareExchange(ref _tail, tail + 1, tail) == tail)
                {
                    return true;
                }
            } while (true);
        }

        public bool TryEnqueue(T e)
        {
            int head;

            do
            {
                head = _head;
                var tail = _tail;

                while (ToIndex(head + 1) == ToIndex(tail))
                {
                    return false;
                }

                _queue[ToIndex(head)] = e;
            } while (Interlocked.CompareExchange(ref _head, head + 1, head) != head);

            while (Interlocked.CompareExchange(ref _maxHead, head + 1, head) != head)
            {
                Thread.Yield();
            }

            return true;
        }

        public void Clear()
        {
            while (ToIndex(_tail) != ToIndex(_head))
            {
                var v = default(T);
                TryDequeue(ref v);
            }
        }

        private int ToIndex(int c) => c % _queue.Length;
    }
}
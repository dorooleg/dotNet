using System.Threading;

namespace Multithreading
{
    public class NewLockFreeArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly T[] _queue;
        private int _head;
        private int _maxHead;
        private SpinWait _spinner = new SpinWait();
        private int _tail;

        public NewLockFreeArrayQueue(int n) => _queue = new T[n];

        public void Enqueue(T e)
        {
            while (!TryEnqueue(e))
            {
                _spinner.SpinOnce();
            }
        }

        public T Dequeue()
        {
            T ret;
            while (!TryDequeue(out ret))
            {
                _spinner.SpinOnce();
            }
            return ret;
        }

        public bool TryDequeue(out T e)
        {
            do
            {
                var tail = _tail;
                var maxHead = _maxHead;

                if (ToIndex(tail) == ToIndex(maxHead))
                {
                    e = default(T);
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
                TryDequeue(out _);
            }
        }

        private int ToIndex(int c) => c % _queue.Length;
    }
}
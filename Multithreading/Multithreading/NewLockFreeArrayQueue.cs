using System.Threading;

namespace Multithreading
{
    public class NewLockFreeArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly T[] _queue;
        private int _head;
        private int _tail;

        public NewLockFreeArrayQueue(int n)
        {
            _queue = new T[n];
        }

        public void Enqueue(T e)
        {
            while (true)
            {
                var oldHead = _head;
                while ((oldHead + 1) % _queue.Length == _tail)
                {
                    oldHead = _head;
                }
                
                if (oldHead == -1)
                    continue;

                Interlocked.CompareExchange(ref _head, -1, oldHead);
                _queue[oldHead] = e;
                Interlocked.CompareExchange(ref _head, (oldHead + 1) % _queue.Length, -1);
                break;
            }
        }

        public T Dequeue()
        {
            while (true)
            {
                var oldTail = _tail;
                while (_head == _tail)
                {
                    oldTail = _tail;
                }

                if (oldTail == -1)
                    continue;

                Interlocked.CompareExchange(ref _tail, -1, oldTail);
                var res = _queue[oldTail];
                _queue[oldTail] = default(T);
                Interlocked.CompareExchange(ref _tail, (oldTail + 1) % _queue.Length, -1);
                return res;
            }
        }

        public bool TryDequeue(ref T e)
        {
            var oldTail = _tail;
            if (_head == _tail)
            {
                return false;
            }

            if (oldTail == -1)
                return false;

            Interlocked.CompareExchange(ref _tail, -1, oldTail);
            e = _queue[oldTail];
            _queue[oldTail] = default(T);
            Interlocked.CompareExchange(ref _tail, (oldTail + 1) % _queue.Length, -1);
            return true;
        }

        public bool TryEnqueue(T e)
        {
            var oldHead = _head;
            if ((oldHead + 1) % _queue.Length == _tail)
            {
                return false;
            }

            if (oldHead == -1)
                return false;
            Interlocked.CompareExchange(ref _head, -1, oldHead);
            _queue[oldHead] = e;
            Interlocked.CompareExchange(ref _head, (oldHead + 1) % _queue.Length, -1);
            return true;
        }

        public void Clear()
        {
            _head = _tail;
        }
    }
}
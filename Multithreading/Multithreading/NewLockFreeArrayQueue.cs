using System.Threading;

namespace Multithreading
{
    public class NewLockFreeArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly Boxed<T>[] _queue;
        private int _head;
        private int _tail;

        public NewLockFreeArrayQueue(int n)
        {
            _queue = new Boxed<T>[n];
        }

        public void Enqueue(T e)
        {
            while (true)
            {
                var oldHead = _head;
                while ((oldHead + 1) % _queue.Length == _tail)
                    oldHead = _head;

                if (Interlocked.CompareExchange(ref _queue[oldHead], new Boxed<T>(e), null) != null)
                    continue;
                _head = (_head + 1) % _queue.Length;
                break;
            }
        }

        public T Dequeue()
        {
            while (true)
            {
                var oldTail = _tail;
                while (_head == _tail)
                    oldTail = _tail;

                var res = _queue[oldTail];

                if (Interlocked.CompareExchange(ref _queue[oldTail], null, res) != res)
                    continue;
                _tail = (_tail + 1) % _queue.Length;
                return res.Value;
            }
        }

        public bool TryDequeue(ref T e)
        {
            var oldTail = _tail;
            if (_head == _tail)
                return false;

            var res = _queue[oldTail];

            if (Interlocked.CompareExchange(ref _queue[oldTail], null, res) != res)
                return false;
            _tail = (_tail + 1) % _queue.Length;
            e = res.Value;
            return true;
        }

        public bool TryEnqueue(T e)
        {
            var oldHead = _head;
            if ((oldHead + 1) % _queue.Length == _tail)
                return false;

            if (Interlocked.CompareExchange(ref _queue[oldHead], new Boxed<T>(e), null) != null)
                return false;
            _head = (_head + 1) % _queue.Length;
            return true;
        }

        public void Clear()
        {
            while (_tail != _head)
            {
                var v = default(T);
                TryDequeue(ref v);
            }
        }
    }
}
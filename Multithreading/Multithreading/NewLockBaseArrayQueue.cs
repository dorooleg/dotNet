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
            lock (_mutex)
            {
                while ((_head + 1) % _queue.Length == _tail)
                    Monitor.Wait(_mutex);

                _queue[_head] = e;
                _head = (_head + 1) % _queue.Length;
                Monitor.PulseAll(_mutex);
            }
        }

        public T Dequeue()
        {
            lock (_mutex)
            {
                while (_head == _tail)
                    Monitor.Wait(_mutex);
                var res = _queue[_tail];
                _queue[_tail] = default(T);
                _tail = (_tail + 1) % _queue.Length;
                Monitor.PulseAll(_mutex);
                return res;
            }
        }

        public bool TryDequeue(ref T e)
        {
            lock (_mutex)
            {
                if (_head == _tail)
                    return false;
                e = _queue[_tail];
                _queue[_tail] = default(T);
                _tail = (_tail + 1) % _queue.Length;
                Monitor.PulseAll(_mutex);
                return true;
            }
        }

        public bool TryEnqueue(T e)
        {
            lock (_mutex)
            {
                if ((_head + 1) % _queue.Length == _tail)
                    return false;

                _queue[_head] = e;
                _head = (_head + 1) % _queue.Length;
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
    }
}
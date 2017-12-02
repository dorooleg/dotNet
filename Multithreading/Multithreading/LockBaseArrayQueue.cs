using System.Collections.Generic;
using System.Threading;

namespace Multithreading
{
    public class LockBaseArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly List<T> _in = new List<T>();
        private readonly object _mutex = new object();
        private readonly List<T> _out = new List<T>();

        public void Enqueue(T e)
        {
            lock (_mutex)
            {
                _in.Add(e);
                Monitor.Pulse(_mutex);
            }
        }

        public T Dequeue()
        {
            lock (_mutex)
            {
                while (_in.Count == 0 && _out.Count == 0)
                {
                    Monitor.Wait(_mutex);
                }

                if (_out.Count == 0)
                {
                    _out.AddRange(_in);
                    _in.Clear();
                }

                var last = _out[_out.Count - 1];
                _out.RemoveAt(_out.Count - 1);
                return last;
            }
        }

        public bool TryDequeue(out T e)
        {
            lock (_mutex)
            {
                if (_out.Count == 0)
                {
                    _out.AddRange(_in);
                    _in.Clear();
                }

                if (_out.Count == 0)
                {
                    e = default(T);
                    return false;
                }

                var last = _out[_out.Count - 1];
                _out.RemoveAt(_out.Count - 1);
                e = last;
                return true;
            }
        }

        public bool TryEnqueue(T e)
        {
            Enqueue(e);
            return true;
        }

        public void Clear()
        {
            lock (_mutex)
            {
                _in.Clear();
                _out.Clear();
            }
        }
    }
}
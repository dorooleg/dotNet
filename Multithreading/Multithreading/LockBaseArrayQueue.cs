using System.Collections;
using System.Threading;

namespace Multithreading
{
    public class LockBaseArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private readonly ArrayList _in = new ArrayList();
        private readonly object _mutex = new object();
        private readonly ArrayList _out = new ArrayList();

        public void Enqueue(T e)
        {
            lock (_mutex)
            {
                _in.Add(e);
                Monitor.PulseAll(_mutex);
            }
        }

        public T Dequeue()
        {
            lock (_mutex)
            {
                while (_in.Count == 0 && _out.Count == 0)
                    Monitor.Wait(_mutex);

                if (_out.Count == 0)
                {
                    _out.AddRange(_in);
                    _in.Clear();
                }

                var last = _out[_out.Count - 1];
                _out.RemoveAt(_out.Count - 1);
                return (T) last;
            }
        }

        public bool TryDequeue(ref T e)
        {
            lock (_mutex)
            {
                if (_out.Count == 0)
                {
                    _out.AddRange(_in);
                    _in.Clear();
                }

                if (_out.Count == 0)
                    return false;

                var last = _out[_out.Count - 1];
                _out.RemoveAt(_out.Count - 1);
                e = (T) last;
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
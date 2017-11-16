using System.Threading;

namespace Multithreading
{
    public class LockFreeArrayQueue<T> : IBlockingArrayQueue<T>
    {
        private Boxed<ImmutableQueue<T>> _queue = Boxed.Of(new ImmutableQueue<T>());

        public void Enqueue(T e)
        {
            while (true)
            {
                var q = _queue;
                var @new = Boxed.Of(q.Value.Enqueue(e));
                if (Interlocked.CompareExchange(ref _queue, @new, q) == q)
                {
                    return;
                }
            }
        }

        public T Dequeue()
        {
            while (true)
            {
                var q = _queue;
                while (q.Value.IsEmpty)
                {
                    q = _queue;
                }
                var @new = Boxed.Of(q.Value.Dequeue(out var res));
                if (Interlocked.CompareExchange(ref _queue, @new, q) == q)
                {
                    return res;
                }
            }
        }

        public bool TryDequeue(ref T e)
        {
            var q = _queue;
            if (q.Value.IsEmpty)
            {
                e = default(T);
                return false;
            }
            var @new = Boxed.Of(q.Value.Dequeue(out var res));
            if (Interlocked.CompareExchange(ref _queue, @new, q) != q)
            {
                return false;
            }
            e = res;
            return true;
        }

        public bool TryEnqueue(T e)
        {
            var q = _queue;
            var @new = Boxed.Of(q.Value.Enqueue(e));
            return Interlocked.CompareExchange(ref _queue, @new, q) == q;
        }

        public void Clear()
        {
            while (true)
            {
                var q = _queue;
                var @new = Boxed.Of(new ImmutableQueue<T>());
                if (Interlocked.CompareExchange(ref _queue, @new, q) == q)
                {
                    return;
                }
            }
        }
    }
}
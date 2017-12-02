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
                var value = Boxed.Of(q.Value.Enqueue(e));
                if (Interlocked.CompareExchange(ref _queue, value, q) == q)
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
                var value = Boxed.Of(q.Value.Dequeue(out var res));
                if (Interlocked.CompareExchange(ref _queue, value, q) == q)
                {
                    return res;
                }
            }
        }

        public bool TryDequeue(out T e)
        {
            while (true)
            {
                var q = _queue;
                if (q.Value.IsEmpty)
                {
                    e = default(T);
                    return false;
                }
                var value = Boxed.Of(q.Value.Dequeue(out var res));
                if (Interlocked.CompareExchange(ref _queue, value, q) == q)
                {
                    e = res;
                    return true;
                }
            }
        }

        public bool TryEnqueue(T e)
        {
            var q = _queue;
            var value = Boxed.Of(q.Value.Enqueue(e));
            return Interlocked.CompareExchange(ref _queue, value, q) == q;
        }

        public void Clear()
        {
            while (true)
            {
                var q = _queue;
                var value = Boxed.Of(new ImmutableQueue<T>());
                if (Interlocked.CompareExchange(ref _queue, value, q) == q)
                {
                    return;
                }
            }
        }
    }
}
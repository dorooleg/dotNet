using System.Diagnostics.Contracts;

namespace Multithreading
{
    public struct ImmutableQueue<T>
    {
        private readonly ImmutableStack<T> _incoming;
        private readonly ImmutableStack<T> _outgoing;

        private ImmutableQueue(ImmutableStack<T> incoming, ImmutableStack<T> outgoing)
        {
            _incoming = incoming;
            _outgoing = outgoing;
        }

        private static void Refill(ref ImmutableStack<T> o, ref ImmutableStack<T> i)
        {
            if (!o.IsEmpty)
            {
                return;
            }

            while (!i.IsEmpty)
            {
                i = i.Pop(out var value);
                o = o.Push(value);
            }
        }


        #region API

        [Pure]
        public bool IsEmpty => _incoming.IsEmpty && _outgoing.IsEmpty;

        [Pure]
        public ImmutableQueue<T> Dequeue(out T res)
        {
            var i = _incoming;
            var o = _outgoing;

            Refill(ref o, ref i);

            if (!o.IsEmpty)
            {
                return new ImmutableQueue<T>(i, o.Pop(out res));
            }
            res = default(T);
            return this;
        }

        [Pure]
        public ImmutableQueue<T> Enqueue(T res) => new ImmutableQueue<T>(_incoming.Push(res), _outgoing);

        #endregion
    }
}
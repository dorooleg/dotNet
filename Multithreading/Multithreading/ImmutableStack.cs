using System;
using System.Diagnostics.Contracts;

namespace Multithreading
{
    public struct ImmutableStack<T>
    {
        private class Entry
        {
            internal readonly Entry Next;
            internal readonly T Value;

            internal Entry(T value, Entry next)
            {
                Value = value;
                Next = next;
            }
        }

        private readonly Entry _head;

        private ImmutableStack(Entry head) => _head = head;


        #region API

        [Pure]
        public bool IsEmpty => _head == null;

        [Pure]
        public ImmutableStack<T> Push(T value) => new ImmutableStack<T>(new Entry(value, _head));

        [Pure]
        public ImmutableStack<T> Pop(out T value)
        {
            if (_head == null)
            {
                throw new InvalidOperationException("head is null");
            }
            value = _head.Value;
            return new ImmutableStack<T>(_head.Next);
        }

        #endregion
    }
}
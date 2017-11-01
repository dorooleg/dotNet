using System;
using JetBrains.Annotations;

namespace Option
{
    public class Option<T>
    {
        private readonly T _value;

        private Option(T value)
        {
            _value = value;
            IsNone = false;
        }

        private Option()
        {
        }

        static Option()
        {
            None = new Option<T>();
        }

        public static Option<T> Some(T value) => new Option<T>(value);

        public static Option<T> None { get; }

        public static Option<T> Flatten([NotNull] Option<Option<T>> option) => option.IsNone ? None : option.Value;

        public bool IsSome => !IsNone;

        public bool IsNone { get; } = true;

        public T Value => IsNone ? throw new InvalidOperationException() : _value;

        public Option<TU> Map<TU>([NotNull] Func<T, TU> f) => IsNone ? Option<TU>.None : Option<TU>.Some(f(Value));

        public override bool Equals(object obj)
            => obj is Option<T> && this == (Option<T>) obj;

        public override int GetHashCode()
            => IsNone || Value == null ? 0 : Value.GetHashCode();

        public static bool operator ==([NotNull] Option<T> o1, [NotNull] Option<T> o2)
            => o1.IsNone && o2.IsNone || !o1.IsNone && !o2.IsNone && Equals(o1.Value, o2.Value);

        public static bool operator !=(Option<T> o1, Option<T> o2) => !(o1 == o2);
    }


}

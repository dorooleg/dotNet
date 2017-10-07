using System;
using JetBrains.Annotations;

namespace Option
{
    public class Option<T>
    {
        private static readonly Option<T> NoneField = new Option<T>();

        private readonly T _value;
        private readonly bool _empty; 

        private Option(T value)
        {
            _value = value;
            _empty = false;
        }

        private Option()
        {
            _empty = true;
        }

        public static Option<T> Some([NotNull] T value) => new Option<T>(value);

        public static Option<T> None() => NoneField;

        public static Option<T> Flatten(Option<Option<T>> option)
            => option.IsNone() ? None() : option.Value();

        public bool IsSome() => !_empty;

        public bool IsNone() => _empty;

        public T Value() => IsNone() ? throw new InvalidOperationException() : _value;

        public Option<TU> Map<TU>(Func<T, TU> f) => IsNone() ? Option<TU>.None() : Option<TU>.Some(f(Value()));
    }
}

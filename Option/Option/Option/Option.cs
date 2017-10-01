namespace OptionNamespace
{
    using System;
    using JetBrains.Annotations;

    public class Option<T>
    {
        private static Option<T> none = new Option<T>();

        private T value;
        private bool empty; 

        private Option(T value)
        {
            this.value = value;
            empty = false;
        }

        private Option()
        {
            empty = true;
        }

        public static Option<T> Some([NotNull] T value) => new Option<T>(value);

        public static Option<T> None() => none;

        public static Option<T> Flatten(Option<Option<T>> option)
            => option.IsNone() ? None() : option.Value();

        public bool IsSome() => !empty;

        public bool IsNone() => empty;

        public T Value() => IsNone() ? throw new InvalidOperationException() : value;

        public Option<U> Map<U>(Func<T, U> f) => IsNone() ? Option<U>.None() : Option<U>.Some(f(Value()));
    }
}

namespace Multithreading
{
    public static class Boxed
    {
        public static Boxed<T> Of<T>(T value) => new Boxed<T>(value);
    }

    public class Boxed<T>
    {
        public Boxed(T value) => Value = value;

        public T Value { get; }
    }
}
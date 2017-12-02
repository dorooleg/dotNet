namespace Multithreading
{
    public interface IBlockingArrayQueue<T>
    {
        void Enqueue(T e);
        T Dequeue();
        bool TryDequeue(out T e);
        bool TryEnqueue(T e);
        void Clear();
    }
}
using System.Threading;

namespace Multithreading
{
    internal class Fork
    {
        private static int _globalId;

        public int Id { get; } = Interlocked.Increment(ref _globalId);
    }
}
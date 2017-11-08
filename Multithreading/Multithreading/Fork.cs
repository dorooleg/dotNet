namespace Multithreading
{
    internal class Fork
    {
        private static uint _globalId;

        public uint GetId { get; } = _globalId++;
    }
}
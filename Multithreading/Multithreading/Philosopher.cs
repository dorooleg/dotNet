using System;
using System.Threading;

namespace Multithreading
{
    internal class Philosopher
    {
        private static int _globalId;
        private readonly Fork _firstFork;

        private readonly Random _rnd = new Random();
        private readonly Fork _secondFork;
        private int _balanceEat;

        public Philosopher(Fork firstFork, Fork secondFork)
        {
            _firstFork = firstFork;
            _secondFork = secondFork;

            if (_firstFork.Id > _secondFork.Id)
            {
                (_firstFork, _secondFork) = (_secondFork, _firstFork);
            }
        }

        public int Id { get; } = Interlocked.Increment(ref _globalId);

        public bool IsDone => _balanceEat == 0;

        public void Eat()
        {
            Interlocked.Increment(ref _balanceEat);
            lock (_firstFork)
            {
                lock (_secondFork)
                {
                    Console.WriteLine("Eat: " + Id);
                    Thread.Sleep(_rnd.Next(300, 1000));
                }
            }
            Interlocked.Decrement(ref _balanceEat);
        }
    }
}
using System;
using System.Threading;

namespace Multithreading
{
    internal class Philosopher
    {
        private static uint _globalId;
        private readonly Fork _leftHand;
        private readonly Fork _rightHand;

        private readonly Random _rnd = new Random();

        public Philosopher(Fork leftHand, Fork rightHand)
        {
            _leftHand = leftHand;
            _rightHand = rightHand;

            if (_leftHand.GetId > _rightHand.GetId)
                (_leftHand, _rightHand) = (_rightHand, _leftHand);
        }

        public uint GetId { get; } = _globalId++;

        public void Eat()
        {
            lock (_leftHand)
            {
                lock (_rightHand)
                {
                    Console.WriteLine("Eat: " + GetId);
                    Thread.Sleep(_rnd.Next(300, 1000));
                }
            }
        }
    }
}
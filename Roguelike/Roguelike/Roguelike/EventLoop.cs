using System;
using System.Threading.Tasks;
using static System.ConsoleKey;

namespace Roguelike
{
    internal class EventLoop
    {
        public event Action<(int dx, int dy)> MoveHandler;

        public void Run()
        {
            Console.CancelKeyPress += (sender, e) => { Environment.Exit(0); };

            var readKeyTask = new Task(ReadKeys);
            readKeyTask.Start();
            Task.WaitAll(readKeyTask);
        }

        private void ReadKeys()
        {
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
                switch (keyinfo.Key)
                {
                    case LeftArrow:
                        MoveHandler?.Invoke((0, -1));
                        break;
                    case RightArrow:
                        MoveHandler?.Invoke((0, 1));
                        break;
                    case UpArrow:
                        MoveHandler?.Invoke((-1, 0));
                        break;
                    case DownArrow:
                        MoveHandler?.Invoke((1, 0));
                        break;
                }
            } while (!Console.KeyAvailable && keyinfo.Key != Escape);
        }
    }
}
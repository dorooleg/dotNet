using System;
using static System.ConsoleKey;

namespace Roguelike
{
    internal class EventLoop
    {
        public event Action<int, int> MoveHandler;

        public void Run()
        {
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
                switch (keyinfo.Key)
                {
                    case LeftArrow:
                        MoveHandler?.Invoke(0, -1);
                        break;
                    case RightArrow:
                        MoveHandler?.Invoke(0, 1);
                        break;
                    case UpArrow:
                        MoveHandler?.Invoke(-1, 0);
                        break;
                    case DownArrow:
                        MoveHandler?.Invoke(1, 0);
                        break;
                }
            } while (keyinfo.Key != Escape);
        }
    }
}
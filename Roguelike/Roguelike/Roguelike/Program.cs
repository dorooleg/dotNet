using System;

namespace Roguelike
{
    internal class Program
    {
        private static void Main()
        {
            Console.CursorVisible = false;
            var world = new World("origin.world");
            if (!world.Validate())
            {
                Console.WriteLine("World validation failed");
                return;
            }
            var eventLoop = new EventLoop();
            eventLoop.MoveHandler += world.MoveTo;
            eventLoop.Run();
        }
    }
}
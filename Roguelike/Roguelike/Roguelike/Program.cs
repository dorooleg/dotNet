using System;

namespace Roguelike
{
    internal class Program
    {
        private static void Main()
        {
            Console.CursorVisible = false;
            var world = new World(@"../../origin.world");
            var eventLoop = new EventLoop();
            eventLoop.MoveHandler += world.MoveTo;
            eventLoop.Run();
        }
    }
}
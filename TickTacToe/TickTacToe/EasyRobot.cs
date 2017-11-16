using System;

namespace TickTacToe
{
    public class EasyRobot : IPlayer
    {
        private static readonly Random Rand = new Random();
        private readonly GameBoard _board;
        private readonly Action<int, int> _updater;

        public EasyRobot(GameBoard board, Action<int, int> updater)
        {
            _board = board;
            _updater = updater;
        }

        public bool Update(int unused, int unsued)
        {
            var list = _board.GetListEmpty();

            if (list.Count == 0)
            {
                return false;
            }

            (int x, int y) = list[Rand.Next(list.Count)];
            _board.Update(x, y);
            _updater(x, y);
            return true;
        }
    }
}
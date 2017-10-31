using System;
using System.Linq;

namespace TickTacToe
{
    public interface IPlayer
    {
        bool Update(int x, int y);
    }

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
                return false;

            (int x, int y) = list[Rand.Next(list.Count)];
            _board.Update(x, y);
            _updater(x, y);
            return true;
        }
    }

    public class MediumRobot : IPlayer
    {
        private static readonly Random Rand = new Random();
        private readonly GameBoard _board;
        private readonly Action<int, int> _updater;

        public MediumRobot(GameBoard board, Action<int, int> updater)
        {
            _board = board;
            _updater = updater;
        }

        public bool Update(int unused, int unsued)
        {
            var list = _board.GetListEmpty();

            if (list.Count == 0)
                return false;

            var p = list
                .Cast<(int, int)?>()
                .FirstOrDefault(t => t != null && _board.IfSetThenEnd(t.Value.Item1, t.Value.Item2));

            (int x, int y) = p ?? list[Rand.Next(list.Count)];
            _board.Update(x, y);
            _updater(x, y);
            return true;
        }
    }

    public class Player : IPlayer
    {
        private readonly GameBoard _board;
        private readonly Action<bool> _updater;

        public Player(GameBoard board, Action<bool> actionAfterUpdate)
        {
            _board = board;
            _updater = actionAfterUpdate;
        }

        public bool Update(int x, int y)
        {
            var ret = _board.Update(x, y);
            _updater(ret);
            return ret;
        }
    }
}
using System;

namespace TickTacToe
{
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
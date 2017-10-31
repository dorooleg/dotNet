using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TickTacToe
{
    public class GameBoard
    {
        public enum BoardElement
        {
            X = 'x',
            O = 'o',
            _ = ' '
        }

        public static readonly int Size = 3;

        private readonly List<BoardElement[]> _board =
            new List<BoardElement[]>(from int i in Enumerable.Range(0, Size)
                select Enumerable.Range(0, Size).Select(_ => BoardElement._).ToArray());

        private BoardElement _currentElement = BoardElement.X;

        public bool WinX { get; set; }
        public bool WinO { get; set; }
        public bool Draw { get; set; }
        public bool EndGame { get; set; }

        private static void Validate(int x, int y)
        {
            Assert.Less(x, Size);
            Assert.Less(y, Size);
            Assert.GreaterOrEqual(x, 0);
            Assert.GreaterOrEqual(y, 0);
        }

        public BoardElement GetElement(int x, int y)
        {
            Validate(x, y);
            return _board[x][y];
        }

        public bool Update(int x, int y)
        {
            Validate(x, y);

            if (EndGame || _board[x][y] != BoardElement._)
            {
                return false;
            }

            _board[x][y] = _currentElement;
            SwapCurrentElement();
            UpdateEndGame();

            return true;
        }

        private bool CheckDraw()
            => !(from BoardElement[] elements in _board
                 from BoardElement element in elements
                 where element == BoardElement._
                 select element).Any();

        private void UpdateDraw()
        {
            UpdateWinX();
            UpdateWinO();

            if (WinX || WinO)
            {
                return;
            }

            Draw = CheckDraw();
        }

        public bool IfSetThenEnd(int x, int y)
        {
            Validate(x, y);

            if (EndGame)
                return true;

            if (_board[x][y] != BoardElement._)
            {
                return false;
            }

            _board[x][y] = _currentElement;
            var result = CheckDraw() || CheckWin(BoardElement.X) || CheckWin(BoardElement.O);
            _board[x][y] = BoardElement._;

            return result;
        }

        public List<(int, int)> GetListEmpty()
            => Enumerable.Range(0, Size)
                    .SelectMany(x => Enumerable.Range(0, Size)
                    .Where(y => _board[x][y] == BoardElement._)
                    .Select(y => (x, y))).ToList();

        private void UpdateEndGame()
        {
            UpdateWinX();
            UpdateWinO();
            UpdateDraw();
            EndGame = WinO || WinX || Draw;
        }

        private bool CheckWin(BoardElement element)
                => Enumerable.Range(0, Size).Any(i => _board[i].All(x => x == element)) ||
                   Enumerable.Range(0, Size).Any(i => _board.TrueForAll(x => x[i] == element)) ||
                   Enumerable.Range(0, Size).All(i => _board[i][i] == element) ||
                   Enumerable.Range(0, Size).All(i => _board[i][Size - i - 1] == element);

        private void UpdateWinX()
            => WinX = CheckWin(BoardElement.X);

        private void UpdateWinO()
            => WinO = CheckWin(BoardElement.O);

        private void SwapCurrentElement()
            => _currentElement = _currentElement != BoardElement.X ? BoardElement.X : BoardElement.O;
    }
}
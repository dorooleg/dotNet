using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TickTacToe;

namespace TicTacToeTest
{
    [TestClass]
    public class GameBoardTest
    {
        private GameBoard _board;

        [TestInitialize]
        public void Initalize()
        {
            _board = new GameBoard();
        }

        [TestMethod]
        public void EmptyTest()
        {
            for (var x = 0; x < GameBoard.Size; x++)
            {
                for (var y = 0; y < GameBoard.Size; y++)
                {
                    Assert.AreEqual(GameBoard.BoardElement._, _board.GetElement(x, y));
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            _board.Update(0, 0);
            Assert.AreEqual(GameBoard.BoardElement.X, _board.GetElement(0, 0));
        }

        [TestMethod]
        public void WinXTest()
        {
            _board.Update(0, 0);
            _board.Update(1, 0);
            _board.Update(0, 1);
            _board.Update(1, 1);
            _board.Update(0, 2);
            _board.Update(1, 2);
            Assert.IsTrue(_board.WinX);
            Assert.IsFalse(_board.WinO);
            Assert.IsFalse(_board.Draw);
            Assert.IsTrue(_board.EndGame);
        }

        [TestMethod]
        public void WinOTest()
        {
            _board.Update(0, 0);
            _board.Update(1, 0);
            _board.Update(0, 1);
            _board.Update(1, 1);
            _board.Update(2, 2);
            _board.Update(1, 2);
            Assert.IsFalse(_board.WinX);
            Assert.IsTrue(_board.WinO);
            Assert.IsFalse(_board.Draw);
            Assert.IsTrue(_board.EndGame);
        }

        [TestMethod]
        public void DrawTest()
        {
            _board.Update(0, 0);
            _board.Update(1, 0);
            _board.Update(0, 1);
            _board.Update(1, 1);
            _board.Update(1, 2);
            _board.Update(0, 2);
            _board.Update(2, 0);
            _board.Update(2, 1);
            _board.Update(2, 2);
            Assert.IsFalse(_board.WinX);
            Assert.IsFalse(_board.WinO);
            Assert.IsTrue(_board.Draw);
            Assert.IsTrue(_board.EndGame);
        }

        [TestMethod]
        public void IfSetThenEndTest()
        {
            _board.Update(0, 0);
            _board.Update(1, 0);
            _board.Update(0, 1);
            _board.Update(1, 1);
            _board.Update(2, 2);
            Assert.IsTrue(_board.IfSetThenEnd(1, 2));
        }

        [TestMethod]
        public void GetListEmptyTest()
        {
            _board.Update(0, 0);
            _board.Update(1, 0);
            _board.Update(0, 1);
            _board.Update(1, 1);
            _board.Update(0, 2);
            _board.Update(1, 2);
            Assert.IsTrue(_board.GetListEmpty().Count == 4);
            Assert.IsTrue(_board.GetListEmpty().Contains((2, 0)));
            Assert.IsTrue(_board.GetListEmpty().Contains((2, 1)));
            Assert.IsTrue(_board.GetListEmpty().Contains((2, 2)));
            Assert.IsTrue(_board.GetListEmpty().Contains((1, 2)));
        }
    }
}

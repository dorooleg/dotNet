using Microsoft.VisualStudio.TestTools.UnitTesting;
using TickTacToe;

namespace TicTacToeTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void UpdateTest()
        {
            var i = 0;
            var board = new GameBoard();
            var player = new Player(board, _ => i++);
            player.Update(0, 0);
            player.Update(0, 0);
            Assert.AreEqual(2, i);
            Assert.AreEqual(GameBoard.BoardElement.X, board.GetElement(0, 0));
        }
    }
}
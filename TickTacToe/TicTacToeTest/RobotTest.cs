using Microsoft.VisualStudio.TestTools.UnitTesting;
using TickTacToe;

namespace TicTacToeTest
{
    [TestClass]
    public class RobotTest
    {
        [TestMethod]
        public void EasyRobotUpdateTest()
        {
            var i = 0;
            var board = new GameBoard();
            var robot = new EasyRobot(board, (_, __) => i++);
            robot.Update(0, 0);
            robot.Update(0, 0);
            Assert.AreEqual(2, i);
            var result = false;
            for (var x = 0; x < GameBoard.Size; x++)
            for (var y = 0; y < GameBoard.Size; y++)
                result = result || board.GetElement(x, y) == GameBoard.BoardElement.X;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MediumRobotUpdateTest()
        {
            var i = 0;
            var board = new GameBoard();
            var robot = new MediumRobot(board, (_, __) => i++);
            robot.Update(0, 0);
            robot.Update(0, 0);
            Assert.AreEqual(2, i);
            var result = false;
            for (var x = 0; x < GameBoard.Size; x++)
            for (var y = 0; y < GameBoard.Size; y++)
                result = result || board.GetElement(x, y) == GameBoard.BoardElement.X;
            Assert.IsTrue(result);
        }
    }
}
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roguelike;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace RoguelikeTest
{
    [TestClass]
    public class BaseTests
    {
        [DataTestMethod]
        [DataRow("empty.world")]
        [DataRow("no_player.world")]
        [DataRow("broken_wall_top.world")]
        [DataRow("broken_wall_left.world")]
        [DataRow("broken_wall_right.world")]
        [DataRow("broken_wall_down.world")]
        [DataRow("multiple_player.world")]
        [DataRow("length.world")]
        public void InvalidMap(string filename)
        {
            var world = new World(filename);
            Assert.IsFalse(world.Validate());
        } 

        [TestMethod]
        public void Origin()
        {
            var world = new World("origin.world");
            Assert.IsTrue(world.Validate());
            var x = world.PlayerPosition.x;
            var y = world.PlayerPosition.y;
            Assert.AreEqual(2, x);
            Assert.AreEqual(10, y);
        }

        [TestMethod]
        public void Move()
        {
            var world = new World("origin.world");
            Assert.IsTrue(world.Validate());
            world.MoveTo((-1, 0));
            var x = world.PlayerPosition.x;
            var y = world.PlayerPosition.y;
            Assert.AreEqual(1, x);
            Assert.AreEqual(10, y);
        }
    }
}

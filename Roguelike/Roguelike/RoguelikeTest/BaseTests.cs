using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Roguelike;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace RoguelikeTest
{
    [TestClass]
    public class BaseTests
    {
        [DataTestMethod]
        [DataRow(@"../../empty.world")]
        [DataRow(@"../../no_player.world")]
        [DataRow(@"../../broken_wall_top.world")]
        [DataRow(@"../../broken_wall_left.world")]
        [DataRow(@"../../broken_wall_right.world")]
        [DataRow(@"../../broken_wall_down.world")]
        [DataRow(@"../../multiple_player.world")]
        [DataRow(@"../../length.world")]
        [ExpectedException(typeof(InvalidDataException))]
        // ReSharper disable once ObjectCreationAsStatement
        public void InvalidMap(string filename) => new World(filename);

        [TestMethod]
        public void Origin()
        {
            var world = new World(@"../../origin.world");
            var getX = world.GetType().GetMethod("GetPlayerX", BindingFlags.NonPublic | BindingFlags.Instance);
            var getY = world.GetType().GetMethod("GetPlayerY", BindingFlags.NonPublic | BindingFlags.Instance);
            var x = (int)getX.Invoke(world, null);
            var y = (int)getY.Invoke(world, null);
            Assert.AreEqual(2, x);
            Assert.AreEqual(10, y);
        }

        [TestMethod]
        public void Move()
        {
            var world = new World(@"../../origin.world");
            var getX = world.GetType().GetMethod("GetPlayerX", BindingFlags.NonPublic | BindingFlags.Instance);
            var getY = world.GetType().GetMethod("GetPlayerY", BindingFlags.NonPublic | BindingFlags.Instance);
            world.MoveTo(-1, 0);
            var x = (int)getX.Invoke(world, null);
            var y = (int)getY.Invoke(world, null);
            Assert.AreEqual(1, x);
            Assert.AreEqual(10, y);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void MoveInvalid()
        {
            var world = new World(@"../../origin.world");
            world.MoveTo(-2, 0);
        }
    }
}

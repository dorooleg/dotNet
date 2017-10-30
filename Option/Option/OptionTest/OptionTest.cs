using Option;

namespace OptionTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OptionTest
    {
        [TestMethod]
        public void Some()
        {
            var value = Option<int>.Some(10);
            Assert.AreEqual(10, value.Value);
        }

        [TestMethod]
        public void None()
        {
            var value = Option<double>.None;
            Assert.IsTrue(value.IsNone);
        }

        [TestMethod]
        public void MapIntToInt()
        {
            Assert.AreEqual(4, Option<int>.Some(2).Map(x => x * 2).Value);
        }

        [TestMethod]
        public void MapIntToDouble()
        {
            Assert.AreEqual(6.0, Option<int>.Some(2).Map(x => x * 3.0).Value);
        }

        [TestMethod]
        public void Flatten()
        {
            var value = Option<Option<int>>.Some(Option<int>.Some(5));
            var flat = Option<int>.Flatten(value);
            Assert.AreEqual(typeof(Option<int>), flat.GetType());
            Assert.AreEqual(5, flat.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Value()
        {
            var value = Option<List<int>>.None;
            var unused = value.Value;
        }

        [TestMethod]
        public void IsSome()
        {
            var value = Option<double>.Some(3.0);
            Assert.IsTrue(value.IsSome);
        }

        [TestMethod]
        public void IsNone()
        {
            var value = Option<double>.None;
            Assert.IsTrue(value.IsNone);
        }

        [TestMethod]
        public void SomeEqualsSomeTest()
        {
            Assert.AreEqual(Option<int>.Some(2).Map(x => x * 2), Option<int>.Some(4));
        }

        [TestMethod]
        public void SomeEqualNoneTest()
        {
            Assert.AreEqual(Option<int>.None, Option<int>.None);
        }

        [TestMethod]
        public void SomeNotEqualsNoneTest()
        {
            Assert.AreNotEqual(Option<int>.None, Option<int>.Some(4));
        }

        [TestMethod]
        public void SomeNotEqualsSomeTest()
        {
            Assert.AreNotEqual(Option<int>.Some(2).Map(x => x * 2), Option<int>.Some(5));
        }

        [TestMethod]
        public void OperatorEqualsTest()
        {
            Assert.IsTrue(Option<int>.Some(2).Map(x => x * 2) == Option<int>.Some(4));
        }

        [TestMethod]
        public void OperatorEqualsNoneTest()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(Option<int>.None == Option<int>.None);
        }

        [TestMethod]
        public void OperatorNotEqualsNoneTest()
        {
            Assert.IsFalse(Option<int>.Some(2).Map(x => x * 2) == Option<int>.None);
        }

        [TestMethod]
        public void OperatorNotEqualsTest()
        {
            Assert.IsFalse(Option<int>.Some(2).Map(x => x * 2) == Option<int>.Some(5));
        }

        [TestMethod]
        public void HashCodeTest()
        {
            Assert.IsTrue(Option<int>.Some(2).Map(x => x * 2).GetHashCode() == Option<int>.Some(4).GetHashCode());
        }

        [TestMethod]
        public void NoneMap()
        {
            Assert.AreEqual(Option<int>.None, Option<int>.None.Map(x => x * 2));
        }

        [TestMethod]
        public void NoneFlatten()
        {
            var valueNone = Option<Option<int>>.None;
            var flatNone = Option<int>.Flatten(valueNone);
            Assert.AreEqual(typeof(Option<int>), flatNone.GetType());
            Assert.IsTrue(flatNone.IsNone);
        }
    }
}

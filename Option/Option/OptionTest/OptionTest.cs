namespace OptionTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OptionNamespace;

    [TestClass]
    public class OptionTest
    {
        [TestMethod]
        public void Some()
        {
            var value = Option<int>.Some(10);
            Assert.AreEqual(10, value.Value());
        }

        [TestMethod]
        public void None()
        {
            var value = Option<double>.None();
            Assert.IsTrue(value.IsNone());
        }

        [TestMethod]
        public void Map()
        {
            Assert.AreEqual(4, Option<int>.Some(2).Map(x => x * 2).Value());
            Assert.AreEqual(Option<int>.None(), Option<int>.None().Map(x => x * 2));
            Assert.AreEqual(6.0, Option<int>.Some(2).Map(x => x * 3.0).Value());
        }

        [TestMethod]
        public void Flatten()
        {
            var value = Option<Option<int>>.Some(Option<int>.Some(5));
            var flat = Option<int>.Flatten(value);
            Assert.AreEqual(typeof(Option<int>), flat.GetType());
            Assert.AreEqual(5, flat.Value());

            var valueNone = Option<Option<int>>.None();
            var flatNone = Option<int>.Flatten(valueNone);
            Assert.AreEqual(typeof(Option<int>), flatNone.GetType());
            Assert.IsTrue(flatNone.IsNone());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Value()
        {
            var value = Option<List<int>>.None();
            value.Value();
        }

        [TestMethod]
        public void IsSome()
        {
            var value = Option<double>.Some(3.0);
            Assert.IsTrue(value.IsSome());
        }

        [TestMethod]
        public void IsNone()
        {
            var value = Option<double>.None();
            Assert.IsTrue(value.IsNone());
        }
    }
}

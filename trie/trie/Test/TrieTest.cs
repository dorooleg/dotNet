namespace TrieDataSctructureTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TrieDataSctructure;

    [TestClass]
    public class TrieTest
    {
        private Trie trie;

        [TestInitialize]
        public void Initialize()
        {
            trie = new Trie();
        }

        [TestMethod]
        public void EmptyInsert()
        {
            Assert.IsTrue(trie.Add(string.Empty));
            Assert.IsTrue(trie.Contains(string.Empty));
        }

        [TestMethod]
        public void SimpleInsert()
        {
            Assert.IsTrue(trie.Add("common"));
            Assert.IsTrue(trie.Contains("common"));
            Assert.IsFalse(trie.Contains("commo"));
            Assert.IsFalse(trie.Contains("comm"));
            Assert.IsFalse(trie.Contains("com"));
            Assert.IsFalse(trie.Contains("co"));
            Assert.IsFalse(trie.Contains("c"));
            Assert.IsFalse(trie.Contains("commona"));
            Assert.IsFalse(trie.Contains("commonn"));
        }

        [TestMethod]
        public void SimpleRemove()
        {
            Assert.IsTrue(trie.Add("common"));
            Assert.IsFalse(trie.Remove("commona"));
            Assert.IsTrue(trie.Remove("common"));
            Assert.IsFalse(trie.Contains("common"));
            Assert.IsFalse(trie.Contains("commo"));
            Assert.IsFalse(trie.Contains("comm"));
            Assert.IsFalse(trie.Contains("com"));
            Assert.IsFalse(trie.Contains("co"));
            Assert.IsFalse(trie.Contains("c"));
            Assert.IsFalse(trie.Contains("commona"));
            Assert.IsFalse(trie.Contains("commonn"));
        }

        [TestMethod]
        public void SimpleSize()
        {
            Assert.IsTrue(trie.Add("common"));
            Assert.IsTrue(trie.Add("commona"));
            Assert.IsTrue(trie.Add("comra"));
            Assert.IsTrue(trie.Add("combra"));
            Assert.AreEqual(4, trie.Size());
        }

        [TestMethod]
        public void SimpleHowManyStartsWithPrefix()
        {
            Assert.IsTrue(trie.Add("common"));
            Assert.IsTrue(trie.Add("commona"));
            Assert.IsTrue(trie.Add("comra"));
            Assert.IsTrue(trie.Add("combra"));
            Assert.AreEqual(4, trie.HowManyStartsWithPrefix("c"));
            Assert.AreEqual(4, trie.HowManyStartsWithPrefix("co"));
            Assert.AreEqual(4, trie.HowManyStartsWithPrefix("com"));
            Assert.AreEqual(0, trie.HowManyStartsWithPrefix("d"));
            Assert.AreEqual(2, trie.HowManyStartsWithPrefix("comm"));
            Assert.AreEqual(1, trie.HowManyStartsWithPrefix("comb"));
            Assert.AreEqual(0, trie.HowManyStartsWithPrefix("combi"));
        }

        [TestMethod]
        public void DifficultTest()
        {
            Assert.IsTrue(trie.Add("common"));
            Assert.IsTrue(trie.Add("data"));
            Assert.IsTrue(trie.Add("dat"));
            Assert.IsTrue(trie.Remove("dat"));
            Assert.AreEqual(2, trie.Size());
            Assert.AreEqual(1, trie.HowManyStartsWithPrefix("d"));
            Assert.IsTrue(trie.Contains("common"));
            Assert.IsTrue(trie.Contains("data"));
            Assert.IsFalse(trie.Contains("dat"));
            Assert.IsTrue(trie.Add("dat"));
            Assert.IsTrue(trie.Contains("dat"));
            Assert.IsFalse(trie.Add("dat"));
            Assert.AreEqual(2, trie.HowManyStartsWithPrefix("d"));
        }
    }
}

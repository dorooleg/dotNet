namespace MyNUnitFrameworkTest
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyNUnit;

    [TestClass]
    public class TestEngineTest
    {
        [TestMethod]
        public void TestMethods()
        {
            var methods = (IEnumerable<MethodInfo>)typeof(TestEngine).GetMethod("GetTestMethods", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(TestClass) });
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "Test1");
            Assert.AreEqual(list[1].Name, "Test2");
        }

        [TestMethod]
        public void BeforeMethods()
        {
            var methods = (IEnumerable<MethodInfo>)typeof(TestEngine).GetMethod("GetBeforeMethods", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(TestClass) });
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "Before1");
            Assert.AreEqual(list[1].Name, "Before2");
        }

        [TestMethod]
        public void AfterMethods()
        {
            var methods = (IEnumerable<MethodInfo>)typeof(TestEngine).GetMethod("GetAfterMethods", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(TestClass) });
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "After1");
            Assert.AreEqual(list[1].Name, "After2");
        }

        [TestMethod]
        public void AfterClassMethods()
        {
            var methods = (IEnumerable<MethodInfo>)typeof(TestEngine).GetMethod("GetAfterClassMethods", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(TestClass) });
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "AfterClass1");
            Assert.AreEqual(list[1].Name, "AfterClass2");
        }

        [TestMethod]
        public void BeforeClassMethods()
        {
            var methods = (IEnumerable<MethodInfo>)typeof(TestEngine).GetMethod("GetBeforeClassMethods", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(TestClass) });
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "BeforeClass1");
            Assert.AreEqual(list[1].Name, "BeforeClass2");
        }

        [TestMethod]
        public void IsTestClass()
        {
            Assert.IsTrue((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(TestClass) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(NoTestClass1) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(NoTestClass2) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(NoTestClass3) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(NoTestClass4) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(NoTestClass5) }));
        }
    }
}

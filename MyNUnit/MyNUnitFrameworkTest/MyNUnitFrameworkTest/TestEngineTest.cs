using System.Text.RegularExpressions;

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
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(EmptyClass) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(BeforeArgs) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(TestWithArgs) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(AfterWithArgs) }));
            Assert.IsFalse((bool)typeof(TestEngine).GetMethod("IsTestClass", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { typeof(AfterClassArgs) }));
        }

        [TestMethod]
        public void ReportTest()
        {
            var te = new TestEngine(@"..\..\..\..\MyNUnit\MyNUnitTest\bin\Debug\MyNUnitTest.dll");
            var report = te.Report();

            const string pattern = @"Test:\sUnitTest1" +
                                   @"\s*\[.*\]\sTestMethod1:\sFailed" +
                                   @"\s*TestMethod2\s\(just\sskip\):\sIgnore" +
                                   @"\s*\[.*\]\sTestMethod3:\sSuccess" +
                                   @"\s*Test:\sBeforeClassException" +
                                   @"\s*BofreClass\sexception\.\sStop\stests" +
                                   @"\s*Test: BeforeException" +
                                   @"\s*BeforeException!" +
                                   @"\s*Test:\sAfterException" +
                                   @"\s*\[.*\]\sTest:\sSuccess" +
                                   @"\s*After\sexception!" +
                                   @"\s*Test:\sAfterClassException" +
                                   @"\s*\[.*\]\sTest:\sSuccess" +
                                   @"\s*AfterClass\sexception.\sStop\stests" +
                                   @"\s*Test:\sTestException" +
                                   @"\s*\[.*\]\sTest:\sFailed";

            Assert.IsTrue(Regex.Match(report, pattern).Success);

        }
    }
}

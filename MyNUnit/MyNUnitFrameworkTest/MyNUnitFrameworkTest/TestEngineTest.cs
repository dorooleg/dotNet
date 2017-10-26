using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNUnit;

namespace MyNUnitFrameworkTest
{
    [TestClass]
    public class TestEngineTest
    {
        private static T CallMethod<T>(string methodName, params object[] args)
            => (T)typeof(TestEngine).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);

        [TestMethod]
        public void TestMethods()
        {
            var methods = CallMethod<IEnumerable<MethodInfo>>("GetTestMethods", typeof(TestClass));
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "Test1");
            Assert.AreEqual(list[1].Name, "Test2");
        }

        [TestMethod]
        public void BeforeMethods()
        {
            var methods = CallMethod<IEnumerable<MethodInfo>>("GetBeforeMethods", typeof(TestClass));
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "Before1");
            Assert.AreEqual(list[1].Name, "Before2");
        }

        [TestMethod]
        public void AfterMethods()
        {
            var methods = CallMethod<IEnumerable<MethodInfo>>("GetAfterMethods", typeof(TestClass));
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "After1");
            Assert.AreEqual(list[1].Name, "After2");
        }

        [TestMethod]
        public void AfterClassMethods()
        {
            var methods = CallMethod<IEnumerable<MethodInfo>>("GetAfterClassMethods", typeof(TestClass));
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "AfterClass1");
            Assert.AreEqual(list[1].Name, "AfterClass2");
        }

        [TestMethod]
        public void BeforeClassMethods()
        {
            var methods = CallMethod<IEnumerable<MethodInfo>>("GetBeforeClassMethods", typeof(TestClass));
            var list = methods.ToList();
            Assert.AreEqual(list[0].Name, "BeforeClass1");
            Assert.AreEqual(list[1].Name, "BeforeClass2");
        }

        [TestMethod]
        public void IsTestClass()
        {
            var correctClass = CallMethod<bool>("IsTestClass", typeof(TestClass));
            Assert.IsTrue(correctClass);
            var emptyClass = CallMethod<bool>("IsTestClass", typeof(EmptyClass));
            Assert.IsFalse(emptyClass);
            var onlyBeforeArgsClass = CallMethod<bool>("IsTestClass", typeof(BeforeArgs));
            Assert.IsFalse(onlyBeforeArgsClass);
            var testWithArgsClass = CallMethod<bool>("IsTestClass", typeof(TestWithArgs));
            Assert.IsFalse(testWithArgsClass);
            var afterWithArgsClass = CallMethod<bool>("IsTestClass", typeof(AfterWithArgs));
            Assert.IsFalse(afterWithArgsClass);
            var afterClassWithArgsClass = CallMethod<bool>("IsTestClass", typeof(AfterClassArgs));
            Assert.IsFalse(afterClassWithArgsClass);
        }

        [TestMethod]
        public void ReportTest()
        {
            var te = new TestEngine(@"..\..\..\..\MyNUnit\MyNUnitTest\bin\Debug\MyNUnitTest.dll");
            var report = te.Report();
            var lines = report.Split(new[]
                {
                    Environment.NewLine
                },
                StringSplitOptions.None);

            // prasing report
            var entries = new List<List<string>>();
            foreach (var line in lines)
            {
                if (line.StartsWith("Test:"))
                {
                    entries.Add(new List<string>());
                }
                entries[entries.Count - 1].Add(line);
            }

            foreach (var entire in entries)
            {
                Assert.IsTrue(entire.Count > 1);
                switch (entire[0])
                {
                    case "Test: ConstructorException":
                        Assert.AreEqual(2, entire.Count);
                        Assert.AreEqual(" CreateInstance exception. Stop tests ", entire[1]);
                        break;
                    case "Test: UnitTest1":
                        Assert.IsTrue(entire.Any(x => x.Contains(" TestMethod1: Failed")));
                        Assert.IsTrue(entire.Any(x => x.Contains("TestMethod2 (just skip): Ignore")));
                        Assert.IsTrue(entire.Any(x => x.Contains(" TestMethod3: Success")));
                        break;
                    case "Test: BeforeException":
                        Assert.AreEqual(2, entire.Count);
                        Assert.IsTrue(entire[1].Contains("BeforeException!"));
                        break;
                    case "Test: BeforeClassException":
                        Assert.AreEqual(2, entire.Count);
                        Assert.IsTrue(entire[1].Contains(" BeforeClass exception. Stop tests"));
                        break;
                    case "Test: AfterException":
                        Assert.AreEqual(3, entire.Count);
                        Assert.IsTrue(entire.Any(x => x.Contains("After exception!")));
                        Assert.IsTrue(entire.Any(x => x.Contains("Test: Success")));
                        break;
                    case "Test: AfterClassException":
                        Assert.AreEqual(3, entire.Count);
                        Assert.IsTrue(entire.Any(x => x.Contains("Test: Success")));
                        Assert.IsTrue(entire.Any(x => x.Contains("AfterClass exception. Stop tests")));
                        break;
                    case "Test: TestException":
                        Assert.AreEqual(3, entire.Count);
                        Assert.IsTrue(entire[1].Contains("Test: Failed"));
                        break;
                    default:
                        Assert.Fail("Entrie are invalid");
                        break;
                }
            }
        }
    }
}

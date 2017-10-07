using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MyNUnit
{
    public class TestEngine
    {
        private readonly Assembly _assembly;

        public TestEngine(string path)
        {
            _assembly = Assembly.LoadFrom(path);
        }

        public string Report()
        {
            var report = string.Empty;
            foreach (var clazz in GetTestClasses())
            {
                report += "Test: " + clazz.Name + "\n";

                object instance;

                try
                {
                    instance = Activator.CreateInstance(clazz);
                }
                catch
                {
                    report += " CreateInstance exception. Stop tests \n";
                    continue;
                }

                try
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    GetBeforeClassMethods(clazz).Select(m => m.Invoke(instance, null)).ToList();
                }
                catch
                {
                    report += " BofreClass exception. Stop tests \n";
                    continue;
                }

                foreach (var test in GetTestMethods(clazz))
                {
                    var attribute = test.GetCustomAttribute(typeof(Test)) as Test;
                    if (attribute?.Ignore != null)
                    {
                        report += "  " + test.Name + " (" + attribute.Ignore + "): Ignore\n";
                        continue;
                    }

                    try
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        GetBeforeMethods(clazz).Select(m => m.Invoke(instance, null)).ToList();
                    }
                    catch
                    {
                        report += "   BeforeException!\n";
                        continue;
                    }

                    bool failed;
                    var sw = new Stopwatch();
                    try
                    {
                        sw.Start();
                        test.Invoke(instance, null);

                        failed = attribute?.Expected != null;
                    }
                    catch (TargetInvocationException e)
                    {
                        failed = e.InnerException?.GetType() != attribute?.Expected;
                    }
                    finally
                    {
                        sw.Stop();
                    }

                    report += "  [" + sw.Elapsed + "] " + test.Name + ": " + (failed ? "Failed" : "Success") + "\n";

                    try
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        GetAfterMethods(clazz).Select(m => m.Invoke(instance, null)).ToList();
                    }
                    catch
                    {
                        report += "    After exception!\n";
                    }
                }

                try
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    GetAfterClassMethods(clazz).Select(m => m.Invoke(instance, null)).ToList();
                }
                catch
                {
                    report += " AfterClass exception. Stop tests\n";
                }
            }

            return report;
        }

        private static bool IsTestClass(Type clazz)
            => clazz
                .GetMethods().Any(m => !m.GetParameters().Any()
                            && m.GetCustomAttributesData().Count == 1
                            && m.GetCustomAttributesData()
                                .Any(a => new[] { typeof(Test), typeof(Before),
                                                         typeof(After), typeof(BeforeClass),
                                                         typeof(AfterClass)
                                                       }.Any(x => x == a.AttributeType)));

        private static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type clazz, Type attribute)
            => clazz.GetMethods()
                .Where(m => !m.GetParameters().Any()
                            && m.GetCustomAttributesData().Count == 1
                            && m.GetCustomAttributesData()
                                .Any(a => a.AttributeType == attribute));

        private static IEnumerable<MethodInfo> GetTestMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(Test));

        private static IEnumerable<MethodInfo> GetBeforeMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(Before));

        private static IEnumerable<MethodInfo> GetAfterMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(After));

        private static IEnumerable<MethodInfo> GetBeforeClassMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(BeforeClass));

        private static IEnumerable<MethodInfo> GetAfterClassMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(AfterClass));

        private IEnumerable<Type> GetTestClasses()
            => _assembly
                .ExportedTypes
                .Where(t => t.IsClass && IsTestClass(t) && t.GetConstructor(Type.EmptyTypes) != null);
    }
}

namespace MyNUnit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    public class TestEngine
    {
        private Assembly assembly;

        public TestEngine(string path)
        {
            assembly = Assembly.LoadFrom(path);
        }

        public string Report()
        {
            string report = string.Empty;
            foreach (var clazz in GetTestClasses())
            {
                report += "Test: " + clazz.Name + "\n";

                object instance = null;

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
                    if (attribute.Ignore != null)
                    {
                        report += "  " + test.Name + " (" + attribute.Ignore + "): Ignore\n";
                        continue;
                    }

                    try
                    {
                        GetBeforeMethods(clazz).Select(m => m.Invoke(instance, null)).ToList();
                    }
                    catch
                    {
                        report += "   BeforeException!\n";
                        continue;
                    }

                    bool failed = false;
                    Stopwatch sw = new Stopwatch();
                    try
                    {
                        sw.Start();
                        test.Invoke(instance, null);

                        failed = attribute.Expected != null;
                    }
                    catch (TargetInvocationException e)
                    {
                        failed = e.InnerException.GetType() != attribute.Expected;
                    }
                    finally
                    {
                        sw.Stop();
                    }

                    report += "  [" + sw.Elapsed + "] " + test.Name + ": " + (failed ? "Failed" : "Success") + "\n";

                    try
                    {
                        GetAfterMethods(clazz).Select(m => m.Invoke(instance, null)).ToList();
                    }
                    catch
                    {
                        report += "    After exception!\n";
                    }
                }

                try
                {
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
            => clazz.GetMethods()
                .Where(m => m.GetParameters().Count() == 0
                            && m.GetCustomAttributesData().Count() == 1
                            && m.GetCustomAttributesData()
                                .Where(a => new Type[] { typeof(Test), typeof(Before),
                                                         typeof(After), typeof(BeforeClass),
                                                         typeof(AfterClass)
                                                       }.Any(x => x == a.AttributeType))
                             .Any()).Any();

        private static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type clazz, Type attribute)
            => clazz.GetMethods()
                .Where(m => m.GetParameters().Count() == 0
                            && m.GetCustomAttributesData().Count() == 1
                            && m.GetCustomAttributesData()
                                .Where(a => a.AttributeType == attribute)
                                .Any());

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
            => assembly
                .ExportedTypes
                .Where(t => t.IsClass && IsTestClass(t) && t.GetConstructor(Type.EmptyTypes) != null);
    }
}

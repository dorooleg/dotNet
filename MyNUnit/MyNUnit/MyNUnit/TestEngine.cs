using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

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
            var report = new StringBuilder();
            foreach (var clazz in GetTestClasses())
            {
                report.AppendLine($"Test: {clazz.Name}");

                object instance;

                try
                {
                    instance = Activator.CreateInstance(clazz);
                }
                catch
                {
                    report.AppendLine(" CreateInstance exception. Stop tests ");
                    continue;
                }

                try
                {
                    GetBeforeClassMethods(clazz).ForEach(m => m.Invoke(instance, null));
                }
                catch
                {
                    report.AppendLine(" BeforeClass exception. Stop tests ");
                    continue;
                }

                foreach (var test in GetTestMethods(clazz))
                {
                    var attribute = test.GetCustomAttribute(typeof(Test)) as Test;
                    if (attribute?.Ignore != null)
                    {
                        report.AppendLine($"  {test.Name} ({attribute.Ignore}): Ignore");
                        continue;
                    }

                    try
                    {
                        GetBeforeMethods(clazz).ForEach(m => m.Invoke(instance, null));
                    }
                    catch
                    {
                        report.AppendLine("   BeforeException!");
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

                    report.AppendLine($"  [{sw.Elapsed}] {test.Name}: {(failed ? "Failed" : "Success")}");

                    try
                    {
                        GetAfterMethods(clazz).ForEach(m => m.Invoke(instance, null));
                    }
                    catch
                    {
                        report.AppendLine("    After exception!");
                    }
                }

                try
                {
                    GetAfterClassMethods(clazz).ForEach(m => m.Invoke(instance, null));
                }
                catch
                {
                    report.AppendLine(" AfterClass exception. Stop tests");
                }
            }

            return report.ToString();
        }

        private static readonly Type[] TypeAttributes = {
            typeof(Test), typeof(Before),
            typeof(After), typeof(BeforeClass),
            typeof(AfterClass)
        };

        private static bool IsTestClass(Type clazz)
            => clazz
                .GetMethods().Any(m => !m.GetParameters().Any()
                            && m.GetCustomAttributesData().Count == 1
                            && m.GetCustomAttributesData()
                                .Any(a => TypeAttributes.Contains(a.AttributeType)));

        private static List<MethodInfo> GetMethodsWithAttribute(Type clazz, Type attribute)
            => clazz
                .GetMethods()
                .Where(m => !m.GetParameters().Any()
                            && m.GetCustomAttributesData().Count == 1
                            && m.GetCustomAttributesData()
                                .Any(a => a.AttributeType == attribute)).ToList();

        private static List<MethodInfo> GetTestMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(Test));

        private static List<MethodInfo> GetBeforeMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(Before));

        private static List<MethodInfo> GetAfterMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(After));

        private static List<MethodInfo> GetBeforeClassMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(BeforeClass));

        private static List<MethodInfo> GetAfterClassMethods(Type clazz)
            => GetMethodsWithAttribute(clazz, typeof(AfterClass));

        private IEnumerable<Type> GetTestClasses()
            => _assembly
                .ExportedTypes
                .Where(t => t.IsClass && IsTestClass(t) && t.GetConstructor(Type.EmptyTypes) != null);
    }
}

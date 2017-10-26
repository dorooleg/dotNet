using System;

namespace MyNUnit
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class Test : Attribute
    {
        public virtual Type Expected { get; set; }

        public virtual string Ignore { get; set; }
    }
}

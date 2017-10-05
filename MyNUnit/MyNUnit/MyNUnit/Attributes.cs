namespace MyNUnit
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class Test : Attribute
    {
        private string ignore;
        private Type expected;

        public virtual Type Expected
        {
            get { return expected; }
            set { expected = value; }
        }

        public virtual string Ignore
        {
            get { return ignore; }
            set { ignore = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class Before : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class After : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BeforeClass : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AfterClass : Attribute
    {
    }
}

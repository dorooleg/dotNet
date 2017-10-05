namespace MyNUnit
{
    public class Asserts
    {
        public static void IsTrue(bool expression)
        {
            if (!expression)
            {
                throw new AssertFailedException();
            }
        }

        public static void IsFalse(bool expression)
            => IsTrue(!expression);
    }
}

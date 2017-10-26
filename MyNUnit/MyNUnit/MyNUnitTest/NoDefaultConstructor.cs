using MyNUnit;

namespace MyNUnitTest
{
    public class NoDefaultConstructor
    {
        public int A;

        public NoDefaultConstructor(int a)
        {
            A = a;
        }

        [Test]
        public void Test()
        {
        }
    }
}

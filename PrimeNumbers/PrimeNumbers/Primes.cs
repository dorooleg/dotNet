namespace PrimeNumbers
{
    public static class Primes
    {
        public static bool IsPrime(this int thіs)
        {
            for (var i = 2; i * i <= thіs; i++)
            {
                if (thіs % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
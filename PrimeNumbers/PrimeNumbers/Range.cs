namespace PrimeNumbers
{
    public class Range
    {
        public readonly int Count;
        public readonly int From;

        public Range(int from, int count)
        {
            From = from;
            Count = count;
        }

        public int ToExclusive => From + Count;
    }
}
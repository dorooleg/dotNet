namespace PrimeNumbers
{
    public class Range
    {
        public readonly int From;
        public readonly int Count;

        public int ToExclusive => From + Count;

        public Range(int @from, int count)
        {
            From = @from;
            Count = count;
        }

    }
}
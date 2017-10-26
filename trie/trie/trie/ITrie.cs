namespace TrieDataSctructure
{
    public interface ITrie
    {
        /// Expected complexity: O(|element|)
        /// Returns true if this set did not already contain the specified element
        bool Add(string element);

        /// Expected complexity: O(|element|)
        bool Contains(string element);

        /// Expected complexity: O(|element|)
        /// Returns true if this set contained the specified element
        int HowManyStartsWithPrefix(string prefix);

        /// Expected complexity: O(|prefix|)
        bool Remove(string element);

        /// Expected complexity: O(1)
        int Size();
    }
}
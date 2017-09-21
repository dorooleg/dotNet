namespace TrieDataSctructure
{
    using System.Collections.Generic;

    public class Trie
    {
        private Node root;

        public Trie()
        {
            root = new Node();
        }

        /// Expected complexity: O(|element|)
        /// Returns true if this set did not already contain the specified element
        public bool Add(string element)
        {
            if (Contains(element))
            {
                return false;
            }

            Node currentNode = root;
            Node previousNode = root;

            foreach (var c in element)
            {
                if (currentNode == null)
                {
                    break;
                }

                if (!currentNode.Childs.ContainsKey(c))
                {
                    currentNode.Childs.Add(c, new Node());
                }

                currentNode.CountPrefixes++;
                previousNode = currentNode;
                currentNode = currentNode.GetChild(c);
            }

            previousNode.Terminal = true;

            return true;
        }

        /// Expected complexity: O(|element|)
        public bool Contains(string element)
            => GetLastEdge(element).previousNode.Terminal;

        /// Expected complexity: O(|element|)
        /// Returns true if this set contained the specified element
        public bool Remove(string element)
        {
            if (!Contains(element))
            {
                return false;
            }

            Node currentNode = root;
            Node previousNode = root;

            previousNode.CountPrefixes--;

            foreach (var c in element)
            {
                if (previousNode.CountPrefixes == 0)
                {
                    previousNode.Childs.Clear();
                }

                previousNode = currentNode;
                currentNode = currentNode.GetChild(c);

                if (currentNode == null)
                {
                    break;
                }

                currentNode.CountPrefixes--;
            }

            previousNode.Terminal = false;
            
            return true;
        }

        /// Expected complexity: O(|prefix|)
        public int HowManyStartsWithPrefix(string prefix)
            => (GetLastEdge(prefix).currentNode?.CountPrefixes).GetValueOrDefault(0);

        /// Expected complexity: O(1)
        public int Size() => HowManyStartsWithPrefix(string.Empty);

        private (Node previousNode, Node currentNode) GetLastEdge(string element)
        {
            Node currentNode = root;
            Node previousNode = root;

            foreach (var c in element)
            {
                if (currentNode == null)
                {
                    break;
                }

                previousNode = currentNode;
                currentNode = currentNode.GetChild(c);
            }

            return (previousNode, currentNode);
        }

        private class Node
        {
            private IDictionary<char, Node> childs = new Dictionary<char, Node>();

            public IDictionary<char, Node> Childs
            {
                get { return childs; }
                set { childs = value;  }
            }

            public bool Terminal { get; set; }

            public int CountPrefixes { get; set; }

            public Node GetChild(char s) => Childs.TryGetValue(s, out Node node) ? node : null;
        }
    }
}
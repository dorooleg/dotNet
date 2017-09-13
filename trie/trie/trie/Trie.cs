namespace TrieDataSctructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

            var(previousNode, currentNode) = GetLastEdge(
                                                element,
                                                (p, c, s) =>
                                                {
                                                    c.CountPrefixes++;
                                                    if (!c.Childs.ContainsKey(s))
                                                        c.Childs.Add(s, new Node());
                                                    return false;
                                                });

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

            Remove(root, 0, element);

            return true;
        }

        /// Expected complexity: O(|prefix|)
        public int HowManyStartsWithPrefix(string prefix)
            => (GetLastEdge(prefix).currentNode?.CountPrefixes).GetValueOrDefault(0);

        /// Expected complexity: O(1)
        public int Size() => HowManyStartsWithPrefix(string.Empty);

        private void Remove(Node node, int position, string element)
        {
            node.CountPrefixes--;
            if (position == element.Length - 1)
            {
                node.Terminal = false;
                return;
            }

            Remove(node.Childs[element[position]], position + 1, element);

            if (node.GetChildCount(element[position]) == 0 && !node.IsTerminalChild(element[position]))
            {
                node.Childs.Remove(element[position]);
            }
        }

        private (Node previousNode, Node currentNode) GetLastEdge(string element)
            => GetLastEdge(element, (p, c, s) => c == null);

        private (Node previousNode, Node currentNode) 
            GetLastEdge(string element, Func<Node, Node, char, bool> predicate)
        {
            Node currentNode = root;
            Node previousNode = root;

            foreach (var c in element)
            {
                if (predicate(previousNode, currentNode, c))
                {
                    break;
                }

                previousNode = currentNode;
                currentNode = currentNode.Childs.ContainsKey(c) ? currentNode.Childs[c] : null;
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

            public Node GetChild(char s)
            {
                Node node;
                Childs.TryGetValue(s, out node);
                return node;
            }

            public int GetChildCount(char s)
            {
                Node node = GetChild(s);
                return node == null ? 0 : node.Childs.Count;
            }

            public bool IsTerminalChild(char s)
            {
                Node node = GetChild(s);
                return node == null ? false : node.Terminal;
            }
        }
    }
}
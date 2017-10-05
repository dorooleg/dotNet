namespace TrieDataSctructure
{
    using System.Collections.Generic;

    public class Trie : ITrie
    {
        private readonly Node _root;

        public Trie()
        {
            _root = new Node();
        }

        public bool Add(string element)
        {
            if (Contains(element))
            {
                return false;
            }

            Node nextNode = _root;
            Node currentNode = _root;

            foreach (char c in element)
            {
                if (!nextNode.Childs.ContainsKey(c))
                {
                    nextNode.Childs.Add(c, new Node());
                }

                ++nextNode.CountPrefixes;
                currentNode = nextNode;
                nextNode = nextNode.GetChild(c);
            }

            currentNode.IsTerminal = true;

            return true;
        }

        public bool Contains(string element)
        {
            (Node currentNode, Node nextNode) edge = GetLastEdge(element);
            return edge.nextNode != null && edge.currentNode.IsTerminal;
        }

        public bool Remove(string element)
        {
            if (!Contains(element))
            {
                return false;
            }

            Node nextNode = _root;
            Node currentNode = _root;

            --currentNode.CountPrefixes;

            foreach (char c in element)
            {
                if (currentNode.CountPrefixes == 0)
                {
                    currentNode.Childs.Clear();
                }

                currentNode = nextNode;
                nextNode = nextNode.GetChild(c);

                if (nextNode == null)
                {
                    break;
                }

                nextNode.CountPrefixes--;
            }

            currentNode.IsTerminal = false;
            
            return true;
        }

        public int HowManyStartsWithPrefix(string prefix)
            => (GetLastEdge(prefix).nextNode?.CountPrefixes).GetValueOrDefault(0);

        public int Size() => _root.CountPrefixes;

        private (Node currentNode, Node nextNode) GetLastEdge(string element)
        {
            Node nextNode = _root;
            Node currentNode = _root;

            foreach (char c in element)
            {
                if (nextNode == null)
                {
                    break;
                }

                currentNode = nextNode;
                nextNode = nextNode.GetChild(c);
            }

            return (currentNode, nextNode);
        }

        private class Node
        {
            public IDictionary<char, Node> Childs { get; set; } = new Dictionary<char, Node>();

            public bool IsTerminal { get; set; }

            public int CountPrefixes { get; set; }

            public Node GetChild(char s) => Childs.TryGetValue(s, out Node node) ? node : null;
        }
    }
}
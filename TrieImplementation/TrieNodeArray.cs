using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrieImplementation
{
    class TrieNodeArray<ValueType>
    {
        private static readonly int MAXCHARS = 26;
        public char Character { get; set; }
        public TrieNodeArray<ValueType>[] Children { get; set; }
        public TrieNodeArray<ValueType> Parent;
        public ValueType Value;

        public TrieNodeArray()
        {
            Character = default(char);
            Parent = null;
            Children = new TrieNodeArray<ValueType>[MAXCHARS];
            Value = default(ValueType);
        }

        // return true if all list elements are null
        public bool IsEmpty()
        {
            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i] != null)
                    return false;
            }
            return true;
        }

        // returns child node given a character, or null if not found
        public TrieNodeArray<ValueType> FindChildNode(char c)
        {
            foreach (var child in Children)
            {
                if (child != null && child.Character == c)
                    return child;
            }
            return null;
        }

        // removes node holding value
        public void Remove()
        {
            Character = default(char);
            Value = default(ValueType);
            Parent = null;
        }

        // Adds the node to first available child node
        // adds optional value if its the last character
        public void AddToChildren(TrieNodeArray<ValueType> node, ValueType value = default(ValueType))
        {
            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i] == null)
                {
                    Children[i] = node;
                    Children[i].Value = value;
                    break;
                }
            }
        }

        // returns collection of all child terminal nodes from the current node
        public IEnumerable<TrieNodeArray<ValueType>> TerminalNodes()
        {
            var result = new List<TrieNodeArray<ValueType>>();
            // nodes are terminal if the value is a non-null ValueType
            if (!EqualityComparer<ValueType>.Default.Equals(Value, default(ValueType)))
                result.Add(this);

            // add enumerable collections to result list
            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i] != null)
                    result = result.Concat(Children[i].TerminalNodes()).ToList();
            }
            return result;
        }

        // builds the 'key' string up from terminal node to root 
        public String BuildKey()
        {
            var b = new StringBuilder();
            b.Insert(0, Character.ToString());
            var selectedNode = Parent;
            while (selectedNode != null)
            {
                b.Insert(0, selectedNode.Character.ToString());
                selectedNode = selectedNode.Parent;
            }
            return b.ToString();
        }

        // string output for testing
        public override String ToString()
        {
            return string.Format("[{0}, {1} ]", BuildKey(), Value);
        }

    }
}

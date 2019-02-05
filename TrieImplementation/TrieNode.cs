using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrieImplementation
{
    public class TrieNode<ValueType>
    {
        public char Character { get; set; }
        public SortedDictionary<char, TrieNode<ValueType>> Children { get; set; }
        public bool Terminal { get; set; }
        public TrieNode<ValueType> Parent { get; set; }
        public ValueType Value { get; private set; }

        public TrieNode()
        {
            Children = new SortedDictionary<char, TrieNode<ValueType>>();
            Terminal = false;
            Character = default(char);
            Parent = null;
            Value = default(ValueType);
        }

        // destroy self
        public void Destroy()
        {
            var currentNode = Parent;
            currentNode.Children.Remove(Character);
        }

        // returns list of all terminal nodes under this TrieNode
        public IEnumerable<TrieNode<ValueType>> TerminalNodes()
        {
            var result = new List<TrieNode<ValueType>>();
            if (Terminal)
                result.Add(this);

            // adding enumerable collection to list results
            foreach (var child in Children.Values)
                result = result.Concat(child.TerminalNodes()).ToList();

            return result;
        }

        // sets node as terminal and records value
        public void SetTerminal(ValueType value)
        {
            Terminal = true;
            Value = value;
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

        // for testing and output purposes
        public override String ToString()
        {
            return string.Format("[{0}, {1} ]", BuildKey(), Value);
        }

    }
}

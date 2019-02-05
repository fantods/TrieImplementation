﻿using System;
using System.Collections.Generic;

namespace TrieImplementation
{
    class TrieArray<KeyType, ValueType> : ITrie<KeyType, ValueType>
    {
        private static readonly int MAXCHARS = 26;
        private TrieNodeArray<ValueType> _root;
        private TrieNodeArray<ValueType> root
        {
            get
            {
                if (_root == null) _root = new TrieNodeArray<ValueType>();
                return _root;
            }
        }

        // Add key/value pair to Trie
        // Update value if key already exists
        public void AddWord(String key, ValueType value)
        {
            // key must be something!
            if (key == null)
                throw new ArgumentException("key cannot be null");

            // value must be something!
            if (value == null)
                throw new ArgumentException("value cannot be null");

            TrieNodeArray<ValueType> currentNode = root;
            TrieNodeArray<ValueType> tmpNode = null;

            // iterate through each key character
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];

                // iterate through each child of currentNode
                for (int j = 0; j < currentNode.Children.Length; j++)
                {
                    // node exists, follow the path
                    TrieNodeArray<ValueType> nodeExists = currentNode.FindChildNode(ch);
                    if (nodeExists != null)
                    {
                        // follow the if it exists, has a value 
                        // and we are not on the last character
                        if (nodeExists.Value != null && (i != (key.Length - 1)))
                        {
                            currentNode = nodeExists;
                            break;
                        }
                        // if the node exists and we are on the last character
                        // update the value
                        else if (i == (key.Length - 1))
                        {
                            nodeExists.Value = value;
                            break;
                        }
                        else
                        {
                            // otherwise, continue down the path
                            currentNode = nodeExists;
                            break;
                        }
                    }
                    // create the character
                    else
                    {
                        tmpNode = new TrieNodeArray<ValueType>() { Character = ch, Parent = currentNode };
                        // we are at the end of the key, save the value
                        if (i == (key.Length - 1))
                        {
                            currentNode.AddToChildren(tmpNode, value);
                            break;
                        }
                        else
                        {
                            currentNode.AddToChildren(tmpNode);
                            currentNode = currentNode.FindChildNode(ch);
                            break;
                        }
                    }
                }
            }
        }

        // Removes key/value pair from Trie
        // Returns true if successful
        public bool Remove(String key)
        {
            var currentNode = root;
            TrieNodeArray<ValueType> tmpNode = null;

            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];

                if ((tmpNode = currentNode.FindChildNode(ch)) != null)
                {
                    // we are on the last character of the key
                    // and the is a child node that is terminal
                    if ((i == (key.Length - 1)) && (tmpNode.Value != null))
                    {
                        tmpNode.Remove();
                        return true;
                    }
                }
                currentNode = tmpNode;
            }
            return false;
        }

        // Returns value v that corresponds to full key k
        public ValueType Translate(String key)
        {
            var currentNode = root;
            TrieNodeArray<ValueType> tmpNode = null;

            // iterate characters of key
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];

                if ((tmpNode = currentNode.FindChildNode(ch)) != null)
                {
                    // if we are on last character and child node exists thats terminal
                    if ((i == (key.Length - 1)) && (tmpNode.Value != null))
                    {
                        // return the value of this child node
                        return tmpNode.Value;
                    }
                    currentNode = tmpNode;
                }
            }
            return default(ValueType);
        }

        // Returns all values whose key begin with k
        public List<ValueType> AutoComplete(String key)
        {
            var result = new List<ValueType>();
            TrieNodeArray<ValueType> tmpNode = null;
            var currentNode = root;

            // iterate characters of key
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];
                // if we find a child node with key matching character
                if ((tmpNode = currentNode.FindChildNode(ch)) != null)
                {
                    // set our current node as child node
                    currentNode = tmpNode;

                    // if we are on the last character
                    // return all terminal nodes from this node
                    if (i == (key.Length - 1))
                    {
                        foreach (var node in currentNode.TerminalNodes())
                            result.Add(node.Value);
                    }
                }
            }
            return result;
        }

        public void Print()
        {
            var terminals = root.TerminalNodes();
            foreach (var node in terminals)
                Console.WriteLine(node.ToString());
        }
    }

    public class TrieArrayTest
    {
        public void Main()
        {
            Console.WriteLine("\n\nTesting Trie with Array Implementation...");
            TrieArray<String, String> trie = new TrieArray<String, String>();
            trie.AddWord("ball", "ball");
            trie.AddWord("bat", "bat");
            trie.AddWord("t", "bat");
            // this is testing that keys are updated after being set
            trie.AddWord("bat", "RENAMED");
            trie.AddWord("oo", "book");
            trie.AddWord("do", "do");
            trie.AddWord("ll", "doll");
            trie.AddWord("dork", "dork");
            trie.AddWord("m", "dorm");
            trie.AddWord("see", "see");
            trie.AddWord("nd", "send");
            trie.AddWord("sens", "sense");
            trie.AddWord("x", "extreme");

            Console.WriteLine("Printing all words: ");
            trie.Print();

            // this should print out all words in the trie that start with b
            // expecting all three words that start with b to be output
            Console.WriteLine("\nTesting autocomplete with: 'b'..");
            var autocomplete1 = trie.AutoComplete("b");
            foreach (var elem in autocomplete1)
                Console.WriteLine(elem);

            // this should print out all words in the trie that start with d
            // expecting all four words that start with d to be output
            Console.WriteLine("\nTesting autocomplete with: 'd'..");
            var autocomplete2 = trie.AutoComplete("d");
            foreach (var elem in autocomplete2)
                Console.WriteLine(elem);

            // this should only print out the words 'ball' and 'bat'
            // testing for autocomplete output when there is two nodes
            Console.WriteLine("\nTesting autocomplete with: 'ba'..");
            var autocomplete3 = trie.AutoComplete("ba");
            foreach (var elem in autocomplete3)
                Console.WriteLine(elem);

            // this should only print out 'ball'
            // testing autocomplete on a path that includes 
            Console.WriteLine("\nTesting autocomplete with: 'bal'..");
            var autocomplete4 = trie.AutoComplete("bal");
            foreach (var elem in autocomplete4)
                Console.WriteLine(elem);

            // this should print out nothing
            // testing autocomplete down a path that doesn't exist
            Console.WriteLine("\nTesting autocomplete with: 'bel'..");
            var autocomplete5 = trie.AutoComplete("bel");
            foreach (var elem in autocomplete5)
                Console.WriteLine(elem);

            // this should print out dork and dorm
            // testing autocomplete with two possible words
            Console.WriteLine("\nTesting autocomplete with: 'dor'..");
            var autocomplete6 = trie.AutoComplete("dor");
            foreach (var elem in autocomplete6)
                Console.WriteLine(elem);

            // this should print out send and sense
            // testing autocomplete with two possible words
            Console.WriteLine("\nTesting autocomplete with: 'sen'..");
            var autocomplete7 = trie.AutoComplete("sen");
            foreach (var elem in autocomplete7)
                Console.WriteLine(elem);

            // this should print out 'extreme'
            // testing autocomplete with two possible words
            Console.WriteLine("\nTesting autocomplete with: 'x'..");
            var autocomplete8 = trie.AutoComplete("x");
            foreach (var elem in autocomplete8)
                Console.WriteLine(elem);

            // this should return "ball"
            // testing translates output at a terminal node with no children
            Console.WriteLine("\nTesting translate with: 'ball'..");
            var translate1 = trie.Translate("ball");
            Console.WriteLine(translate1);

            // this should return nothing
            // this is testing for translates output just before a terminal
            Console.WriteLine("\nTesting translate with: 'bal'..");
            var translate2 = trie.Translate("bal");
            Console.WriteLine(translate2);

            // this should return 'do'
            // this is testing for a terminal with nodes on either side
            Console.WriteLine("\nTesting translate with: 'do'..");
            var translate3 = trie.Translate("do");
            Console.WriteLine(translate3);

            // this should return nothing
            // this is testing for translates output just after a terminal
            Console.WriteLine("\nTesting translate with: 'dor'..");
            var translate4 = trie.Translate("dor");
            Console.WriteLine(translate4);

            // this should return 'extreme'
            // this is testing for translates output just after a terminal
            Console.WriteLine("\nTesting translate with: 'x'..");
            var translate5 = trie.Translate("x");
            Console.WriteLine(translate5);

            // should only remove the word 'ball' from the trie, leaving the other b words
            Console.WriteLine("\nTesting remove with: 'ball'..");
            var remove1 = trie.Remove("ball");
            Console.WriteLine($"Result: {remove1}");
            Console.WriteLine("Printing all words:");
            trie.Print();

            // this attempts remove 'dor' but will return false since it cannot be found
            Console.WriteLine("\nTesting remove with: 'dor'..");
            var remove2 = trie.Remove("dor");
            Console.WriteLine($"Result: {remove2}");
            Console.WriteLine("Printing all words:");
            trie.Print();

            // this should remove all words that start with 'd' from the trie
            // removes 'do' and 'doll'
            Console.WriteLine("\nTesting remove with: 'do'..");
            var remove3 = trie.Remove("do");
            Console.WriteLine($"Result: {remove3}");
            Console.WriteLine("Printing all words:");
            trie.Print();
        }
    }
}

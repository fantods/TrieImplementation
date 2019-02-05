using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrieImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            TrieTest();
            TrieArrayTest();
            Console.ReadKey();
        }

        static void TrieTest()
        {
            TrieTest trie = new TrieTest();
            trie.Main();
        }

        static void TrieArrayTest()
        {
            TrieArrayTest trie = new TrieArrayTest();
            trie.Main();
        }
    }
}

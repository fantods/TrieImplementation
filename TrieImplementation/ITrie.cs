using System;
using System.Collections.Generic;

namespace TrieImplementation
{
    interface ITrie<KeyType, ValueType>
    {
        void AddWord(String key, ValueType value);
        bool Remove(String key);
        ValueType Translate(String key);
        List<ValueType> AutoComplete(String key);
        void Print();
    }
}

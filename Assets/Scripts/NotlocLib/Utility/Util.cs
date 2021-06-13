using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Dictionary<K, V> Dictionary<K, V>(IList<V> values, Func<V, K> keyMapper)
    {
        Dictionary<K, V> dictionary = new Dictionary<K, V>();
        foreach (V val in values)
        {
            dictionary.Add(keyMapper(val), val);
        }
        return dictionary;
    }

    public static Dictionary<K, V> Dictionary<K, V, T>(IList<T> values, Func<T, K> keyMapper, Func<T, V> valueMapper)
    {
        Dictionary<K, V> dictionary = new Dictionary<K, V>();
        foreach (T val in values)
        {
            dictionary.Add(keyMapper(val), valueMapper(val));
        }
        return dictionary;
    }
}

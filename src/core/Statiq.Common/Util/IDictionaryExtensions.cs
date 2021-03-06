﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Statiq.Common
{
    public static class IDictionaryExtensions
    {
        public static void AddOrReplaceRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            _ = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

            if (items != null)
            {
                foreach (KeyValuePair<TKey, TValue> item in items)
                {
                    dictionary[item.Key] = item.Value;
                }
            }
        }

        public static void AddRangeIfNonExisting<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            _ = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

            if (items != null)
            {
                foreach (KeyValuePair<TKey, TValue> item in items)
                {
                    dictionary.AddIfNonExisting(item.Key, item.Value);
                }
            }
        }

        public static void AddIfNonExisting<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
    }
}

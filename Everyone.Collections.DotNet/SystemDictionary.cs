using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Everyone
{
    public static class SystemDictionary
    {
        public static SystemDictionary<TKey, TValue> Create<TKey, TValue>(params (TKey, TValue)[] values) where TKey : notnull
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return SystemDictionary.Create(values.Map(entry => Tuple.Create(entry.Item1, entry.Item2)));
        }

        public static SystemDictionary<TKey, TValue> Create<TKey, TValue>(IEnumerable<Tuple<TKey, TValue>> values) where TKey : notnull
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return SystemDictionary.Create(values.Map(t => new KeyValuePair<TKey, TValue>(t.Item1, t.Item2)));
        }

        public static SystemDictionary<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values) where TKey : notnull
        {
            return SystemDictionary<TKey,TValue>.Create(values);
        }
    }

    public class SystemDictionary<TKey,TValue> : MutableMapBase<TKey,TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> dictionary;

        protected SystemDictionary(IEnumerable<KeyValuePair<TKey,TValue>> values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            this.dictionary = new Dictionary<TKey, TValue>();

            this.SetAll(values);
        }

        public static SystemDictionary<TKey, TValue> Create(IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            return new SystemDictionary<TKey, TValue>(values);
        }

        public override ICollection<TKey> Keys => this.dictionary.Keys;

        public override ICollection<TValue> Values => this.dictionary.Values;

        public override int Count => this.dictionary.Count;

        public override void Clear()
        {
            this.dictionary.Clear();
        }

        public override bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.dictionary.Contains(item);
        }

        public override bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public override void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)this.dictionary).CopyTo(array, arrayIndex);
        }

        public override TValue Get(TKey key)
        {
            return this.dictionary[key];
        }

        public override Iterator<KeyValuePair<TKey, TValue>> Iterate()
        {
            return this.dictionary.Iterate();
        }

        public override bool Remove(TKey key)
        {
            return this.dictionary.Remove(key);
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey,TValue>)this.dictionary).Remove(item);
        }

        public override void Set(TKey key, TValue value)
        {
            if (!this.dictionary.TryAdd(key, value))
            {
                this.dictionary.Remove(key);
                this.dictionary.Add(key, value);
            }
        }

        public override bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }
    }
}

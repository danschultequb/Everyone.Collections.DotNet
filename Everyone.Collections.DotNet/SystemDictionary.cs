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

    public class SystemDictionary<TKey,TValue> : MutableMapBase<TKey,TValue,SystemDictionary<TKey,TValue>>
        where TKey : notnull
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

        public override Iterable<TKey> Keys => List.Create<TKey>(this.dictionary.Keys);

        public override Iterable<TValue> Values => List.Create<TValue>(this.dictionary.Values);

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

        public override Result<TValue> Get(TKey key)
        {
            return Result.Create(() =>
            {
                if (!this.TryGetValue(key,out TValue? result))
                {
                    throw new NotFoundException($"Could not find the key: {key}");
                }
                return result!;
            });
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

        public override SystemDictionary<TKey,TValue> Set(TKey key, TValue value)
        {
            if (!this.dictionary.TryAdd(key, value))
            {
                this.dictionary.Remove(key);
                this.dictionary.Add(key, value);
            }
            return this;
        }

        public override bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }
    }
}

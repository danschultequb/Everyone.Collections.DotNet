using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Everyone
{
    /// <summary>
    /// A mapping between values of type <typeparamref name="TKey"/> and
    /// <typeparamref name="TValue"/>.
    /// </summary>
    public static class Map
    {
        public static MutableMap<TKey, TValue> Create<TKey, TValue>(params (TKey, TValue)[] values) where TKey : notnull
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return Map.Create(values.Map(entry => Tuple.Create(entry.Item1, entry.Item2)));
        }

        public static MutableMap<TKey, TValue> Create<TKey, TValue>(IEnumerable<Tuple<TKey, TValue>> values) where TKey : notnull
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return Map.Create(values.Map(t => new KeyValuePair<TKey, TValue>(t.Item1, t.Item2)));
        }

        public static MutableMap<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values) where TKey : notnull
        {
            return MutableMap.Create(values);
        }
    }

    /// <summary>
    /// A mapping between values of type <typeparamref name="TKey"/> and
    /// <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in this <see cref="Map{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValue">The type of the values in this <see cref="Map{TKey, TValue}"/>.</typeparam>
    public interface Map<TKey,TValue> : Iterable<KeyValuePair<TKey,TValue>>, IReadOnlyDictionary<TKey,TValue>
    {
        public new int Count { get; }

        /// <summary>
        /// Get the value associated with the provided <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the value to return.</param>
        public Result<TValue> Get(TKey key);

        /// <summary>
        /// Try to get the value associated with the provided <paramref name="key"/>. Return
        /// whether the key is found.
        /// </summary>
        /// <param name="key">The key of the value to return.</param>
        /// <param name="value">The value associated with the provided <paramref name="key"/> if
        /// the <paramref name="key"/> is found. If the <paramref name="key"/> is not found, then
        /// this will be set to null.</param>
        public new bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);

        /// <summary>
        /// Get an <see cref="Iterator{T}"/> that will iterate through the entries of this
        /// <see cref="Map{TKey, TValue}"/> as <see cref="Tuple{T1, T2}"/>s.
        /// </summary>
        public Iterator<Tuple<TKey, TValue>> IterateTuples();
    }

    public abstract class MapBase<TKey, TValue> : Map<TKey, TValue>
    {
        public virtual TValue this[TKey key] => this.Get(key).Await();

        public abstract IEnumerable<TKey> Keys { get; }

        public abstract IEnumerable<TValue> Values { get; }

        public abstract int Count { get; }

        public abstract bool ContainsKey(TKey key);

        public abstract Result<TValue> Get(TKey key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.Iterate();
        }

        public abstract Iterator<KeyValuePair<TKey, TValue>> Iterate();

        public virtual Iterator<Tuple<TKey, TValue>> IterateTuples()
        {
            return this.Iterate().Map((KeyValuePair<TKey, TValue> entry) => Tuple.Create(entry.Key, entry.Value)).Iterate();
        }

        public abstract bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Iterate();
        }
    }

    public abstract class MapDecorator<TKey,TValue> : MapBase<TKey,TValue>
    {
        private readonly Map<TKey, TValue> innerMap;

        protected MapDecorator(Map<TKey, TValue> innerMap)
        {
            Pre.Condition.AssertNotNull(innerMap, nameof(innerMap));

            this.innerMap = innerMap;
        }

        public override IEnumerable<TKey> Keys => this.innerMap.Keys;

        public override IEnumerable<TValue> Values => this.innerMap.Values;

        public override int Count => this.innerMap.Count;

        public override bool ContainsKey(TKey key)
        {
            return this.innerMap.ContainsKey(key);
        }

        public override Result<TValue> Get(TKey key)
        {
            return this.innerMap.Get(key);
        }

        public override Iterator<KeyValuePair<TKey, TValue>> Iterate()
        {
            return this.innerMap.Iterate();
        }

        public override bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return this.innerMap.TryGetValue(key, out value);
        }
    }
}

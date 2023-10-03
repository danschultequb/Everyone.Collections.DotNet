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
    public static partial class Map
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
}

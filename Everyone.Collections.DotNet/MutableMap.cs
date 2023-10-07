using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Everyone
{
    /// <summary>
    /// A mapping between values of type <typeparamref name="TKey"/> and
    /// <typeparamref name="TValue"/> that can be modified.
    /// </summary>
    public static partial class MutableMap
    {
        public static MutableMap<TKey, TValue> Create<TKey, TValue>(params (TKey, TValue)[] values) where TKey : notnull
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return MutableMap.Create(values.Map(entry => Tuple.Create(entry.Item1, entry.Item2)));
        }

        public static MutableMap<TKey,TValue> Create<TKey,TValue>(IEnumerable<Tuple<TKey, TValue>> values) where TKey : notnull
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return MutableMap.Create(values.Map(t => new KeyValuePair<TKey, TValue>(t.Item1, t.Item2)));
        }

        public static MutableMap<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values) where TKey : notnull
        {
            return SystemDictionary.Create(values);
        }
    }

    /// <summary>
    /// A mapping between values of type <typeparamref name="TKey"/> and
    /// <typeparamref name="TValue"/> that can be modified.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in this <see cref="Map{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValue">The type of the values in this <see cref="Map{TKey, TValue}"/>.</typeparam>
    public interface MutableMap<TKey,TValue> : Map<TKey, TValue>, IDictionary<TKey,TValue>
    {
        public new int Count { get; }

        /// <summary>
        /// Get the keys in this <see cref="MutableMap"/>.
        /// </summary>
        public new Iterable<TKey> Keys { get; }

        /// <summary>
        /// Get the values in this <see cref="MutableMap"/>.
        /// </summary>
        public new Iterable<TValue> Values { get; }

        /// <summary>
        /// Get whether this <see cref="MutableMap{TKey,TValue}"/> contains the provided
        /// <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look for.</param>
        public new bool ContainsKey(TKey key);

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
        /// Set the provided <paramref name="key"/> and <paramref name="value"/> mapping in this
        /// <see cref="MutableMap{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the mapping.</param>
        /// <param name="value">The value of the mapping.</param>
        public MutableMap<TKey,TValue> Set(TKey key, TValue value);

        /// <summary>
        /// Set the provided <paramref name="entry"/> in this
        /// <see cref="MutableMap{TKey, TValue}"/>.
        /// </summary>
        /// <param name="entry">The entry to set in this <see cref="MutableMap{TKey, TValue}"/>.</param>
        public MutableMap<TKey, TValue> Set(Tuple<TKey, TValue> entry);

        /// <summary>
        /// Set the provided <paramref name="entry"/> in this
        /// <see cref="MutableMap{TKey, TValue}"/>.
        /// </summary>
        /// <param name="entry">The entry to set in this <see cref="MutableMap{TKey, TValue}"/>.</param>
        public MutableMap<TKey, TValue> Set(KeyValuePair<TKey, TValue> entry);

        public MutableMap<TKey, TValue> SetAll(params (TKey, TValue)[] values);

        public MutableMap<TKey, TValue> SetAll(IEnumerable<(TKey, TValue)> values);

        public MutableMap<TKey, TValue> SetAll(IEnumerable<Tuple<TKey, TValue>> values);

        public MutableMap<TKey, TValue> SetAll(IEnumerable<KeyValuePair<TKey, TValue>> values);
    }

    public abstract class MutableMapBase<TKey, TValue, TMutableMap> : MapBase<TKey, TValue>, MutableMap<TKey, TValue>
        where TMutableMap : class, MutableMap<TKey,TValue>
    {
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return base[key]; }
            set { this.Set(key, value); }
        }

        public bool IsReadOnly => false;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Keys.ToList();

        public override abstract Iterable<TKey> Keys { get; }

        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.Values.ToList();

        public override abstract Iterable<TValue> Values { get; }

        public override int Count => throw new NotImplementedException();

        public TMutableMap Add(TKey key, TValue value)
        {
            return this.Set(key, value);
        }
        
        public TMutableMap Add(KeyValuePair<TKey, TValue> item)
        {
            return this.Set(item);
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            this.Set(key, value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Set(item);
        }

        public abstract void Clear();
        
        public abstract bool Contains(KeyValuePair<TKey, TValue> item);

        public abstract void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);
        
        public abstract bool Remove(TKey key);
        
        public abstract bool Remove(KeyValuePair<TKey, TValue> item);

        public abstract TMutableMap Set(TKey key, TValue value);

        MutableMap<TKey, TValue> MutableMap<TKey, TValue>.Set(TKey key, TValue value)
        {
            return this.Set(key, value);
        }

        public virtual TMutableMap Set(Tuple<TKey, TValue> entry)
        {
            Pre.Condition.AssertNotNull(entry, nameof(entry));

            return this.Set(entry.Item1, entry.Item2);
        }

        MutableMap<TKey, TValue> MutableMap<TKey, TValue>.Set(Tuple<TKey, TValue> entry)
        {
            return this.Set(entry);
        }

        public virtual TMutableMap Set(KeyValuePair<TKey, TValue> entry)
        {
            Pre.Condition.AssertNotNull(entry, nameof(entry));

            return this.Set(entry.Key, entry.Value);
        }

        MutableMap<TKey, TValue> MutableMap<TKey, TValue>.Set(KeyValuePair<TKey, TValue> entry)
        {
            return this.Set(entry);
        }

        public TMutableMap SetAll(params (TKey, TValue)[] values)
        {
            return this.SetAll(System.Linq.Enumerable.ToList(values));
        }

        MutableMap<TKey, TValue> MutableMap<TKey, TValue>.SetAll(params (TKey, TValue)[] values)
        {
            return this.SetAll(values);
        }

        public TMutableMap SetAll(IEnumerable<(TKey, TValue)> values)
        {
            return this.SetAll(values.Map(entry => KeyValuePair.Create(entry.Item1, entry.Item2)));
        }

        MutableMap<TKey, TValue> MutableMap<TKey, TValue>.SetAll(IEnumerable<(TKey, TValue)> values)
        {
            return this.SetAll(values);
        }

        public TMutableMap SetAll(IEnumerable<Tuple<TKey, TValue>> values)
        {
            return this.SetAll(values.Map(entry => KeyValuePair.Create(entry.Item1, entry.Item2)));
        }

        MutableMap<TKey, TValue> MutableMap<TKey, TValue>.SetAll(IEnumerable<Tuple<TKey, TValue>> values)
        {
            return this.SetAll(values);
        }

        public TMutableMap SetAll(IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            foreach (KeyValuePair<TKey,TValue> entry in values)
            {
                this.Set(entry);
            }
            return (this as TMutableMap)!;
        }

        MutableMap<TKey, TValue> MutableMap<TKey, TValue>.SetAll(IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            return this.SetAll(values);
        }
    }

    public abstract class MutableMapDecorator<TKey, TValue, TMutableMap> : MutableMapBase<TKey, TValue, TMutableMap>
        where TMutableMap : class, MutableMap<TKey, TValue>
    {
        private readonly MutableMap<TKey, TValue> innerMap;

        protected MutableMapDecorator(MutableMap<TKey, TValue> innerMap)
        {
            Pre.Condition.AssertNotNull(innerMap, nameof(innerMap));

            this.innerMap = innerMap;
        }

        public override Iterable<TKey> Keys => this.innerMap.Keys;

        public override Iterable<TValue> Values => this.innerMap.Values;

        public override int Count => this.innerMap.Count;

        public override void Clear()
        {
            this.innerMap.Clear();
        }

        public override bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.innerMap.Contains(item);
        }

        public override bool ContainsKey(TKey key)
        {
            return this.innerMap.ContainsKey(key);
        }

        public override void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.innerMap.CopyTo(array, arrayIndex);
        }

        public override Result<TValue> Get(TKey key)
        {
            return this.innerMap.Get(key);
        }

        public override Iterator<KeyValuePair<TKey, TValue>> Iterate()
        {
            return this.innerMap.Iterate();
        }

        public override bool Remove(TKey key)
        {
            return this.innerMap.Remove(key);
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.innerMap.Remove(item);
        }

        public override TMutableMap Set(TKey key, TValue value)
        {
            this.innerMap.Set(key, value);

            return (this as TMutableMap)!;
        }

        public override bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return this.innerMap.TryGetValue(key, out value);
        }
    }
}

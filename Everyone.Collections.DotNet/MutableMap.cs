using System;
using System.Collections.Generic;

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
        /// Set the provided <paramref name="key"/> and <paramref name="value"/> mapping in this
        /// <see cref="MutableMap{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the mapping.</param>
        /// <param name="value">The value of the mapping.</param>
        public void Set(TKey key, TValue value);

        /// <summary>
        /// Set the provided <paramref name="entry"/> in this
        /// <see cref="MutableMap{TKey, TValue}"/>.
        /// </summary>
        /// <param name="entry">The entry to set in this <see cref="MutableMap{TKey, TValue}"/>.</param>
        public void Set(Tuple<TKey, TValue> entry);

        /// <summary>
        /// Set the provided <paramref name="entry"/> in this
        /// <see cref="MutableMap{TKey, TValue}"/>.
        /// </summary>
        /// <param name="entry">The entry to set in this <see cref="MutableMap{TKey, TValue}"/>.</param>
        public void Set(KeyValuePair<TKey, TValue> entry);

        public void SetAll(params (TKey, TValue)[] values);

        public void SetAll(IEnumerable<(TKey, TValue)> values);

        public void SetAll(IEnumerable<Tuple<TKey, TValue>> values);

        public void SetAll(IEnumerable<KeyValuePair<TKey, TValue>> values);

        public new ICollection<TKey> Keys { get; }
    }

    public abstract class MutableMapBase<TKey, TValue> : MapBase<TKey, TValue>, MutableMap<TKey, TValue>
    {
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return base[key]; }
            set { this.Set(key, value); }
        }

        public bool IsReadOnly => false;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Keys;

        public override abstract ICollection<TKey> Keys { get; }

        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.Values;

        public override abstract ICollection<TValue> Values { get; }

        public void Add(TKey key, TValue value)
        {
            this.Set(key, value);
        }
        
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Set(item);
        }
        
        public abstract void Clear();
        
        public abstract bool Contains(KeyValuePair<TKey, TValue> item);
        
        public abstract void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);
        
        public abstract bool Remove(TKey key);
        
        public abstract bool Remove(KeyValuePair<TKey, TValue> item);

        public abstract void Set(TKey key, TValue value);

        public virtual void Set(Tuple<TKey, TValue> entry)
        {
            Pre.Condition.AssertNotNull(entry, nameof(entry));

            this.Set(entry.Item1, entry.Item2);
        }

        public virtual void Set(KeyValuePair<TKey, TValue> entry)
        {
            Pre.Condition.AssertNotNull(entry, nameof(entry));

            this.Set(entry.Key, entry.Value);
        }

        public void SetAll(params (TKey, TValue)[] values)
        {
            this.SetAll(System.Linq.Enumerable.ToList(values));
        }

        public void SetAll(IEnumerable<(TKey, TValue)> values)
        {
            this.SetAll(values.Map(entry => KeyValuePair.Create(entry.Item1, entry.Item2)));
        }

        public void SetAll(IEnumerable<Tuple<TKey, TValue>> values)
        {
            this.SetAll(values.Map(entry => KeyValuePair.Create(entry.Item1, entry.Item2)));
        }

        public void SetAll(IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            foreach (KeyValuePair<TKey,TValue> entry in values)
            {
                this.Set(entry);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everyone
{
    public static class SystemHashSet
    {
        /// <summary>
        /// Create a new <see cref="SystemHashSet{T}"/> with the provided <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T">The type of values stored in the returned
        /// <see cref="SystemHashSet{T}"/>.</typeparam>
        /// <param name="values">The initial values of the returned <see cref="SystemHashSet{T}"/>.</param>
        public static SystemHashSet<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return SystemHashSet.Create<T>(values.ToList());
        }

        /// <summary>
        /// Create a new <see cref="SystemHashSet{T}"/> with the provided <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T">The type of values stored in the returned
        /// <see cref="SystemHashSet{T}"/>.</typeparam>
        /// <param name="values">The initial values of the returned <see cref="SystemHashSet{T}"/>.</param>
        public static SystemHashSet<T> Create<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            return SystemHashSet<T>.Create(values);
        }
    }
    public class SystemHashSet<T> : MutableSetBase<T,SystemHashSet<T>>
    {
        private readonly System.Collections.Generic.HashSet<T> hashSet;

        protected SystemHashSet()
        {
            this.hashSet = new System.Collections.Generic.HashSet<T>();
        }

        public static SystemHashSet<T> Create(System.Collections.Generic.IEnumerable<T> values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return new SystemHashSet<T>().AddAll(values);
        }

        public override int Count => this.hashSet.Count;

        public override SystemHashSet<T> Add(T value)
        {
            this.hashSet.Add(value);
            return this;
        }

        public override SystemHashSet<T> Add(T value, out bool added)
        {
            added = this.hashSet.Add(value);
            return this;
        }

        public override void Clear()
        {
            this.hashSet.Clear();
        }

        public override bool Contains(T value)
        {
            return this.hashSet.Contains(value);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            Pre.Condition.AssertNotNull(array, nameof(array));
            Pre.Condition.AssertAccessIndex(arrayIndex, array, nameof(arrayIndex));

            this.hashSet.CopyTo(array, arrayIndex);
        }

        public override void IntersectWith(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            this.hashSet.IntersectWith(other);
        }

        public override bool IsProperSubsetOf(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            return this.hashSet.IsProperSubsetOf(other);
        }

        public override bool IsProperSupersetOf(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            return this.hashSet.IsProperSupersetOf(other);
        }

        public override bool IsSubsetOf(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            return this.hashSet.IsSubsetOf(other);
        }

        public override bool IsSupersetOf(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            return this.hashSet.IsSupersetOf(other);
        }

        public override Iterator<T> Iterate()
        {
            return this.hashSet.Iterate();
        }

        public override bool Overlaps(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            return this.hashSet.Overlaps(other);
        }

        public override bool Remove(T value)
        {
            return this.hashSet.Remove(value);
        }

        public override bool SetEquals(IEnumerable<T> other)
        {
            return this.hashSet.SetEquals(other);
        }

        public override void SymmetricExceptWith(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            this.hashSet.SymmetricExceptWith(other);
        }

        public override void UnionWith(IEnumerable<T> other)
        {
            Pre.Condition.AssertNotNull(other, nameof(other));

            this.hashSet.UnionWith(other);
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Everyone
{
    public static class Set
    {
        /// <summary>
        /// Create a new <see cref="Set{T}"/> with the provided <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T">The type of values stored in the returned <see cref="Set{T}"/>.</typeparam>
        /// <param name="values">The initial values of the returned <see cref="Set{T}"/>.</param>
        public static MutableSet<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return Set.Create<T>(values.ToList());
        }

        /// <summary>
        /// Create a new <see cref="Set{T}"/> with the provided <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T">The type of values stored in the returned <see cref="Set{T}"/>.</typeparam>
        /// <param name="values">The initial values of the returned <see cref="Set{T}"/>.</param>
        public static MutableSet<T> Create<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            return MutableSet.Create(values);
        }
    }

    /// <summary>
    /// An <see cref="Iterable{T}"/> that only has unique values.
    /// </summary>
    /// <typeparam name="T">The type of values stored in the <see cref="Set{T}"/>.</typeparam>
    public interface Set<T> : Iterable<T>, System.Collections.Generic.IReadOnlySet<T>
    {
        /// <summary>
        /// Get the number of values in this <see cref="Set{T}"/>.
        /// </summary>
        public new int Count { get; }
    }

    public abstract class SetBase<T> : IterableBase<T>, Set<T>
    {
        protected SetBase()
        {
        }

        public abstract int Count { get; }

        public abstract bool Contains(T value);

        public override abstract Iterator<T> Iterate();

        public abstract bool IsProperSubsetOf(System.Collections.Generic.IEnumerable<T> other);

        public abstract bool IsProperSupersetOf(System.Collections.Generic.IEnumerable<T> other);

        public abstract bool IsSubsetOf(System.Collections.Generic.IEnumerable<T> other);

        public abstract bool IsSupersetOf(System.Collections.Generic.IEnumerable<T> other);

        public abstract bool Overlaps(System.Collections.Generic.IEnumerable<T> other);

        public abstract bool SetEquals(System.Collections.Generic.IEnumerable<T> other);
    }

    public abstract class SetDecorator<T> : SetBase<T>
    {
        private readonly Set<T> innerSet;

        protected SetDecorator(Set<T> innerSet)
        {
            Pre.Condition.AssertNotNull(innerSet, nameof(innerSet));

            this.innerSet = innerSet;
        }

        public override int Count => this.innerSet.Count;

        public override bool Contains(T value)
        {
            return this.innerSet.Contains(value);
        }

        public override bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.innerSet.IsProperSubsetOf(other);
        }

        public override bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.innerSet.IsProperSupersetOf(other);
        }

        public override bool IsSubsetOf(IEnumerable<T> other)
        {
            return this.innerSet.IsSubsetOf(other);
        }

        public override bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.innerSet.IsSupersetOf(other);
        }

        public override Iterator<T> Iterate()
        {
            return this.innerSet.Iterate();
        }

        public override bool Overlaps(IEnumerable<T> other)
        {
            return this.innerSet.Overlaps(other);
        }

        public override bool SetEquals(IEnumerable<T> other)
        {
            return this.innerSet.SetEquals(other);
        }
    }
}

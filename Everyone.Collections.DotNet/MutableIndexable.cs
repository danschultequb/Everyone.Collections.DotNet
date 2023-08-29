using System.Collections.Generic;
using System.Linq;

namespace Everyone
{
    public static class MutableIndexable
    {
        public static MutableIndexable<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return MutableIndexable.Create<T>(values.ToList());
        }

        public static MutableIndexable<T> Create<T>(IEnumerable<T> values)
        {
            return List.Create(values);
        }
    }

    /// <summary>
    /// A sequence of values that can be accessed and modified via an index.
    /// </summary>
    /// <typeparam name="T">The type of the values in this <see cref="Indexable{T}"/>.</typeparam>
    public interface MutableIndexable<T> : Indexable<T>
    {
        /// <summary>
        /// Set the value at the provided index to be the provided <paramref name="value"/>.
        /// </summary>
        /// <param name="index">The index to change.</param>
        /// <param name="value">The value to assign to the provided index.</param>
        public void Set(int index, T value);

        /// <summary>
        /// Get or set the value in this <see cref="MutableIndexable{T}"/> at the provided
        /// <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The <paramref name="index"/> of the value to return.</param>
        public new T this[int index] { get; set; }
    }

    public abstract class MutableIndexableBase<T> : IndexableBase<T>, MutableIndexable<T>
    {
        public abstract void Set(int index, T value);

        public new T this[int index]
        {
            get { return base[index]; }
            set { this.Set(index, value); }
        }
    }
}

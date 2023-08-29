namespace Everyone
{
    /// <summary>
    /// A sequence of values that can be accessed via an index.
    /// </summary>
    public static partial class Indexable
    {
        public static Indexable<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return Indexable.Create<T>(System.Linq.Enumerable.ToList(values));
        }

        public static Indexable<T> Create<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            return MutableIndexable.Create(values);
        }
    }

    /// <summary>
    /// A sequence of values that can be accessed via an index.
    /// </summary>
    /// <typeparam name="T">The type of the values in this <see cref="Indexable{T}"/>.</typeparam>
    public interface Indexable<T> : Iterable<T>, System.Collections.Generic.IReadOnlyList<T>
    {
        /// <summary>
        /// Get the last index in this <see cref="Indexable{T}"/>.
        /// </summary>
        public int EndIndex { get; }

        /// <summary>
        /// Get the value in this <see cref="Indexable{T}"/> at the provided
        /// <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The <paramref name="index"/> of the value to return.</param>
        public T Get(int index);

        /// <summary>
        /// Get an <see cref="IndexableIterator{T}"/> that can be used to iterate through the
        /// values in this <see cref="Indexable{T}"/>.
        /// </summary>
        public new IndexableIterator<T> Iterate();
    }

    public abstract class IndexableBase<T> : IterableBase<T>, Indexable<T>
    {
        public abstract int Count { get; }

        public int EndIndex
        {
            get
            {
                Pre.Condition.AssertNotNullAndNotEmpty(this, "this", $"Cannot get the {nameof(EndIndex)} of an empty {nameof(Indexable)}.");

                return this.Count - 1;
            }
        }

        public abstract T Get(int index);

        public override abstract IndexableIterator<T> Iterate();

        public virtual T this[int index]
        {
            get { return this.Get(index); }
        }
    }
}

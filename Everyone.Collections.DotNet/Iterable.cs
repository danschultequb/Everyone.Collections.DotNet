namespace Everyone
{
    /// <summary>
    /// A collection of values that can be iterated over.
    /// </summary>
    public static partial class Iterable
    {
        public static Iterable<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return Iterable.Create<T>(System.Linq.Enumerable.ToList(values));
        }

        public static Iterable<T> Create<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            return Indexable.Create(values);
        }
    }

    /// <summary>
    /// A collection of values that can be iterated over.
    /// </summary>
    /// <typeparam name="T">The type of values that are contained in this
    /// <see cref="Iterable{T}"/>.</typeparam>
    public interface Iterable<T> : System.Collections.Generic.IEnumerable<T>
    {
        /// <summary>
        /// Get an <see cref="Iterator{T}"/> that can be used to iterate through the values in this
        /// <see cref="Iterable{T}"/>.
        /// </summary>
        public Iterator<T> Iterate();
    }

    /// <summary>
    /// A base implementation of the <see cref="Iterable{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of values contained by this <see cref="Iterable{T}"/>.</typeparam>
    public abstract class IterableBase<T> : Iterable<T>
    {
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return this.Iterate();
        }

        public abstract Iterator<T> Iterate();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Iterate();
        }
    }
}

namespace Everyone
{
    /// <summary>
    /// A collection of values that can be iterated over.
    /// </summary>
    public static class Iterable
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

        public override string ToString()
        {
            return Enumerables.ToString(this);
        }

        public override bool Equals(object? rhs)
        {
            return rhs is System.Collections.IEnumerable rhsEnumerable &&
                Enumerables.SequenceEqual(this, rhsEnumerable);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = HashCode.Create();
            foreach (T value in this)
            {
                hashCode.Add(value?.GetHashCode());
            }
            return hashCode.Value;
        }
    }

    public abstract class IterableDecorator<T> : IterableBase<T>
    {
        private readonly Iterable<T> innerIterable;

        protected IterableDecorator(Iterable<T> innerIterable)
        {
            Pre.Condition.AssertNotNull(innerIterable, nameof(innerIterable));

            this.innerIterable = innerIterable;
        }

        public override Iterator<T> Iterate()
        {
            return this.innerIterable.Iterate();
        }

        public override string ToString()
        {
            return this.innerIterable.ToString()!;
        }

        public override bool Equals(object? rhs)
        {
            return this.innerIterable.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return this.innerIterable.GetHashCode();
        }
    }
}

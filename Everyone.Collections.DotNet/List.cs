namespace Everyone
{
    /// <summary>
    /// A sequence of values that can add or insert new values.
    /// </summary>
    public static class List
    {
        public static List<T> Create<T>(params T[] initialValues)
        {
            Pre.Condition.AssertNotNull(initialValues, nameof(initialValues));

            return List.Create<T>(System.Linq.Enumerable.ToList(initialValues));
        }

        public static List<T> Create<T>(System.Collections.Generic.IEnumerable<T> initialValues)
        {
            return SystemList.Create(initialValues);
        }
    }

    /// <summary>
    /// A sequence of values that can add or insert new values.
    /// </summary>
    /// <typeparam name="T">The type of values stored in this <see cref="List{T}"/>.</typeparam>
    public interface List<T> : MutableIndexable<T>, System.Collections.Generic.IList<T>
    {
        public void AddAll(params T[] values);

        public void AddAll(System.Collections.Generic.IEnumerable<T> values);

        /// <summary>
        /// Get or set the value in this <see cref="MutableIndexable{T}"/> at the provided
        /// <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The <paramref name="index"/> of the value to return.</param>
        public new T this[int index] { get; set; }
    }

    public abstract class ListBase<T> : MutableIndexableBase<T>, List<T>
    {
        public bool IsReadOnly => false;

        public abstract void Insert(int index, T value);

        public void Add(T value)
        {
            this.Insert(this.Count, value);
        }

        public virtual void AddAll(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            foreach (T value in values)
            {
                this.Add(value);
            }
        }

        public virtual void AddAll(System.Collections.Generic.IEnumerable<T> values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            foreach (T value in values)
            {
                this.Add(value);
            }
        }

        public abstract int IndexOf(T item);

        public abstract void RemoveAt(int index);

        public abstract void Clear();

        public abstract bool Contains(T item);

        public abstract void CopyTo(T[] array, int arrayIndex);

        public abstract bool Remove(T item);
    }
}

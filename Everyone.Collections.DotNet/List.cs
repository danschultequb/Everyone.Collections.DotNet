using System.Collections.Generic;

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
        /// <summary>
        /// Get the number of values in this <see cref="List{T}"/>.
        /// </summary>
        public new int Count { get; }

        /// <summary>
        /// Add all of the provided <paramref name="values"/> to this <see cref="List{T}"/>.
        /// </summary>
        /// <param name="values">The values to add.</param>
        public List<T> AddAll(params T[] values);

        /// <summary>
        /// Add all of the provided <paramref name="values"/> to this <see cref="List{T}"/>.
        /// </summary>
        /// <param name="values">The values to add.</param>
        public List<T> AddAll(System.Collections.Generic.IEnumerable<T> values);

        /// <summary>
        /// Get or set the value in this <see cref="MutableIndexable{T}"/> at the provided
        /// <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The <paramref name="index"/> of the value to return.</param>
        public new T this[int index] { get; set; }

        /// <summary>
        /// Get the index of the provided <paramref name="value"/> in this <see cref="List{T}"/>.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        public new Result<int> IndexOf(T value);
    }

    public abstract class ListBase<T,TList> : MutableIndexableBase<T,TList>, List<T>
        where TList : class, List<T>
    {
        public override abstract int Count { get; }

        public bool IsReadOnly => false;

        public abstract TList Insert(int index, T value);

        void IList<T>.Insert(int index, T item)
        {
            this.Insert(index, item);
        }

        public TList Add(T value)
        {
            return this.Insert(this.Count, value);
        }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public virtual TList AddAll(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            foreach (T value in values)
            {
                this.Add(value);
            }

            return (this as TList)!;
        }

        List<T> List<T>.AddAll(params T[] values)
        {
            return this.AddAll(values);
        }

        public virtual TList AddAll(System.Collections.Generic.IEnumerable<T> values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            foreach (T value in values)
            {
                this.Add(value);
            }

            return (this as TList)!;
        }

        List<T> List<T>.AddAll(IEnumerable<T> values)
        {
            return this.AddAll(values);
        }

        public override abstract Result<int> IndexOf(T item);

        public abstract void RemoveAt(int index);

        public abstract void Clear();

        public abstract bool Contains(T item);

        public abstract void CopyTo(T[] array, int arrayIndex);

        public abstract bool Remove(T item);

        int IList<T>.IndexOf(T item)
        {
            return this.IndexOf(item).Await();
        }
    }

    public abstract class ListDecorator<T,TList> : ListBase<T,TList>
        where TList : class, List<T>
    {
        private readonly List<T> innerList;

        protected ListDecorator(List<T> innerList)
        {
            Pre.Condition.AssertNotNull(innerList, nameof(innerList));

            this.innerList = innerList;
        }

        public override int Count => this.innerList.Count;

        public override void Clear()
        {
            this.innerList.Clear();
        }

        public override bool Contains(T item)
        {
            return this.innerList.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            this.innerList.CopyTo(array, arrayIndex);
        }

        public override T Get(int index)
        {
            return this.innerList.Get(index);
        }

        public override Result<int> IndexOf(T item)
        {
            return this.innerList.IndexOf(item);
        }

        public override TList Insert(int index, T value)
        {
            this.innerList.Insert(index, value);

            return (this as TList)!;
        }

        public override IndexableIterator<T> Iterate()
        {
            return this.innerList.Iterate();
        }

        public override bool Remove(T item)
        {
            return this.innerList.Remove(item);
        }

        public override void RemoveAt(int index)
        {
            this.innerList.RemoveAt(index);
        }

        public override TList Set(int index, T value)
        {
            this.innerList.Set(index, value);

            return (this as TList)!;
        }
    }
}

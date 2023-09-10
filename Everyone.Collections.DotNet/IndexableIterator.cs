namespace Everyone
{
    /// <summary>
    /// An object that can iterate over a sequence of values.
    /// </summary>
    public static partial class IndexableIterator
    {
        public static IndexableIterator<T> Create<T>(params T[] values)
        {
            return IndexableIterator.Create<T>(System.Linq.Enumerable.ToList(values));
        }

        public static IndexableIterator<T> Create<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return IndexableIterator.Create(values.GetEnumerator());
        }

        public static IndexableIterator<T> Create<T>(System.Collections.Generic.IEnumerator<T> values)
        {
            return IndexableIterator.Create(Iterator.Create(values));
        }

        public static IndexableIterator<T> Create<T>(Iterator<T> enumerator)
        {
            return BasicIndexableIterator<T>.Create(enumerator);
        }
    }

    /// <summary>
    /// An <see cref="Iterator{T}"/> that maintains a current index.
    /// </summary>
    /// <typeparam name="T">The type of values returned by this <see cref="IndexableIterator{T}"/>.</typeparam>
    public interface IndexableIterator<T> : Iterator<T>
    {
        /// <summary>
        /// The index that this <see cref="IndexableIterator{T}"/> is currently pointing at.
        /// </summary>
        public int CurrentIndex { get; }

        public new IndexableIterator<T> Start();
    }

    public class BasicIndexableIterator<T> : IteratorDecorator<T, BasicIndexableIterator<T>>, IndexableIterator<T>
    {
        private int currentIndex;

        protected BasicIndexableIterator(Iterator<T> innerDecorator)
            : base(innerDecorator)
        {
            this.currentIndex = -1;
        }

        public static BasicIndexableIterator<T> Create(Iterator<T> enumerator)
        {
            return new BasicIndexableIterator<T>(enumerator);
        }

        public int CurrentIndex
        {
            get
            {
                Pre.Condition.AssertTrue(this.HasCurrent(), "this.HasCurrent()");

                return this.currentIndex;
            }
        }

        public override bool Next()
        {
            bool result = base.Next();
            if (result)
            {
                this.currentIndex++;
            }
            return result;
        }

        IndexableIterator<T> IndexableIterator<T>.Start()
        {
            return this.Start();
        }
    }
}

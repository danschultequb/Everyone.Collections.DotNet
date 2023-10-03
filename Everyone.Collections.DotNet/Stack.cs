namespace Everyone
{
    public static class Stack
    {
        public static Stack<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return Stack.Create<T>(System.Linq.Enumerable.ToList(values));
        }

        public static Stack<T> Create<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            return SystemStack.Create(values);
        }
    }

    /// <summary>
    /// A collection that contains values in a LIFO (last in first out) order.
    /// </summary>
    /// <typeparam name="T">The type of values contained by this <see cref="Stack{T}"/>.</typeparam>
    public interface Stack<T>
    {
        /// <summary>
        /// The number of values stored in this <see cref="Stack{T}"/>.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Whether this <see cref="Stack{T}"/> contains any values.
        /// </summary>
        public bool Any();

        /// <summary>
        /// Add the provided value to the top of this <see cref="Stack{T}"/>.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>This object for method chaining.</returns>
        public Stack<T> Add(T value);

        /// <summary>
        /// Add all of the provided values to the top of this <see cref="Stack{T}"/>.
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This object for method chaining.</returns>
        public Stack<T> AddAll(params T[] values);

        /// <summary>
        /// Add all of the provided values to the top of this <see cref="Stack{T}"/>.
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This object for method chaining.</returns>
        public Stack<T> AddAll(System.Collections.Generic.IEnumerable<T> values);

        /// <summary>
        /// Get the top value on this <see cref="Stack{T}"/> without removing it.
        /// </summary>
        public Result<T> Peek();

        /// <summary>
        /// Remove the top value from this <see cref="Stack{T}"/>.
        /// </summary>
        public Result<T> Remove();

        /// <summary>
        /// Get whether this <see cref="Stack{T}"/> contains the provided <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        public bool Contains(T value);
    }

    public abstract class StackBase<T, TStack> : Stack<T> where TStack : class, Stack<T>
    {
        public abstract int Count { get; }

        Stack<T> Stack<T>.Add(T value)
        {
            return this.Add(value);
        }

        public abstract TStack Add(T value);

        Stack<T> Stack<T>.AddAll(params T[] values)
        {
            return this.AddAll(values);
        }

        public virtual TStack AddAll(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            foreach (T value in values)
            {
                this.Add(value);
            }

            return (this as TStack)!;
        }

        Stack<T> Stack<T>.AddAll(System.Collections.Generic.IEnumerable<T> values)
        {
            return this.AddAll(values);
        }

        public virtual TStack AddAll(System.Collections.Generic.IEnumerable<T> values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            foreach (T value in values)
            {
                this.Add(value);
            }

            return (this as TStack)!;
        }

        public virtual bool Any()
        {
            return this.Count > 0;
        }

        public abstract bool Contains(T value);

        public abstract Result<T> Remove();

        public abstract Result<T> Peek();
    }

    public abstract class StackDecorator<T,TStack> : StackBase<T,TStack> where TStack : class, Stack<T>
    {
        private readonly Stack<T> innerStack;

        protected StackDecorator(Stack<T> innerStack)
        {
            Pre.Condition.AssertNotNull(innerStack, nameof(innerStack));

            this.innerStack = innerStack;
        }

        public override int Count
        {
            get { return this.innerStack.Count; }
        }

        public override TStack Add(T value)
        {
            this.innerStack.Add(value);

            return (this as TStack)!;
        }

        public override bool Contains(T value)
        {
            return this.innerStack.Contains(value);
        }

        public override Result<T> Remove()
        {
            return this.innerStack.Remove();
        }

        public override Result<T> Peek()
        {
            return this.innerStack.Peek();
        }
    }
}

using System;

namespace Everyone
{
    public static class SystemStack
    {
        public static SystemStack<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return SystemStack.Create<T>(System.Linq.Enumerable.ToList(values));
        }

        public static SystemStack<T> Create<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            return SystemStack<T>.Create(values);
        }
    }

    public class SystemStack<T> : StackBase<T, SystemStack<T>>
    {
        private readonly System.Collections.Generic.Stack<T> stack;

        protected SystemStack()
        {
            this.stack = new System.Collections.Generic.Stack<T>();
        }

        public static SystemStack<T> Create(System.Collections.Generic.IEnumerable<T> values)
        {
            return new SystemStack<T>().AddAll(values);
        }

        public override int Count
        {
            get { return this.stack.Count; }
        }

        public override SystemStack<T> Add(T value)
        {
            this.stack.Push(value);

            return this;
        }

        public override bool Contains(T value)
        {
            return this.stack.Contains(value);
        }

        public override Result<T> Remove()
        {
            return Result.Create(() =>
            {
                T result;
                try
                {
                    result = this.stack.Pop();
                }
                catch (InvalidOperationException)
                {
                    throw new EmptyException();
                }
                return result;
            });
        }

        public override Result<T> Peek()
        {
            return Result.Create(() =>
            {
                T result;
                try
                {
                    result = this.stack.Peek();
                }
                catch (InvalidOperationException)
                {
                    throw new EmptyException();
                }
                return result;
            });
        }
    }
}

﻿namespace Everyone
{
    public static class AssertionsExtensions
    {
        public static TAssertions AssertAccessIndex<TAssertions, T>(this TAssertions assertions, int index, T[] indexable, string? expression = null, string? message = null) where TAssertions : Assertions
        {
            return assertions.AssertAccessIndex(
                indexable: indexable,
                index: index,
                parameters: new AssertParameters
                {
                    Expression = expression,
                    Message = message,
                });
        }

        public static TAssertions AssertAccessIndex<TAssertions, T>(this TAssertions assertions, int index, T[] indexable, AssertParameters? parameters) where TAssertions : Assertions
        {
            Pre.Condition.AssertNotNull(assertions, nameof(assertions));
            Pre.Condition.AssertNotNullAndNotEmpty(indexable, "this");

            assertions.AssertBetween(0, index, indexable.Length - 1, parameters);

            return assertions;
        }

        public static TAssertions AssertAccessIndex<TAssertions,T>(this TAssertions assertions, int index, Indexable<T> indexable, string? expression = null, string? message = null) where TAssertions : Assertions
        {
            return assertions.AssertAccessIndex(
                indexable: indexable,
                index: index,
                parameters: new AssertParameters
                {
                    Expression = expression,
                    Message = message,
                });
        }

        public static TAssertions AssertAccessIndex<TAssertions,T>(this TAssertions assertions, int index, Indexable<T> indexable, AssertParameters? parameters) where TAssertions : Assertions
        {
            Pre.Condition.AssertNotNull(assertions, nameof(assertions));
            Pre.Condition.AssertNotNullAndNotEmpty(indexable, "this");
            
            assertions.AssertBetween(0, index, indexable.Count - 1, parameters);
            
            return assertions;
        }

        public static TAssertions AssertInsertIndex<TAssertions, T>(this TAssertions assertions, int index, Indexable<T> indexable, string? expression = null, string? message = null) where TAssertions : Assertions
        {
            return assertions.AssertInsertIndex(
                indexable: indexable,
                index: index,
                parameters: new AssertParameters
                {
                    Expression = expression,
                    Message = message,
                });
        }

        public static TAssertions AssertInsertIndex<TAssertions, T>(this TAssertions assertions, int index, Indexable<T> indexable, AssertParameters? parameters) where TAssertions : Assertions
        {
            Pre.Condition.AssertNotNull(assertions, nameof(assertions));
            Pre.Condition.AssertNotNull(indexable, nameof(indexable));

            assertions.AssertBetween(0, index, indexable.Count, parameters);

            return assertions;
        }

        public static TAssertions AssertLength<TAssertions, T>(this TAssertions assertions, int length, T[] indexable, int startIndex, string? expression = null, string? message = null) where TAssertions : Assertions
        {
            return assertions.AssertLength(
                length: length,
                indexable: indexable,
                startIndex: startIndex,
                parameters: new AssertParameters
                {
                    Expression = expression,
                    Message = message
                });
        }

        public static TAssertions AssertLength<TAssertions, T>(this TAssertions assertions, int length, T[] indexable, int startIndex, AssertParameters? parameters) where TAssertions : Assertions
        {
            Pre.Condition.AssertNotNull(assertions, nameof(assertions));
            Pre.Condition.AssertNotNull(indexable, nameof(indexable));

            assertions.AssertBetween(0, length, indexable.Length - startIndex, parameters);

            return assertions;
        }

        public static TAssertions AssertLength<TAssertions, T>(this TAssertions assertions, int length, Indexable<T> indexable, int startIndex, string? expression = null, string? message = null) where TAssertions : Assertions
        {
            return assertions.AssertLength(
                length: length,
                indexable: indexable,
                startIndex: startIndex,
                parameters: new AssertParameters
                {
                    Expression = expression,
                    Message = message
                });
        }

        public static TAssertions AssertLength<TAssertions, T>(this TAssertions assertions, int length, Indexable<T> indexable, int startIndex, AssertParameters? parameters) where TAssertions : Assertions
        {
            Pre.Condition.AssertNotNull(assertions, nameof(assertions));
            Pre.Condition.AssertNotNull(indexable, nameof(indexable));

            assertions.AssertBetween(0, length, indexable.Count - startIndex, parameters);

            return assertions;
        }
    }
}

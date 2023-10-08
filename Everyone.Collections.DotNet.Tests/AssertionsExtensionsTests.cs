namespace Everyone
{
    public static class AssertionsExtensionsTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(AssertionsExtensions), () =>
            {
                runner.TestMethod("AssertAccessIndex<TAssertions, T>(this TAssertions,int,T[],string?,string?)", () =>
                {
                    runner.Test("with null assertions", (Test test) =>
                    {
                        test.AssertThrows(() =>
                            {
                                AssertionsExtensions.AssertAccessIndex<Assertions,char>(
                                    assertions: null!,
                                    index: 0,
                                    indexable: new char[0],
                                    expression: "a",
                                    message: "b");
                            },
                            new PreConditionFailure(
                                "Expression: assertions",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with null indexable", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 0,
                                indexable: (char[])null!,
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Expression: this",
                                "Expected: not null and not empty",
                                "Actual:   null"));
                    });

                    runner.Test("with empty indexable", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 0,
                                indexable: new char[0],
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Expression: this",
                                "Expected: not null and not empty",
                                "Actual:   []"));
                    });

                    runner.Test("with negative index and indexable with one value", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: -1,
                                indexable: new[] { 'c' },
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: 0",
                                "Actual:   -1"));
                    });

                    runner.Test("with negative index and indexable with two values", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: -1,
                                indexable: new[] { 'c', 'd' },
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: between 0 and 1",
                                "Actual:   -1"));
                    });

                    runner.Test("with too large index and indexable with one value", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 1,
                                indexable: new[] { 'c' },
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: 0",
                                "Actual:   1"));
                    });

                    runner.Test("with too large index and indexable with two values", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 2,
                                indexable: new[] { 'c', 'd' },
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: between 0 and 1",
                                "Actual:   2"));
                    });

                    runner.Test("with valid arguments", (Test test) =>
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: i,
                                indexable: new[] { 'c', 'd' },
                                expression: "a",
                                message: "b");
                        }
                    });
                });

                runner.TestMethod("AssertAccessIndex<TAssertions, T>(this TAssertions,int,Indexable<T>,string?,string?)", () =>
                {
                    runner.Test("with null assertions", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex<Assertions, char>(
                                assertions: null!,
                                index: 0,
                                indexable: Indexable.Create<char>(),
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Expression: assertions",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with null indexable", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 0,
                                indexable: (Indexable<char>)null!,
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Expression: this",
                                "Expected: not null and not empty",
                                "Actual:   null"));
                    });

                    runner.Test("with empty indexable", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 0,
                                indexable: Indexable.Create<char>(),
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Expression: this",
                                "Expected: not null and not empty",
                                "Actual:   []"));
                    });

                    runner.Test("with negative index and indexable with one value", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: -1,
                                indexable: Indexable.Create('c'),
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: 0",
                                "Actual:   -1"));
                    });

                    runner.Test("with negative index and indexable with two values", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: -1,
                                indexable: Indexable.Create('c', 'd'),
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: between 0 and 1",
                                "Actual:   -1"));
                    });

                    runner.Test("with too large index and indexable with one value", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 1,
                                indexable: Indexable.Create('c'),
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: 0",
                                "Actual:   1"));
                    });

                    runner.Test("with too large index and indexable with two values", (Test test) =>
                    {
                        test.AssertThrows(() =>
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: 2,
                                indexable: Indexable.Create('c', 'd'),
                                expression: "a",
                                message: "b");
                        },
                            new PreConditionFailure(
                                "Message: b",
                                "Expression: a",
                                "Expected: between 0 and 1",
                                "Actual:   2"));
                    });

                    runner.Test("with valid arguments", (Test test) =>
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            AssertionsExtensions.AssertAccessIndex(
                                assertions: Pre.Condition,
                                index: i,
                                indexable: Indexable.Create('c', 'd'),
                                expression: "a",
                                message: "b");
                        }
                    });
                });
            });
        }
    }
}

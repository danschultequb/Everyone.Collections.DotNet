using System;

namespace Everyone
{
    public static class IndexableTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(Indexable), () =>
            {
                runner.TestMethod("Create<T>(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        Indexable<int> indexable = Indexable.Create<int>();
                        test.AssertEqual(new int[0], indexable);
                    });

                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => Indexable.Create((int[])null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        Indexable<int> indexable = Indexable.Create(10);
                        test.AssertEqual(new[] { 10 }, indexable);
                    });

                    runner.Test("with two value", (Test test) =>
                    {
                        Indexable<int> indexable = Indexable.Create(10, 20);
                        test.AssertEqual(new[] { 10, 20 }, indexable);
                    });
                });

                IndexableTests.Test(runner, Indexable.Create);
            });
        }

        public static void Test(TestRunner runner, Func<int[],Indexable<int>> creator)
        {
            Pre.Condition.AssertNotNull(runner, nameof(runner));
            Pre.Condition.AssertNotNull(creator, nameof(creator));

            runner.TestType($"{nameof(Indexable)}<T>", () =>
            {
                IterableTests.Test(runner, creator);

                runner.TestMethod("Count", () =>
                {
                    void CountTest(int[] values)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Indexable<int> indexable = creator(values);
                            test.AssertNotNull(indexable);
                            test.AssertEqual(values.Length, indexable.Count);
                        });
                    }

                    CountTest(new int[0]);
                    CountTest(new[] { 1 });
                    CountTest(new[] { 1, 2 });
                    CountTest(new[] { 1, 2, 3 });
                });

                runner.TestMethod("EndIndex", () =>
                {
                    void EndIndexTest(int[] initialValues, int? expectedValue = null, Exception? expectedException = null)
                    {
                        runner.Test($"with {initialValues.Length} initial values", (Test test) =>
                        {
                            Indexable<int> indexable = creator(initialValues);
                            test.AssertThrows(expectedException, () =>
                            {
                                test.AssertEqual(expectedValue, indexable.EndIndex);
                            });
                        });
                    }

                    EndIndexTest(
                        initialValues: new int[0],
                        expectedException: new PreConditionFailure(
                            "Message: Cannot get the EndIndex of an empty Indexable.",
                            "Expression: this",
                            "Expected: not null and not empty",
                            "Actual:   []"));
                    EndIndexTest(
                        initialValues: new int[1],
                        expectedValue: 0);
                    EndIndexTest(
                        initialValues: new int[2],
                        expectedValue: 1);
                    EndIndexTest(
                        initialValues: new int[100],
                        expectedValue: 99);
                });

                runner.TestMethod("Iterate()", (Test test) =>
                {
                    void IterateTest(int[] values)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Indexable<int> iterable = creator(values);
                            test.AssertNotNull(iterable);

                            IndexableIterator<int> iterator = iterable.Iterate();
                            test.AssertNotNull(iterator);
                            test.AssertFalse(iterator.HasStarted());
                            test.AssertFalse(iterator.HasCurrent());

                            for (int i = 0; i < values.Length; i++)
                            {
                                test.AssertEqual(i, iterator.CurrentIndex);
                                test.AssertTrue(iterator.Next());
                                test.AssertTrue(iterator.HasStarted());
                                test.AssertTrue(iterator.HasCurrent());
                                test.AssertEqual(values[i], iterator.Current);
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                test.AssertFalse(iterator.Next());
                                test.AssertTrue(iterator.HasStarted());
                                test.AssertFalse(iterator.HasCurrent());
                                test.AssertThrows(() =>
                                    {
                                        int value = iterator.Current;
                                        test.AssertEqual(0, value, "Just here so that 'iterator.Current' doesn't get optimized away.");
                                    },
                                    new PreConditionFailure("blah"));
                                test.AssertThrows(() =>
                                    {
                                        int value = iterator.CurrentIndex;
                                        test.AssertEqual(0, value, "Just here so that 'iterator.CurrentIndex' doesn't get optimized away.");
                                    },
                                    new PreConditionFailure("blah"));
                            }
                        });
                    }

                    IterateTest(new int[0]);
                    IterateTest(new[] { 1 });
                    IterateTest(new[] { 1, 2 });
                    IterateTest(new[] { 1, 2, 3 });
                });

                runner.TestMethod("IndexOf(T)", () =>
                {
                    runner.Test("with empty Indexable", (Test test) =>
                    {
                        Indexable<int> indexable = creator.Invoke(new int[0]);
                        test.AssertNotNull(indexable);
                        test.AssertEqual(0, indexable.Count);

                        Result<int> indexOfResult = indexable.IndexOf(5);
                        test.AssertNotNull(indexOfResult);

                        test.AssertThrows(() => indexOfResult.Await(),
                            new NotFoundException("Could not find 5."));
                    });

                    runner.Test("with non-empty Indexable that doesn't contain the value", (Test test) =>
                    {
                        Indexable<int> indexable = creator.Invoke(new[] { 1, 2, 3 });
                        test.AssertNotNull(indexable);
                        test.AssertEqual(3, indexable.Count);

                        Result<int> indexOfResult = indexable.IndexOf(5);
                        test.AssertNotNull(indexOfResult);

                        test.AssertThrows(() => indexOfResult.Await(),
                            new NotFoundException("Could not find 5."));
                    });

                    runner.Test("with non-empty Indexable that contains the value", (Test test) =>
                    {
                        Indexable<int> indexable = creator.Invoke(new[] { 1, 2, 3 });
                        test.AssertNotNull(indexable);
                        test.AssertEqual(3, indexable.Count);

                        Result<int> indexOfResult = indexable.IndexOf(1);
                        test.AssertNotNull(indexOfResult);

                        test.AssertEqual(0, indexOfResult.Await());
                    });
                });
            });
        }
    }
}

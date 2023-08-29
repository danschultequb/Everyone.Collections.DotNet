using System;

namespace Everyone
{
    public static class IterableTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(Iterable), () =>
            {
                runner.TestMethod("Create(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        Iterable<int> values = Iterable.Create<int>();
                        test.AssertEqual(new int[0], values);
                    });

                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => Iterable.Create((int[])null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one argument", (Test test) =>
                    {
                        Iterable<int> values = Iterable.Create(1);
                        test.AssertEqual(new[] { 1 }, values);
                    });

                    runner.Test("with two arguments", (Test test) =>
                    {
                        Iterable<int> values = Iterable.Create(1, 2);
                        test.AssertEqual(new[] { 1, 2 }, values);
                    });
                });

                IterableTests.Test(runner, Iterable.Create);
            });
        }

        public static void Test(TestRunner runner, Func<int[],Iterable<int>> creator)
        {
            Pre.Condition.AssertNotNull(runner, nameof(runner));
            Pre.Condition.AssertNotNull(creator, nameof(creator));

            runner.TestType($"{nameof(Iterable)}<T>", () =>
            {
                runner.TestMethod("GetEnumerator()", () =>
                {
                    void GetEnumeratorTest(int[] values)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Iterable<int> iterable = creator(values);
                            test.AssertNotNull(iterable);

                            System.Collections.Generic.IEnumerator<int> iterator = iterable.GetEnumerator();
                            test.AssertNotNull(iterator);

                            for (int i = 0; i < values.Length; i++)
                            {
                                test.AssertTrue(iterator.MoveNext());
                                test.AssertEqual(values[i], iterator.Current);
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                test.AssertFalse(iterator.MoveNext());
                                test.AssertThrows(() => { int _ = iterator.Current; },
                                    new PreConditionFailure(
                                        "Expression: this.HasCurrent()",
                                        "Expected: True",
                                        "Actual:   False"));
                            }
                        });
                    }

                    GetEnumeratorTest(new int[0]);
                    GetEnumeratorTest(new[] { 1 });
                    GetEnumeratorTest(new[] { 1, 2 });
                    GetEnumeratorTest(new[] { 1, 2, 3 });
                });

                runner.TestMethod("Iterate()", (Test test) =>
                {
                    void IterateTest(int[] values)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Iterable<int> iterable = creator(values);
                            test.AssertNotNull(iterable);

                            Iterator<int> iterator = iterable.Iterate();
                            test.AssertNotNull(iterator);
                            test.AssertFalse(iterator.HasStarted());
                            test.AssertFalse(iterator.HasCurrent());

                            for (int i = 0; i < values.Length; i++)
                            {
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
                                test.AssertThrows(() => { int _ = iterator.Current; },
                                    new PreConditionFailure(
                                        "Expression: this.HasCurrent()",
                                        "Expected: True",
                                        "Actual:   False"));
                            }
                        });
                    }

                    IterateTest(new int[0]);
                    IterateTest(new[] { 1 });
                    IterateTest(new[] { 1, 2 });
                    IterateTest(new[] { 1, 2, 3 });
                });
            });
        }
    }
}

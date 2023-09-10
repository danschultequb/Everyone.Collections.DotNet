using System;
using System.Collections.Generic;

namespace Everyone
{
    public static class SetTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(Set), () =>
            {
                runner.TestMethod("Create(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        MutableSet<int> set = Set.Create<int>();
                        test.AssertNotNull(set);
                        test.AssertEqual(0, set.Count);
                    });

                    void CreateTest<T>(T[] values, Exception? expectedException = null)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            test.AssertThrows(expectedException, () =>
                            {
                                MutableSet<T> set = Set.Create(values);
                                test.AssertNotNull(set);
                                test.AssertTrue(set.ContainsAll(values));
                            });
                        });
                    }

                    CreateTest(
                        values: (int[])null!,
                        expectedException: new PreConditionFailure(
                            "Expression: values",
                            "Expected: not null",
                            "Actual:   null"));
                    CreateTest(new bool[0]);
                    CreateTest(new[] { 'a', 'b', 'c' });
                    CreateTest(new[] { false, false, false, true, true });
                });

                runner.TestMethod("Create(IEnumerable<T>)", () =>
                {
                    void CreateTest<T>(IEnumerable<T> values, Exception? expectedException = null)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            test.AssertThrows(expectedException, () =>
                            {
                                MutableSet<T> set = Set.Create(values);
                                test.AssertNotNull(set);
                                test.AssertTrue(set.ContainsAll(values));
                            });
                        });
                    }

                    CreateTest(
                        values: (int[])null!,
                        expectedException: new PreConditionFailure(
                            "Expression: values",
                            "Expected: not null",
                            "Actual:   null"));
                    CreateTest(new bool[0]);
                    CreateTest(new[] { 'a', 'b', 'c' });
                    CreateTest("hello");
                    CreateTest(List.Create(1.2, 3.45));
                    CreateTest(Iterable.Create(false, false, false, true, true));
                });

                SetTests.Test(runner, Set.Create);
            });
        }

        public static void Test(TestRunner runner, Func<int[],Set<int>> creator)
        {
            Pre.Condition.AssertNotNull(runner, nameof(runner));
            Pre.Condition.AssertNotNull(creator, nameof(creator));

            runner.TestType(typeof(Set), () =>
            {
                runner.TestMethod("Count", () =>
                {
                    void CountTest(int[] values, int expected)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Set<int> set = creator(values);
                            test.AssertNotNull(set);
                            test.AssertEqual(expected, set.Count);
                        });
                    }

                    CountTest(
                        values: new int[0],
                        expected: 0);
                    CountTest(
                        values: new[] { 1 },
                        expected: 1);
                    CountTest(
                        values: new[] { 1, 1, 1 },
                        expected: 1);
                    CountTest(
                        values: new[] { 1, 2, 3 },
                        expected: 3);
                });

                runner.TestMethod("Contains(T)", () =>
                {
                    void ContainsTest(int[] values, int value, bool expected)
                    {
                        runner.Test($"with {Language.AndList(new object[] { values, value }.Map(runner.ToString))}", (Test test) =>
                        {
                            Set<int> set = creator(values);
                            test.AssertNotNull(set);
                            test.AssertEqual(expected, set.Contains(value));
                        });
                    }

                    ContainsTest(
                        values: new int[0],
                        value: 5,
                        expected: false);
                    ContainsTest(
                        values: new[] { 1, 2, 3 },
                        value: 5,
                        expected: false);
                    ContainsTest(
                        values: new[] { 1, 2, 3 },
                        value: 3,
                        expected: true);
                });
            });
        }
    }
}

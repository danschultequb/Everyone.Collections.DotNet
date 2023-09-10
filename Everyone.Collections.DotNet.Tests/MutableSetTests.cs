using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everyone
{
    public static class MutableSetTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(MutableSet), () =>
            {
                runner.TestMethod("Create(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        MutableSet<int> set = MutableSet.Create<int>();
                        test.AssertNotNull(set);
                        test.AssertEqual(0, set.Count);
                    });

                    void CreateTest<T>(T[] values, Exception? expectedException = null)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            test.AssertThrows(expectedException, () =>
                            {
                                MutableSet<T> set = MutableSet.Create(values);
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
                                MutableSet<T> set = MutableSet.Create(values);
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

                MutableSetTests.Test(runner, MutableSet.Create);
            });
        }

        public static void Test(TestRunner runner, Func<int[], MutableSet<int>> creator)
        {
            Pre.Condition.AssertNotNull(runner, nameof(runner));
            Pre.Condition.AssertNotNull(creator, nameof(creator));

            runner.TestType(typeof(MutableSet), () =>
            {
                SetTests.Test(runner, creator);

                runner.TestMethod("Add(T)", () =>
                {
                    void AddTest(int[] initialValues, int toAdd, int expectedCount)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, toAdd }.Map(runner.ToString))}", (Test test) =>
                        {
                            MutableSet<int> set = creator(initialValues);
                            test.AssertNotNull(set);

                            MutableSet<int> addResult = set.Add(toAdd);
                            test.AssertSame(set, addResult);

                            test.AssertTrue(set.Contains(toAdd));
                            test.AssertEqual(expectedCount, set.Count);
                        });
                    }

                    AddTest(
                        initialValues: new int[0],
                        toAdd: 1,
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1 },
                        toAdd: 1,
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1, 1, 1 },
                        toAdd: 1,
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: 4,
                        expectedCount: 4);
                });

                runner.TestMethod("Add(T,out bool)", () =>
                {
                    void AddTest(int[] initialValues, int toAdd, int expectedCount, bool expectedAdded)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, toAdd }.Map(runner.ToString))}", (Test test) =>
                        {
                            MutableSet<int> set = creator(initialValues);
                            test.AssertNotNull(set);

                            MutableSet<int> addResult = set.Add(toAdd, out bool added);
                            test.AssertSame(set, addResult);
                            test.AssertEqual(expectedAdded, added);

                            test.AssertTrue(set.Contains(toAdd));
                            test.AssertEqual(expectedCount, set.Count);
                        });
                    }

                    AddTest(
                        initialValues: new int[0],
                        toAdd: 1,
                        expectedCount: 1,
                        expectedAdded: true);
                    AddTest(
                        initialValues: new[] { 1 },
                        toAdd: 1,
                        expectedCount: 1,
                        expectedAdded: false);
                    AddTest(
                        initialValues: new[] { 1, 1, 1 },
                        toAdd: 1,
                        expectedCount: 1,
                        expectedAdded: false);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: 4,
                        expectedCount: 4,
                        expectedAdded: true);
                });

                runner.TestMethod("AddAll(T)", () =>
                {
                    void AddTest(int[] initialValues, IEnumerable<int> toAdd, int expectedCount)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, toAdd }.Map(runner.ToString))}", (Test test) =>
                        {
                            MutableSet<int> set = creator(initialValues);
                            test.AssertNotNull(set);

                            MutableSet<int> addResult = set.AddAll(toAdd);
                            test.AssertSame(set, addResult);

                            test.AssertTrue(set.ContainsAll(toAdd));
                            test.AssertEqual(expectedCount, set.Count);
                        });
                    }

                    AddTest(
                        initialValues: new int[0],
                        toAdd: new[] { 1 },
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1 },
                        toAdd: new[] { 1 },
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1, 1, 1 },
                        toAdd: new[] { 1 },
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: new[] { 4 },
                        expectedCount: 4);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: new[] { 1, 2, 3 },
                        expectedCount: 3);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: new[] { 4, 5 },
                        expectedCount: 5);
                });

                runner.TestMethod("AddAll(T)", () =>
                {
                    void AddTest(int[] initialValues, IEnumerable<int> toAdd, int expectedCount)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, toAdd }.Map(runner.ToString))}", (Test test) =>
                        {
                            MutableSet<int> set = creator(initialValues);
                            test.AssertNotNull(set);

                            MutableSet<int> addResult = set.AddAll(toAdd);
                            test.AssertSame(set, addResult);

                            test.AssertTrue(set.ContainsAll(toAdd));
                            test.AssertEqual(expectedCount, set.Count);
                        });
                    }

                    AddTest(
                        initialValues: new int[0],
                        toAdd: new[] { 1 },
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1 },
                        toAdd: new[] { 1 },
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1, 1, 1 },
                        toAdd: new[] { 1 },
                        expectedCount: 1);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: new[] { 4 },
                        expectedCount: 4);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: new[] { 1, 2, 3 },
                        expectedCount: 3);
                    AddTest(
                        initialValues: new[] { 1, 2, 3 },
                        toAdd: new[] { 4, 5 },
                        expectedCount: 5);
                });
            });
        }
    }
}

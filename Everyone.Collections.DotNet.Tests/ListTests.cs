using System;
using System.Collections.Generic;

namespace Everyone
{
    public static class ListTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType("Everyone.List<T>", () =>
            {
                ListTests.Test(runner, List.Create);

                runner.TestMethod("Create(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        List<int> list = List.Create<int>();
                        test.AssertNotNull(list);
                    });

                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => List.Create((int[])null!),
                            new PreConditionFailure(
                                "Expression: initialValues",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one argument", (Test test) =>
                    {
                        List<int> list = List.Create(100);
                        test.AssertEqual(new[] { 100 }, list);
                    });

                    runner.Test("with two arguments", (Test test) =>
                    {
                        List<int> list = List.Create(100, 200);
                        test.AssertEqual(new[] { 100, 200 }, list);
                    });
                });
            });
        }

        public static void Test(TestRunner runner, Func<int[],List<int>> creator)
        {
            Pre.Condition.AssertNotNull(runner, nameof(runner));
            Pre.Condition.AssertNotNull(creator, nameof(creator));

            runner.TestType($"{nameof(List)}<T>", () =>
            {
                MutableIndexableTests.Test(runner, creator);

                runner.TestMethod("IsReadOnly", (Test test) =>
                {
                    List<int> list = creator(new int[0]);
                    test.AssertFalse(list.IsReadOnly);
                });

                runner.TestMethod("Insert(int,T)", () =>
                {
                    void InsertTest(int[] initialValues, int index, int value, Exception? expectedException = null)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, index, value }.Map(runner.ToString))}", (Test test) =>
                        {
                            List<int> list = creator(initialValues);

                            Exception? exception = test.Catch<Exception>(() =>
                            {
                                list.Insert(index, value);

                                for (int i = 0; i < index; i++)
                                {
                                    test.AssertEqual(initialValues[i], list[i]);
                                }

                                test.AssertEqual(value, list[index]);

                                for (int i = index; i < initialValues.Length; i++)
                                {
                                    test.AssertEqual(initialValues[i], list[i + 1]);
                                }
                            });
                            test.AssertEqual(expectedException, exception);
                            if (exception != null)
                            {
                                test.AssertEqual(initialValues, list);
                            }
                        });
                    }

                    InsertTest(
                        initialValues: new int[0],
                        index: -1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: 0",
                            "Actual:   -1"));
                    InsertTest(
                        initialValues: new int[0],
                        index: 0,
                        value: 5);
                    InsertTest(
                        initialValues: new int[0],
                        index: 1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: 0",
                            "Actual:   1"));

                    InsertTest(
                        initialValues: new[] { 0 },
                        index: -1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: between 0 and 1",
                            "Actual:   -1"));
                    InsertTest(
                        initialValues: new[] { 0 },
                        index: 0,
                        value: 5);
                    InsertTest(
                        initialValues: new[] { 0 },
                        index: 1,
                        value: 5);
                    InsertTest(
                        initialValues: new[] { 0 },
                        index: 2,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: between 0 and 1",
                            "Actual:   2"));

                    InsertTest(
                        initialValues: new[] { 0, 1 },
                        index: -1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: between 0 and 2",
                            "Actual:   -1"));
                    InsertTest(
                        initialValues: new[] { 0, 1 },
                        index: 0,
                        value: 5);
                    InsertTest(
                        initialValues: new[] { 0, 1 },
                        index: 1,
                        value: 5);
                    InsertTest(
                        initialValues: new[] { 0, 1 },
                        index: 2,
                        value: 5);
                    InsertTest(
                        initialValues: new[] { 0, 1 },
                        index: 3,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: between 0 and 2",
                            "Actual:   3"));
                });

                runner.TestMethod("Add(T)", () =>
                {
                    void AddTest(int[] initialValues, int value)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, value }.Map(runner.ToString))}", (Test test) =>
                        {
                            List<int> list = creator(initialValues);

                            list.Add(value);

                            for (int i = 0; i < initialValues.Length; i++)
                            {
                                test.AssertEqual(initialValues[i], list[i]);
                            }

                            test.AssertEqual(value, list[initialValues.Length]);
                        });
                    }

                    AddTest(
                        initialValues: new int[0],
                        value: 5);
                    AddTest(
                        initialValues: new[] { 0 },
                        value: 5);
                    AddTest(
                        initialValues: new[] { 0, 1 },
                        value: 5);
                });

                runner.TestMethod("AddAll(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        List<int> list = creator(new int[0]);

                        list.AddAll();

                        test.AssertEqual(new int[0], list);
                    });

                    void AddAllTest(int[] initialValues, int[] values, int[] expectedValues, Exception? expectedException = null)
                    {
                        runner.Test($"with {Language.AndList(new[] { initialValues, values }.Map(runner.ToString))}", (Test test) =>
                        {
                            List<int> list = List.Create(initialValues);

                            Exception? caughtException = test.Catch<Exception>(() => list.AddAll(values));
                            test.AssertEqual(expectedException, caughtException);
                            test.AssertEqual(expectedValues, list);
                        });
                    }

                    AddAllTest(
                        initialValues: new int[0],
                        values: null!,
                        expectedValues: new int[0],
                        expectedException: new PreConditionFailure(
                            "Expression: values",
                            "Expected: not null",
                            "Actual:   null"));
                    AddAllTest(
                        initialValues: new int[0],
                        values: new int[0],
                        expectedValues: new int[0]);
                    AddAllTest(
                        initialValues: new int[0],
                        values: new[] { 1 },
                        expectedValues: new[] { 1 });
                    AddAllTest(
                        initialValues: new int[0],
                        values: new[] { 1, 2 },
                        expectedValues: new[] { 1, 2 });
                    AddAllTest(
                        initialValues: new[] { 1 },
                        values: new int[0],
                        expectedValues: new[] { 1 });
                    AddAllTest(
                        initialValues: new[] { 1 },
                        values: new[] { 2 },
                        expectedValues: new[] { 1, 2 });
                    AddAllTest(
                        initialValues: new[] { 1 },
                        values: new[] { 2, 3 },
                        expectedValues: new[] { 1, 2, 3 });
                });

                runner.TestMethod("AddAll(IEnumerable<T>)", () =>
                {
                    void AddAllTest(int[] initialValues, IEnumerable<int> values, int[] expectedValues, Exception? expectedException = null)
                    {
                        runner.Test($"with {Language.AndList(new[] { initialValues, values }.Map(runner.ToString))}", (Test test) =>
                        {
                            List<int> list = List.Create(initialValues);

                            Exception? caughtException = test.Catch<Exception>(() => list.AddAll(values));
                            test.AssertEqual(expectedException, caughtException);
                            test.AssertEqual(expectedValues, list);
                        });
                    }

                    AddAllTest(
                        initialValues: new int[0],
                        values: null!,
                        expectedValues: new int[0],
                        expectedException: new PreConditionFailure(
                            "Expression: values",
                            "Expected: not null",
                            "Actual:   null"));
                    AddAllTest(
                        initialValues: new int[0],
                        values: new int[0],
                        expectedValues: new int[0]);
                    AddAllTest(
                        initialValues: new int[0],
                        values: new[] { 1 },
                        expectedValues: new[] { 1 });
                    AddAllTest(
                        initialValues: new int[0],
                        values: new[] { 1, 2 },
                        expectedValues: new[] { 1, 2 });
                    AddAllTest(
                        initialValues: new[] { 1 },
                        values: new int[0],
                        expectedValues: new[] { 1 });
                    AddAllTest(
                        initialValues: new[] { 1 },
                        values: new[] { 2 },
                        expectedValues: new[] { 1, 2 });
                    AddAllTest(
                        initialValues: new[] { 1 },
                        values: new[] { 2, 3 },
                        expectedValues: new[] { 1, 2, 3 });
                });
            });
        }
    }
}

using System;

namespace Everyone
{
    public static class MutableIndexableTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(MutableIndexable), () =>
            {
                runner.TestMethod("Create<T>(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        MutableIndexable<int> indexable = MutableIndexable.Create<int>();
                        test.AssertEqual(new int[0], indexable);
                    });

                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => MutableIndexable.Create((int[])null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        MutableIndexable<int> indexable = MutableIndexable.Create(10);
                        test.AssertEqual(new[] { 10 }, indexable);
                    });

                    runner.Test("with two value", (Test test) =>
                    {
                        MutableIndexable<int> indexable = MutableIndexable.Create(10, 20);
                        test.AssertEqual(new[] { 10, 20 }, indexable);
                    });
                });

                MutableIndexableTests.Test(runner, MutableIndexable.Create);
            });
        }

        public static void Test(TestRunner runner, Func<int[],MutableIndexable<int>> creator)
        {
            Pre.Condition.AssertNotNull(runner, nameof(runner));
            Pre.Condition.AssertNotNull(creator, nameof(creator));

            runner.TestType($"{nameof(MutableIndexable)}<T>", () =>
            {
                IndexableTests.Test(runner, creator);

                runner.TestMethod("Set(int,T)", () =>
                {
                    void SetTest(int[] initialValues, int index, int value, Exception? expectedException = null)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, index, value }.Map(runner.ToString))}", (Test test) =>
                        {
                            MutableIndexable<int> indexable = creator(initialValues);

                            Exception? caughtException = test.Catch<Exception>(() => indexable.Set(index, value));
                            test.AssertEqual(expectedException, caughtException);

                            if (expectedException != null)
                            {
                                test.AssertEqual(initialValues, indexable);
                            }
                            else
                            {
                                for (int i = 0; i < index; i++)
                                {
                                    test.AssertEqual(initialValues[i], indexable[i]);
                                }
                                test.AssertEqual(value, indexable[index]);
                                for (int i = index + 1; i < indexable.Count; i++)
                                {
                                    test.AssertEqual(initialValues[i], indexable[i]);
                                }
                            }
                        });
                    }

                    SetTest(
                        initialValues: new int[0],
                        index: -1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: this",
                            "Expected: not null and not empty",
                            "Actual:   []"));
                    SetTest(
                        initialValues: new int[0],
                        index: 0,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: this",
                            "Expected: not null and not empty",
                            "Actual:   []"));
                    SetTest(
                        initialValues: new int[0],
                        index: 1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: this",
                            "Expected: not null and not empty",
                            "Actual:   []"));
                    SetTest(
                        initialValues: new[] { 1 },
                        index: -1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: 0",
                            "Actual:   -1"));
                    SetTest(
                        initialValues: new[] { 1 },
                        index: 0,
                        value: 5);
                    SetTest(
                        initialValues: new[] { 1 },
                        index: 1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: 0",
                            "Actual:   1"));
                });

                runner.TestMethod("this[int]", () =>
                {
                    void IndexOperatorSetTest(int[] initialValues, int index, int value, Exception? expectedException = null)
                    {
                        runner.Test($"with {Language.AndList(new object[] { initialValues, index, value }.Map(runner.ToString))}", (Test test) =>
                        {
                            MutableIndexable<int> indexable = creator(initialValues);

                            Exception? caughtException = test.Catch<Exception>(() => indexable[index] = value);
                            test.AssertEqual(expectedException, caughtException);

                            if (expectedException != null)
                            {
                                test.AssertEqual(initialValues, indexable);
                            }
                            else
                            {
                                for (int i = 0; i < index; i++)
                                {
                                    test.AssertEqual(initialValues[i], indexable[i]);
                                }
                                test.AssertEqual(value, indexable[index]);
                                for (int i = index + 1; i < indexable.Count; i++)
                                {
                                    test.AssertEqual(initialValues[i], indexable[i]);
                                }
                            }
                        });
                    }

                    IndexOperatorSetTest(
                        initialValues: new int[0],
                        index: -1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: this",
                            "Expected: not null and not empty",
                            "Actual:   []"));
                    IndexOperatorSetTest(
                        initialValues: new int[0],
                        index: 0,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: this",
                            "Expected: not null and not empty",
                            "Actual:   []"));
                    IndexOperatorSetTest(
                        initialValues: new int[0],
                        index: 1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: this",
                            "Expected: not null and not empty",
                            "Actual:   []"));
                    IndexOperatorSetTest(
                        initialValues: new[] { 1 },
                        index: -1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: 0",
                            "Actual:   -1"));
                    IndexOperatorSetTest(
                        initialValues: new[] { 1 },
                        index: 0,
                        value: 5);
                    IndexOperatorSetTest(
                        initialValues: new[] { 1 },
                        index: 1,
                        value: 5,
                        expectedException: new PreConditionFailure(
                            "Expression: index",
                            "Expected: 0",
                            "Actual:   1"));
                });
            });
        }
    }
}

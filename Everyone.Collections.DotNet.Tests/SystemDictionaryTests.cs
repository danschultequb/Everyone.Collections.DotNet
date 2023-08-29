using System;
using System.Collections.Generic;
using System.Linq;

namespace Everyone
{
    public static class SystemDictionaryTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(SystemDictionary), () =>
            {
                runner.TestMethod("Create(params (TKey,TValue))", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        SystemDictionary<int, bool> map = SystemDictionary.Create<int, bool>();
                        test.AssertNotNull(map);
                        test.AssertEqual(0, map.Count);
                    });

                    void CreateTest<TKey, TValue>((TKey, TValue)[] initialValues, Exception? expectedException = null) where TKey : notnull
                    {
                        runner.Test($"with {runner.ToString(initialValues)}", (Test test) =>
                        {
                            test.AssertThrows(expectedException, () =>
                            {
                                SystemDictionary<TKey, TValue> map = SystemDictionary.Create(initialValues);
                                test.AssertNotNull(map);
                                test.AssertEqual(initialValues.Length, map.Count);
                                test.AssertEqual(initialValues, map.IterateTuples());
                            });
                        });
                    }

                    CreateTest(
                        initialValues: ((int, bool)[])null!,
                        expectedException: new PreConditionFailure(
                            "Expression: values",
                            "Expected: not null",
                            "Actual:   null"));
                    CreateTest(
                        initialValues: new[] { (1, true) });
                    CreateTest(
                        initialValues: new[] { (1, true), (2, false) });
                    CreateTest(
                        initialValues: new[] { (1, true), (2, false), (3, true) });
                });

                runner.TestMethod("Create(IEnumerable<Tuple<TKey,TValue>>)", () =>
                {
                    void CreateTest<TKey, TValue>(IEnumerable<Tuple<TKey, TValue>> initialValues, Exception? expectedException = null) where TKey : notnull
                    {
                        runner.Test($"with {runner.ToString(initialValues)}", (Test test) =>
                        {
                            test.AssertThrows(expectedException, () =>
                            {
                                SystemDictionary<TKey, TValue> map = SystemDictionary.Create(initialValues);
                                test.AssertNotNull(map);
                                test.AssertEqual(initialValues.Count(), map.Count);
                                test.AssertEqual(initialValues, map.IterateTuples());
                            });
                        });
                    }

                    CreateTest(
                        initialValues: (IEnumerable<Tuple<int, bool>>)null!,
                        expectedException: new PreConditionFailure(
                            "Expression: values",
                            "Expected: not null",
                            "Actual:   null"));
                    CreateTest(
                        initialValues: new[] { Tuple.Create(1, true) });
                    CreateTest(
                        initialValues: new[] { Tuple.Create(1, true), Tuple.Create(2, false) });
                    CreateTest(
                        initialValues: new[] { Tuple.Create(1, true), Tuple.Create(2, false), Tuple.Create(3, true) });
                });

                runner.TestMethod("Create(IEnumerable<KeyValuePair<TKey,TValue>>)", () =>
                {
                    void CreateTest<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> initialValues, Exception? expectedException = null) where TKey : notnull
                    {
                        runner.Test($"with {runner.ToString(initialValues)}", (Test test) =>
                        {
                            test.AssertThrows(expectedException, () =>
                            {
                                SystemDictionary<TKey, TValue> map = SystemDictionary.Create(initialValues);
                                test.AssertNotNull(map);
                                test.AssertEqual(initialValues.Count(), map.Count);
                                test.AssertEqual(initialValues, map);
                            });
                        });
                    }

                    CreateTest(
                        initialValues: (IEnumerable<KeyValuePair<int, bool>>)null!,
                        expectedException: new PreConditionFailure(
                            "Expression: values",
                            "Expected: not null",
                            "Actual:   null"));
                    CreateTest(
                        initialValues: new[] { KeyValuePair.Create(1, true) });
                    CreateTest(
                        initialValues: new[] { KeyValuePair.Create(1, true), KeyValuePair.Create(2, false) });
                    CreateTest(
                        initialValues: new[] { KeyValuePair.Create(1, true), KeyValuePair.Create(2, false), KeyValuePair.Create(3, true) });
                });
            });
        }
    }
}

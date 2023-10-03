using System;
using System.Collections.Generic;
using System.Linq;

namespace Everyone
{
    public static class MapTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(Map), () =>
            {
                runner.TestMethod("Create(params (TKey,TValue))", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        MutableMap<int, bool> map = Map.Create<int,bool>();
                        test.AssertNotNull(map);
                        test.AssertEqual(0, map.Count);
                    });

                    void CreateTest<TKey, TValue>((TKey, TValue)[] initialValues, Exception? expectedException = null) where TKey : notnull
                    {
                        runner.Test($"with {runner.ToString(initialValues)}", (Test test) =>
                        {
                            test.AssertThrows(expectedException, () =>
                            {
                                MutableMap<TKey, TValue> map = Map.Create(initialValues);
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
                                MutableMap<TKey, TValue> map = Map.Create(initialValues);
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
                                MutableMap<TKey, TValue> map = Map.Create(initialValues);
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

                MapTests.Test(runner, Map.Create);
            });
        }

        public static void Test(TestRunner runner, Func<(int, char)[],Map<int,char>> creator)
        {
            Pre.Condition.AssertNotNull(runner, nameof(runner));
            Pre.Condition.AssertNotNull(creator, nameof(creator));

            runner.TestType($"{nameof(Map)}<TKey,TValue>", () =>
            {
                runner.TestMethod("Count", () =>
                {
                    void CountTest((int, char)[] values, int? expected = null)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Map<int, char> map = creator(values);
                            test.AssertEqual(expected ?? values.Length, map.Count);
                        });
                    }

                    CountTest(
                        values: new (int, char)[0]);
                    CountTest(
                        values: new[] { (1, 'a') });
                    CountTest(
                        values: new[] { (1, 'a'), (2, 'b') });
                    CountTest(
                        values: new[] { (1, 'a'), (1, 'b') },
                        expected: 1);
                });

                runner.TestMethod("Get(TKey)", () =>
                {
                    runner.Test("with not found key", (Test test) =>
                    {
                        Map<int, char> map = creator(new (int, char)[0]);
                        test.AssertThrows(() => map.Get(5).Await(),
                            new NotFoundException("Could not find the key: 5"));
                    });

                    void CountTest((int, char)[] values, int? expected = null)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Map<int, char> map = creator(values);
                            foreach ((int,char) entry in values)
                            {
                                test.AssertEqual(entry.Item2, map.Get(entry.Item1).Await());
                            }
                        });
                    }

                    CountTest(
                        values: new[] { (1, 'a') });
                    CountTest(
                        values: new[] { (1, 'a'), (2, 'b') });
                });

                runner.TestMethod("this(TKey)", () =>
                {
                    runner.Test("with not found key", (Test test) =>
                    {
                        Map<int, char> map = creator(new (int, char)[0]);
                        test.AssertThrows(() => { char _ = map[5]; },
                            new NotFoundException("Could not find the key: 5"));
                    });

                    void CountTest((int, char)[] values, int? expected = null)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Map<int, char> map = creator(values);
                            foreach ((int, char) entry in values)
                            {
                                test.AssertEqual(entry.Item2, map[entry.Item1]);
                            }
                        });
                    }

                    CountTest(
                        values: new[] { (1, 'a') });
                    CountTest(
                        values: new[] { (1, 'a'), (2, 'b') });
                });

                runner.TestMethod("TryGetValue(TKey,out TValue)", () =>
                {
                    runner.Test("with not found key", (Test test) =>
                    {
                        Map<int, char> map = creator(new (int, char)[0]);
                        test.AssertFalse(map.TryGetValue(5, out char value));
                        test.AssertEqual(default(char), value);
                    });

                    void CountTest((int, char)[] values, int? expected = null)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            Map<int, char> map = creator(values);
                            foreach ((int, char) entry in values)
                            {
                                test.AssertTrue(map.TryGetValue(entry.Item1, out char value));
                                test.AssertEqual(entry.Item2, value);
                            }
                        });
                    }

                    CountTest(
                        values: new[] { (1, 'a') });
                    CountTest(
                        values: new[] { (1, 'a'), (2, 'b') });
                });
            });
        }
    }
}

using System.Collections.Generic;

namespace Everyone
{
    public static class SystemListTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(SystemList), () =>
            {
                ListTests.Test(runner, SystemList.Create);

                runner.TestMethod("Create(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        SystemList<int> list = SystemList.Create<int>();
                        test.AssertNotNull(list);
                        test.AssertEqual(0, list.Count);
                        test.AssertFalse(list.Any());
                    });

                    runner.Test("with null array", (Test test) =>
                    {
                        test.AssertThrows(() => SystemList.Create((int[])null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one argument", (Test test) =>
                    {
                        SystemList<int> list = SystemList.Create(1);
                        test.AssertNotNull(list);
                        test.AssertEqual(1, list.Count);
                        test.AssertTrue(list.Any());
                        test.AssertEqual(new[] { 1 }, list);
                    });

                    runner.Test("with two arguments", (Test test) =>
                    {
                        SystemList<int> list = SystemList.Create(1, 2);
                        test.AssertNotNull(list);
                        test.AssertEqual(2, list.Count);
                        test.AssertTrue(list.Any());
                        test.AssertEqual(new[] { 1, 2 }, list);
                    });
                });

                runner.TestMethod("Create(IEnumerable<T>)", () =>
                {
                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => SystemList.Create((IEnumerable<int>)null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        SystemList<int> list = SystemList.Create<int>(Iterable.Create(1));
                        test.AssertNotNull(list);
                        test.AssertEqual(1, list.Count);
                        test.AssertTrue(list.Any());
                        test.AssertEqual(new[] { 1 }, list);
                    });

                    runner.Test("with two arguments", (Test test) =>
                    {
                        SystemList<int> list = SystemList.Create<int>(Iterable.Create(1, 2));
                        test.AssertNotNull(list);
                        test.AssertEqual(2, list.Count);
                        test.AssertTrue(list.Any());
                        test.AssertEqual(new[] { 1, 2 }, list);
                    });
                });
            });
        }
    }
}

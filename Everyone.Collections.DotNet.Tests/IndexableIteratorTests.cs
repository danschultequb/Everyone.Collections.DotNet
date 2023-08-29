namespace Everyone
{
    public static class IndexableIteratorTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType($"{nameof(IndexableIterator)}<T>", () =>
            {
                runner.TestMethod("Create(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        IndexableIterator<int> iterator = IndexableIterator.Create<int>();
                        test.AssertNotNull(iterator);
                        test.AssertFalse(iterator.HasCurrent());
                        test.AssertFalse(iterator.HasStarted());
                        test.AssertThrows(() => { int _ = iterator.Current; },
                            new PreConditionFailure(
                                "Expression: this.HasCurrent()",
                                "Expected: True",
                                "Actual:   False"));
                        test.AssertThrows(() => { int _ = iterator.CurrentIndex; },
                            new PreConditionFailure(
                                "Expression: this.HasCurrent()",
                                "Expected: True",
                                "Actual:   False"));
                    });

                    void CreateTest(int[] values)
                    {
                        runner.Test($"with {runner.ToString(values)}", (Test test) =>
                        {
                            IndexableIterator<int> iterator = IndexableIterator.Create<int>(values);
                            test.AssertNotNull(iterator);
                            test.AssertFalse(iterator.HasCurrent());
                            test.AssertFalse(iterator.HasStarted());
                            test.AssertThrows(() => { int _ = iterator.Current; },
                                new PreConditionFailure(
                                    "Expression: this.HasCurrent()",
                                    "Expected: True",
                                    "Actual:   False"));
                            test.AssertThrows(() => { int _ = iterator.CurrentIndex; },
                                new PreConditionFailure(
                                    "Expression: this.HasCurrent()",
                                    "Expected: True",
                                    "Actual:   False"));

                            for (int i = 0; i < values.Length; i++)
                            {
                                test.AssertTrue(iterator.Next());
                                test.AssertTrue(iterator.HasCurrent());
                                test.AssertTrue(iterator.HasStarted());
                                test.AssertEqual(values[i], iterator.Current);
                                test.AssertEqual(i, iterator.CurrentIndex);
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
                                test.AssertThrows(() => { int _ = iterator.CurrentIndex; },
                                    new PreConditionFailure(
                                        "Expression: this.HasCurrent()",
                                        "Expected: True",
                                        "Actual:   False"));
                            }
                        });
                    }

                    CreateTest(new int[0]);
                    CreateTest(new[] { 1 });
                    CreateTest(new[] { 1, 2 });
                    CreateTest(new[] { 100, 200, 300 });
                });
            });
        }
    }
}

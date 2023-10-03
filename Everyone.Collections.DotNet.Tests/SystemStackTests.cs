using System.Collections.Generic;

namespace Everyone
{
    public static class SystemStackTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(SystemStack), () =>
            {
                runner.TestMethod("Create<T>(params T[])", () =>
                {
                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => SystemStack.Create((bool[])null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with no arguments", (Test test) =>
                    {
                        SystemStack<bool> stack = SystemStack.Create<bool>();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);
                    });

                    runner.Test("with one argument", (Test test) =>
                    {
                        SystemStack<bool> stack = SystemStack.Create(false);
                        test.AssertNotNull(stack);
                        test.AssertEqual(1, stack.Count);
                        test.AssertEqual(false, stack.Peek().Await());
                    });

                    runner.Test("with two arguments", (Test test) =>
                    {
                        SystemStack<bool> stack = SystemStack.Create(false, true);
                        test.AssertNotNull(stack);
                        test.AssertEqual(2, stack.Count);
                        test.AssertEqual(true, stack.Peek().Await());
                    });
                });

                runner.TestMethod("Create<T>(IEnumerable<T>)", () =>
                {
                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => SystemStack.Create((IEnumerable<bool>)null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one argument", (Test test) =>
                    {
                        SystemStack<bool> stack = SystemStack.Create<bool>(List.Create(false));
                        test.AssertNotNull(stack);
                        test.AssertEqual(1, stack.Count);
                        test.AssertEqual(false, stack.Peek().Await());
                    });

                    runner.Test("with two arguments", (Test test) =>
                    {
                        SystemStack<bool> stack = SystemStack.Create<bool>(List.Create(false, true));
                        test.AssertNotNull(stack);
                        test.AssertEqual(2, stack.Count);
                        test.AssertEqual(true, stack.Peek().Await());
                    });
                });

                StackTests.Test(runner, () => SystemStack.Create<int>());
            });
        }
    }
}

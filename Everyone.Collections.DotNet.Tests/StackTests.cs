using System;
using System.Collections.Generic;

namespace Everyone
{
    public static class StackTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType(typeof(Stack), () =>
            {
                runner.TestMethod("Create(params T[])", () =>
                {
                    runner.Test("with no arguments", (Test test) =>
                    {
                        Stack<int> stack = Stack.Create<int>();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);
                        test.AssertFalse(stack.Any());
                    });

                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => Stack.Create<int>(null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        Stack<int> stack = Stack.Create(1);
                        test.AssertNotNull(stack);
                        test.AssertEqual(1, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(1, stack.Peek().Await());
                        test.AssertEqual(1, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(1, stack.Remove().Await());
                        test.AssertEqual(0, stack.Count);
                        test.AssertFalse(stack.Any());

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertThrows(() => stack.Remove().Await(),
                                new EmptyException());
                            test.AssertEqual(0, stack.Count);
                            test.AssertFalse(stack.Any());
                        }
                    });

                    runner.Test("with two values", (Test test) =>
                    {
                        Stack<int> stack = Stack.Create(10, 20);
                        test.AssertNotNull(stack);
                        test.AssertEqual(2, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(20, stack.Peek().Await());
                        test.AssertEqual(2, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(20, stack.Remove().Await());
                        test.AssertEqual(1, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(10, stack.Remove().Await());
                        test.AssertEqual(0, stack.Count);
                        test.AssertFalse(stack.Any());

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertThrows(() => stack.Remove().Await(),
                                new EmptyException());
                            test.AssertEqual(0, stack.Count);
                            test.AssertFalse(stack.Any());
                        }
                    });
                });

                runner.TestMethod("Create(IEnumerable<T>)", () =>
                {
                    runner.Test("with null", (Test test) =>
                    {
                        test.AssertThrows(() => Stack.Create((IEnumerable<int>)null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        Stack<int> stack = Stack.Create<int>(List.Create(1));
                        test.AssertNotNull(stack);
                        test.AssertEqual(1, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(1, stack.Peek().Await());
                        test.AssertEqual(1, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(1, stack.Remove().Await());
                        test.AssertEqual(0, stack.Count);
                        test.AssertFalse(stack.Any());

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertThrows(() => stack.Remove().Await(),
                                new EmptyException());
                            test.AssertEqual(0, stack.Count);
                            test.AssertFalse(stack.Any());
                        }
                    });

                    runner.Test("with two values", (Test test) =>
                    {
                        Stack<int> stack = Stack.Create<int>(List.Create(10, 20));
                        test.AssertNotNull(stack);
                        test.AssertEqual(2, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(20, stack.Peek().Await());
                        test.AssertEqual(2, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(20, stack.Remove().Await());
                        test.AssertEqual(1, stack.Count);
                        test.AssertTrue(stack.Any());

                        test.AssertEqual(10, stack.Remove().Await());
                        test.AssertEqual(0, stack.Count);
                        test.AssertFalse(stack.Any());

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertThrows(() => stack.Remove().Await(),
                                new EmptyException());
                            test.AssertEqual(0, stack.Count);
                            test.AssertFalse(stack.Any());
                        }
                    });
                });

                StackTests.Test(runner, () => Stack.Create<int>());
            });
        }

        public static void Test(TestRunner runner, Func<Stack<int>> creator)
        { 
            runner.TestType("Stack<T>", () =>
            {
                runner.TestMethod("Add(T)", (Test test) =>
                {
                    Stack<int> stack = creator.Invoke();
                    test.AssertNotNull(stack);
                    test.AssertEqual(0, stack.Count);

                    Stack<int> addResult10 = stack.Add(10);
                    test.AssertSame(stack, addResult10);
                    test.AssertEqual(1, stack.Count);
                    test.AssertEqual(10, stack.Peek().Await());

                    Stack<int> addResult20 = stack.Add(20);
                    test.AssertSame(stack, addResult10);
                    test.AssertEqual(2, stack.Count);
                    test.AssertEqual(20, stack.Peek().Await());
                });

                runner.TestMethod("AddAll(params T[])", () =>
                {
                    runner.Test("with null", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        test.AssertThrows(() => stack.AddAll((int[])null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                        test.AssertEqual(0, stack.Count);
                    });

                    runner.Test("with no arguments", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Stack<int> addAllResult = stack.AddAll();
                        test.AssertSame(stack, addAllResult);
                        test.AssertEqual(0, stack.Count);
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Stack<int> addAllResult = stack.AddAll(50);
                        test.AssertSame(stack, addAllResult);
                        test.AssertEqual(1, stack.Count);
                        test.AssertEqual(50, stack.Peek().Await());
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Stack<int> addAllResult = stack.AddAll(50, 100);
                        test.AssertSame(stack, addAllResult);
                        test.AssertEqual(2, stack.Count);
                        test.AssertEqual(100, stack.Peek().Await());
                    });
                });

                runner.TestMethod("AddAll(IEnumerable<T>)", () =>
                {
                    runner.Test("with null", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        test.AssertThrows(() => stack.AddAll((IEnumerable<int>)null!),
                            new PreConditionFailure(
                                "Expression: values",
                                "Expected: not null",
                                "Actual:   null"));
                        test.AssertEqual(0, stack.Count);
                    });

                    runner.Test("with no values", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Stack<int> addAllResult = stack.AddAll(List.Create<int>());
                        test.AssertSame(stack, addAllResult);
                        test.AssertEqual(0, stack.Count);
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Stack<int> addAllResult = stack.AddAll(List.Create(50));
                        test.AssertSame(stack, addAllResult);
                        test.AssertEqual(1, stack.Count);
                        test.AssertEqual(50, stack.Peek().Await());
                    });

                    runner.Test("with one value", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Stack<int> addAllResult = stack.AddAll(List.Create(50, 100));
                        test.AssertSame(stack, addAllResult);
                        test.AssertEqual(2, stack.Count);
                        test.AssertEqual(100, stack.Peek().Await());
                    });
                });

                runner.TestMethod("Peek()", () =>
                {
                    runner.Test("when empty", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Result<int> peekResult = stack.Peek();
                        test.AssertNotNull(peekResult);
                        test.AssertEqual(0, stack.Count);

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertThrows(() => peekResult.Await(),
                                new EmptyException());
                            test.AssertEqual(0, stack.Count);
                        }
                    });

                    runner.Test("when not empty", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke().Add(10);
                        test.AssertNotNull(stack);
                        test.AssertEqual(1, stack.Count);

                        Result<int> peekResult = stack.Peek();
                        test.AssertNotNull(peekResult);
                        test.AssertEqual(1, stack.Count);

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertEqual(10, peekResult.Await());
                            test.AssertEqual(1, stack.Count);
                        }
                    });
                });

                runner.TestMethod("Remove()", () =>
                {
                    runner.Test("when empty", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertNotNull(stack);
                        test.AssertEqual(0, stack.Count);

                        Result<int> removeResult = stack.Remove();
                        test.AssertNotNull(removeResult);
                        test.AssertEqual(0, stack.Count);

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertThrows(() => removeResult.Await(),
                                new EmptyException());
                            test.AssertEqual(0, stack.Count);
                        }
                    });

                    runner.Test("when not empty", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke().Add(10);
                        test.AssertNotNull(stack);
                        test.AssertEqual(1, stack.Count);

                        Result<int> peekResult = stack.Remove();
                        test.AssertNotNull(peekResult);
                        test.AssertEqual(1, stack.Count);

                        for (int i = 0; i < 2; i++)
                        {
                            test.AssertEqual(10, peekResult.Await());
                            test.AssertEqual(0, stack.Count);
                        }
                    });
                });

                runner.TestMethod("Contains(T)", () =>
                {
                    runner.Test("with not contained value", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke();
                        test.AssertFalse(stack.Contains(5));
                    });

                    runner.Test("with contained value", (Test test) =>
                    {
                        Stack<int> stack = creator.Invoke().Add(10);
                        test.AssertTrue(stack.Contains(10));
                    });
                });
            });
        }
    }
}

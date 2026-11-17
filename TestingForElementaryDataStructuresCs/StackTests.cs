using ElementaryDataStructuresCs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingForElementaryDataStructuresCs;

[TestClass]
public class StackTests
{
    [TestMethod]
    public void PushAndPop_FollowLastInFirstOutOrder()
    {
        var stack = new StackViaTwoQueues<int>();

        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.AreEqual(3, stack.Pop());
        Assert.AreEqual(2, stack.Pop());
        Assert.AreEqual(1, stack.Pop());
    }

    [TestMethod]
    public void Peek_DoesNotRemoveTopOrBreakFollowingOperations()
    {
        var stack = new StackViaTwoQueues<int>();
        stack.Push(10);
        stack.Push(20);

        Assert.AreEqual(20, stack.Peek());
        Assert.AreEqual(20, stack.Peek());
        stack.Push(30);

        Assert.AreEqual(30, stack.Pop());
        Assert.AreEqual(20, stack.Pop());
        Assert.AreEqual(10, stack.Pop());
    }

    [TestMethod]
    public void PeekAndPop_OnEmptyStack_Throw()
    {
        var stack = new StackViaTwoQueues<int>();

        Assert.ThrowsException<InvalidOperationException>(() => stack.Peek());
        Assert.ThrowsException<Exception>(() => stack.Pop());
    }

    [TestMethod]
    public void PushAfterRemovingAllItems_Works()
    {
        var stack = new StackViaTwoQueues<string>();
        stack.Push("first");
        Assert.AreEqual("first", stack.Pop());

        stack.Push("second");

        Assert.AreEqual("second", stack.Peek());
        Assert.AreEqual("second", stack.Pop());
    }
}

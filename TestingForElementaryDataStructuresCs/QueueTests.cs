using ElementaryDataStructuresCs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingForElementaryDataStructuresCs;

[TestClass]
public class QueueTests
{
    [TestMethod]
    public void EnqueueAndDequeue_FollowFirstInFirstOutOrder()
    {
        var queue = new QueueViaTwoStacks<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        Assert.AreEqual(1, queue.Dequeue());
        Assert.AreEqual(2, queue.Dequeue());
        Assert.AreEqual(3, queue.Dequeue());
    }

    [TestMethod]
    public void Peek_DoesNotRemoveFront()
    {
        var queue = new QueueViaTwoStacks<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);

        Assert.AreEqual(1, queue.Peek());
        Assert.AreEqual(1, queue.Peek());
        Assert.AreEqual(1, queue.Dequeue());
        Assert.AreEqual(2, queue.Dequeue());
    }

    [TestMethod]
    public void EnqueueAfterDequeue_PreservesFifoOrder()
    {
        var queue = new QueueViaTwoStacks<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        Assert.AreEqual(1, queue.Dequeue());

        queue.Enqueue(3);

        Assert.AreEqual(2, queue.Dequeue());
        Assert.AreEqual(3, queue.Dequeue());
    }

    [TestMethod]
    public void DequeueAndPeek_OnEmptyQueue_Throw()
    {
        var queue = new QueueViaTwoStacks<int>();

        Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
        Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
    }
}

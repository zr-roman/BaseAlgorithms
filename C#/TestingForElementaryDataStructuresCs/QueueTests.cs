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

    [TestMethod]
    public void EnqueueNullAndPeekAndDequeue_PreserveNullValue()
    {
        var queue = new QueueViaTwoStacks<string>();
        queue.Enqueue(null);
        queue.Enqueue("next");

        Assert.IsNull(queue.Peek());
        Assert.IsNull(queue.Dequeue());
        Assert.AreEqual("next", queue.Peek());
        Assert.AreEqual("next", queue.Dequeue());
    }

    [TestMethod]
    public void MixedOperations_MatchStandardQueue()
    {
        var actual = new QueueViaTwoStacks<int>();
        var expected = new Queue<int>();

        for (int i = 0; i < 200; i++)
        {
            actual.Enqueue(i);
            expected.Enqueue(i);

            if (i % 3 == 0)
            {
                Assert.AreEqual(expected.Peek(), actual.Peek(), $"front at step {i}");
            }

            if (i % 4 == 0)
            {
                Assert.AreEqual(expected.Dequeue(), actual.Dequeue(), $"dequeue at step {i}");
            }
        }

        while (expected.Count > 0)
        {
            Assert.AreEqual(expected.Dequeue(), actual.Dequeue());
        }
    }
}

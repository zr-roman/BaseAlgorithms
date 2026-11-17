using GraphsCs;

namespace TestingForGraphsCs;

[TestClass]
public class MinHeapTests
{
    [TestMethod]
    public void Constructor_BuildsMinHeapAndExtractsItemsInAscendingOrder()
    {
        var heap = new MinHeap<int>([7, 2, 9, 1, 5, 3, 8, 4, 6]);

        CollectionAssert.AreEqual(
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            ExtractAll(heap));
    }

    [TestMethod]
    public void ExtractMin_WithTwoItemsLeft_ReturnsEachItemOnce()
    {
        var heap = new MinHeap<int>([1, 2, 3]);

        Assert.AreEqual(1, heap.ExtractMin());
        Assert.AreEqual(2, heap.GetSize());
        Assert.AreEqual(2, heap.ExtractMin());
        Assert.AreEqual(1, heap.GetSize());
        Assert.AreEqual(3, heap.ExtractMin());
        Assert.AreEqual(0, heap.GetSize());
    }

    [TestMethod]
    public void EmptyHeap_HasSizeZero()
    {
        var heap = new MinHeap<int>(Array.Empty<int>());

        Assert.AreEqual(0, heap.GetSize());
    }

    [TestMethod]
    public void ExtractMin_FromSingleItemHeap_LeavesItEmpty()
    {
        var heap = new MinHeap<int>(new[] { 42 });

        Assert.AreEqual(42, heap.ExtractMin());
        Assert.AreEqual(0, heap.GetSize());
    }

    [TestMethod]
    public void DecreaseKey_BubblesItemToRoot()
    {
        var first = new MutablePriority(10);
        var second = new MutablePriority(20);
        var decreased = new MutablePriority(30);
        var heap = new MinHeap<MutablePriority>(new[] { first, second, decreased });

        decreased.Priority = 5;
        heap.DecreaseKey(decreased);

        Assert.AreSame(decreased, heap.ExtractMin());
        Assert.AreEqual(2, heap.GetSize());
    }

    [TestMethod]
    public void DecreaseKey_RepeatedAfterSwaps_KeepsIndexMapConsistent()
    {
        var items = new[]
        {
            new MutablePriority(30),
            new MutablePriority(10),
            new MutablePriority(40),
            new MutablePriority(20),
            new MutablePriority(50),
            new MutablePriority(60),
            new MutablePriority(70)
        };
        var heap = new MinHeap<MutablePriority>(items);

        items[6].Priority = 5;
        heap.DecreaseKey(items[6]);
        items[5].Priority = 8;
        heap.DecreaseKey(items[5]);
        items[4].Priority = 3;
        heap.DecreaseKey(items[4]);

        var expected = items.OrderBy(item => item.Priority).Select(item => item.Priority).ToArray();
        var actual = ExtractAll(heap).Select(item => item.Priority).ToArray();

        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void EqualPriorities_AreAllExtractedExactlyOnce()
    {
        var items = Enumerable.Range(0, 8)
            .Select(_ => new MutablePriority(4))
            .ToArray();
        var heap = new MinHeap<MutablePriority>(items);

        var extracted = ExtractAll(heap);

        Assert.AreEqual(items.Length, extracted.Count);
        Assert.IsTrue(extracted.All(item => item.Priority == 4));
        CollectionAssert.AreEquivalent(items, extracted);
    }

    [TestMethod]
    public void DecreaseKey_OnSingleItemHeap_StillExtractsUpdatedItem()
    {
        var item = new MutablePriority(100);
        var heap = new MinHeap<MutablePriority>(new[] { item });

        item.Priority = -1;
        heap.DecreaseKey(item);

        Assert.AreSame(item, heap.ExtractMin());
        Assert.AreEqual(0, heap.GetSize());
    }

    private static List<int> ExtractAll(MinHeap<int> heap)
    {
        var values = new List<int>();
        while (heap.GetSize() > 0)
        {
            values.Add(heap.ExtractMin());
        }

        return values;
    }

    private static List<T> ExtractAll<T>(MinHeap<T> heap) where T : IComparable<T>
    {
        var values = new List<T>();
        while (heap.GetSize() > 0)
        {
            values.Add(heap.ExtractMin());
        }

        return values;
    }

    private sealed class MutablePriority(int priority) : IComparable<MutablePriority>
    {
        public int Priority { get; set; } = priority;

        public int CompareTo(MutablePriority? other) =>
            other is null ? 1 : Priority.CompareTo(other.Priority);
    }
}

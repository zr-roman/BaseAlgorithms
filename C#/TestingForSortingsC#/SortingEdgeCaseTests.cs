using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing;

[TestClass]
public class SortingEdgeCaseTests
{
    private delegate void IntSort(int[] values, Order order);
    private delegate void StringSort(string[] values, Order order);

    private static readonly (string Name, IntSort Sort)[] ComparisonSorts =
    {
        ("BubbleSort", (values, order) => Lib.BubbleSort(values, order)),
        ("CombSort", (values, order) => Lib.CombSort(values, order)),
        ("Heapsort", (values, order) => Lib.Heapsort(values, order)),
        ("InsertionSort", (values, order) => Lib.InsertionSort(values, order)),
        ("IterativeMergeSort", (values, order) => Lib.IterativeMergeSort(values, order)),
        ("MergeSort", (values, order) => Lib.MergeSort(values, order)),
        ("SelectionSort", (values, order) => Lib.SelectionSort(values, order)),
        ("ShellSort", (values, order) => Lib.ShellSort(values, order)),
    };

    private static readonly (string Name, StringSort Sort)[] StringComparisonSorts =
    {
        ("BubbleSort", (values, order) => Lib.BubbleSort(values, order)),
        ("CombSort", (values, order) => Lib.CombSort(values, order)),
        ("Heapsort", (values, order) => Lib.Heapsort(values, order)),
        ("InsertionSort", (values, order) => Lib.InsertionSort(values, order)),
        ("IterativeMergeSort", (values, order) => Lib.IterativeMergeSort(values, order)),
        ("MergeSort", (values, order) => Lib.MergeSort(values, order)),
        ("QuickSort", (values, order) => Lib.QuickSort(values, order, Pivot.RANDOM)),
        ("SelectionSort", (values, order) => Lib.SelectionSort(values, order)),
        ("ShellSort", (values, order) => Lib.ShellSort(values, order)),
    };

    [TestMethod]
    public void ComparisonSorts_SortDuplicatesAndIntegerBoundsInBothOrders()
    {
        var source = new[] { int.MaxValue, 0, int.MinValue, -1, 42, 42, int.MaxValue, int.MinValue };

        AssertAllComparisonSorts(source, Order.ASC);
        AssertAllComparisonSorts(source, Order.DESC);
    }

    [TestMethod]
    public void ComparisonSorts_SortTwoElementAndOddLengthArrays()
    {
        foreach (var source in new[]
                 {
                     new[] { 2, 1 },
                     new[] { 7, -3, 7, 0, -3 },
                     new[] { 9, 1, 8, 2, 7, 3, 6 },
                 })
        {
            AssertAllComparisonSorts(source, Order.ASC);
            AssertAllComparisonSorts(source, Order.DESC);
        }
    }

    [TestMethod]
    public void IterativeMergeSort_SortsLengthsThatAreNotPowersOfTwo()
    {
        foreach (var length in new[] { 3, 5, 7, 9, 13, 17 })
        {
            var source = new int[length];
            for (var index = 0; index < source.Length; index++)
            {
                source[index] = (index * 11 + 3) % 7 - 3;
            }

            AssertMatchesArraySort(source, Order.ASC, "IterativeMergeSort", Lib.IterativeMergeSort);
            AssertMatchesArraySort(source, Order.DESC, "IterativeMergeSort", Lib.IterativeMergeSort);
        }
    }

    [TestMethod]
    public void QuickSort_SortsDuplicateRichInputForEveryPivotStrategy()
    {
        var source = new[] { 3, 1, 3, 2, 3, 0, 2, 1, 3, 0, 2 };

        foreach (var pivot in new[] { Pivot.FIRST, Pivot.LAST, Pivot.RANDOM })
        {
            AssertMatchesArraySort(source, Order.ASC, $"QuickSort ({pivot})", (values, order) => Lib.QuickSort(values, order, pivot));
            AssertMatchesArraySort(source, Order.DESC, $"QuickSort ({pivot})", (values, order) => Lib.QuickSort(values, order, pivot));
        }
    }

    [TestMethod]
    public void ComparisonSorts_SortStringsInBothOrders()
    {
        var source = new[] { "pear", "apple", "banana", "apple", "plum" };

        foreach (var (name, sort) in StringComparisonSorts)
        {
            AssertMatchesArraySort(source, Order.ASC, name, sort);
            AssertMatchesArraySort(source, Order.DESC, name, sort);
        }
    }

    [TestMethod]
    public void CountingSort_SortsSmallRangeWithNegativeValuesAndDuplicates()
    {
        var source = new[] { 5, -3, 0, -3, 5, -1, 2, 0, -20, 20 };

        AssertMatchesArraySort(source, Order.ASC, "CountingSort", Lib.CountingSort);
        AssertMatchesArraySort(source, Order.DESC, "CountingSort", Lib.CountingSort);
    }

    [TestMethod]
    public void RadixSort_SortsLargePositiveAndNegativeMagnitudes()
    {
        var source = new[] { int.MaxValue, int.MinValue + 1, 0, -1, 1, int.MaxValue - 1, -999_999_999 };

        AssertMatchesArraySort(source, Order.ASC, "RadixSort", Lib.RadixSort);
        AssertMatchesArraySort(source, Order.DESC, "RadixSort", Lib.RadixSort);
    }

    [TestMethod]
    public void LinearTimeSorts_SortDeterministicRandomValues()
    {
        var random = new Random(20261117);
        var source = new int[257];
        for (var index = 0; index < source.Length; index++)
        {
            source[index] = random.Next(-100, 101);
        }

        AssertMatchesArraySort(source, Order.ASC, "CountingSort", Lib.CountingSort);
        AssertMatchesArraySort(source, Order.DESC, "CountingSort", Lib.CountingSort);
        AssertMatchesArraySort(source, Order.ASC, "RadixSort", Lib.RadixSort);
        AssertMatchesArraySort(source, Order.DESC, "RadixSort", Lib.RadixSort);
    }

    private static void AssertAllComparisonSorts(int[] source, Order order)
    {
        foreach (var (name, sort) in ComparisonSorts)
        {
            AssertMatchesArraySort(source, order, name, sort);
        }
    }

    private static void AssertMatchesArraySort(int[] source, Order order, string sortName, IntSort sort)
    {
        var expected = (int[])source.Clone();
        var actual = (int[])source.Clone();
        Array.Sort(expected);

        if (order == Order.DESC)
        {
            Array.Reverse(expected);
        }

        sort(actual, order);
        CollectionAssert.AreEqual(expected, actual, $"{sortName} returned an unexpected {order} result.");
    }

    private static void AssertMatchesArraySort(string[] source, Order order, string sortName, StringSort sort)
    {
        var expected = (string[])source.Clone();
        var actual = (string[])source.Clone();
        Array.Sort(expected);

        if (order == Order.DESC)
        {
            Array.Reverse(expected);
        }

        sort(actual, order);
        CollectionAssert.AreEqual(expected, actual, $"{sortName} returned an unexpected {order} result.");
    }
}

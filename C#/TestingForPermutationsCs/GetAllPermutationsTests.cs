using Microsoft.VisualStudio.TestTools.UnitTesting;
using Permutations;

namespace PermutationsTests;

[TestClass]
public class GetAllPermutationsTests
{
    [TestMethod]
    public void GetAllPermutations_ReturnsEveryPermutationForDistinctCharacters()
    {
        var result = Program.GetAllPermutations("abc");
        var expected = new HashSet<string> { "abc", "acb", "bac", "bca", "cab", "cba" };

        Assert.AreEqual(expected.Count, result.Count);
        Assert.IsTrue(result.SetEquals(expected));
    }

    [TestMethod]
    public void GetAllPermutations_ReturnsOnlyUniquePermutationsForRepeatedCharacters()
    {
        var result = Program.GetAllPermutations("aab");
        var expected = new HashSet<string> { "aab", "aba", "baa" };

        Assert.AreEqual(expected.Count, result.Count);
        Assert.IsTrue(result.SetEquals(expected));
    }

    [TestMethod]
    public void GetAllPermutations_ReturnsFactorialNumberOfResultsForFiveDistinctCharacters()
    {
        var result = Program.GetAllPermutations("abcde");

        Assert.AreEqual(120, result.Count);
        Assert.IsTrue(result.Contains("abcde"));
        Assert.IsTrue(result.Contains("edcba"));
    }

    [TestMethod]
    public void GetAllPermutations_ReturnsTheInputForOneCharacter()
    {
        var result = Program.GetAllPermutations("x");

        Assert.IsTrue(result.SetEquals(new[] { "x" }));
    }

    [TestMethod]
    public void GetAllPermutations_RejectsEmptyAndNullInput()
    {
        Assert.ThrowsException<ArgumentException>(() => Program.GetAllPermutations(string.Empty));
        Assert.ThrowsException<ArgumentException>(() => Program.GetAllPermutations(null));
    }
}

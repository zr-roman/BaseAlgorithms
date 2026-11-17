using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringMatching;

namespace TestingForStringMatchingCs;

[TestClass]
public class StringMatchingAlgorithmTests
{
    private static readonly (string Text, string Pattern, int[] Matches)[] Cases =
    {
        ("aaaaa", "aaa", new[] { 0, 1, 2 }),                  // overlapping matches
        ("zzabcxxabc", "abc", new[] { 2, 7 }),               // multiple matches
        ("abcdef", "xy", Array.Empty<int>()),                 // no match
        ("needle", "needle", new[] { 0 }),                    // whole text matches
        ("abc", "abcd", Array.Empty<int>()),                  // pattern longer than text
        ("babx", "ab", new[] { 1 })                           // trivial-hash collision at index 0
    };

    [TestMethod]
    public void BruteForce_FindsAllMatchesAcrossEdgeCases() =>
        Verify((text, pattern) => new BruteForce().GetAllOccurences(text, pattern).Select(i => (int)i));

    [TestMethod]
    public void Kmp_FindsAllMatchesAcrossEdgeCases() =>
        Verify((text, pattern) => new KMP().GetAllOccurences(text, pattern).Select(i => (int)i));

    [TestMethod]
    public void Kmp_EmptyPatternMatchesEveryStringBoundary()
    {
        var kmp = new KMP();

        CollectionAssert.AreEqual(new uint[] { 0 }, kmp.GetAllOccurences(string.Empty, string.Empty));
        CollectionAssert.AreEqual(new uint[] { 0, 1, 2, 3 }, kmp.GetAllOccurences("abc", string.Empty));
    }

    [TestMethod]
    public void FiniteAutomaton_FindsAllMatchesAcrossEdgeCases() =>
        Verify((text, pattern) => new FiniteAutomaton().GetAllOccurences(text, pattern).Select(i => (int)i));

    [TestMethod]
    public void KarpRabinTrivialHash_FindsAllMatchesAcrossEdgeCases() =>
        Verify((text, pattern) => new KarpRabin(KarpRabinHash.TRIVIAL)
            .GetAllOccurences(text, pattern).Select(i => (int)i));

    [TestMethod]
    public void KarpRabinStrongerHash_FindsAllMatchesAcrossEdgeCases() =>
        Verify((text, pattern) => new KarpRabin(KarpRabinHash.STRONGER)
            .GetAllOccurences(text, pattern).Select(i => (int)i));

    [TestMethod]
    public void KarpRabinModes_HandleEmptyAndLongerPatterns()
    {
        foreach (var mode in new[] { KarpRabinHash.TRIVIAL, KarpRabinHash.STRONGER })
        {
            var karpRabin = new KarpRabin(mode);

            CollectionAssert.AreEqual(Array.Empty<uint>(), karpRabin.GetAllOccurences("", "x"));
            CollectionAssert.AreEqual(Array.Empty<uint>(), karpRabin.GetAllOccurences("abc", "abcd"));
            CollectionAssert.AreEqual(new uint[] { 0, 1, 2, 3 }, karpRabin.GetAllOccurences("abc", ""));
        }
    }

    [TestMethod]
    public void KarpRabin_CanBeReusedForDifferentInputs()
    {
        var karpRabin = new KarpRabin();

        CollectionAssert.AreEqual(new uint[] { 0, 2 }, karpRabin.GetAllOccurences("abab", "ab"));
        CollectionAssert.AreEqual(new uint[] { 1 }, karpRabin.GetAllOccurences("xyz", "y"));
        CollectionAssert.AreEqual(Array.Empty<uint>(), karpRabin.GetAllOccurences("short", "longer"));
    }

    [TestMethod]
    public void BoyerMoore_FindsAllMatchesAcrossEdgeCases() =>
        Verify((text, pattern) => new BoyerMoore().GetAllOccurences(text, pattern));

    [TestMethod]
    public void BoyerMoore_HandlesEmptySingleCharacterAndOverlappingMatches()
    {
        var boyerMoore = new BoyerMoore();

        CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, boyerMoore.GetAllOccurences("abc", ""));
        CollectionAssert.AreEqual(new[] { 1, 3, 5 }, boyerMoore.GetAllOccurences("banana", "a"));
        CollectionAssert.AreEqual(new[] { 0, 1, 2 }, boyerMoore.GetAllOccurences("aaaaa", "aaa"));
        CollectionAssert.AreEqual(new[] { 2 }, boyerMoore.GetAllOccurences("zzabc", "abc"));
    }

    [TestMethod]
    public void BoyerMoore_MatchesNaiveSearchForAllShortBinaryStrings()
    {
        var boyerMoore = new BoyerMoore();
        var texts = GenerateStrings(maxLength: 6).ToArray();
        var patterns = GenerateStrings(maxLength: 4).ToArray();

        foreach (var text in texts)
        {
            foreach (var pattern in patterns)
            {
                var expected = new List<int>();
                for (int start = 0; start <= text.Length - pattern.Length; start++)
                {
                    if (string.CompareOrdinal(text, start, pattern, 0, pattern.Length) == 0)
                    {
                        expected.Add(start);
                    }
                }

                CollectionAssert.AreEqual(
                    expected,
                    boyerMoore.GetAllOccurences(text, pattern),
                    $"Text: '{text}', pattern: '{pattern}'");
            }
        }
    }

    private static void Verify(Func<string, string, IEnumerable<int>> find)
    {
        foreach (var testCase in Cases)
        {
            var actual = find(testCase.Text, testCase.Pattern).ToArray();
            CollectionAssert.AreEqual(
                testCase.Matches,
                actual,
                $"Text: '{testCase.Text}', pattern: '{testCase.Pattern}'");
        }
    }

    private static IEnumerable<string> GenerateStrings(int maxLength)
    {
        for (int length = 0; length <= maxLength; length++)
        {
            var count = 1 << length;
            for (int mask = 0; mask < count; mask++)
            {
                var chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    chars[i] = (mask & (1 << i)) == 0 ? 'a' : 'b';
                }

                yield return new string(chars);
            }
        }
    }
}

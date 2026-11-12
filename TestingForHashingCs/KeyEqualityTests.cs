using HashTables;

namespace TestingForHashingCs
{
    /// <summary>
    /// A key whose CompareTo disagrees with Equals: CompareTo ignores case, Equals and GetHashCode do not.
    /// Real keys behave the same way: for strings "a".CompareTo("a­") == 0 (the soft hyphen is ignored
    /// by culture-sensitive comparison) while "a".Equals("a­") is false.
    /// The hash code is passed explicitly so that a test can put two keys onto the same probe sequence.
    /// </summary>
    public sealed class CaseKey : IComparable
    {
        private readonly string text;
        private readonly int hashCode;

        public CaseKey(string text, int hashCode)
        {
            this.text = text;
            this.hashCode = hashCode;
        }

        public override bool Equals(object obj) => obj is CaseKey other && text == other.text;
        public override int GetHashCode() => hashCode;
        public int CompareTo(object obj) => string.Compare(text, ((CaseKey)obj).text, StringComparison.OrdinalIgnoreCase);
        public override string ToString() => text;
    }

    /// <summary>
    /// The number of a key is taken from GetHashCode, and GetHashCode is consistent with Equals, not with CompareTo.
    /// So two keys are the same key only if Equals says so, exactly as in System.Collections.Generic.Dictionary.
    /// </summary>
    [TestClass]
    public class KeyEqualityTests
    {
        private static void AssertBothKeysAreStored(Hashing<CaseKey, string> t, CaseKey lower, CaseKey upper)
        {
            t.Insert(lower, "lower");
            t.Insert(upper, "upper");

            TableAssert.Found(t, lower, "lower", "Key \"abc\"");
            TableAssert.Found(t, upper, "upper", "Key \"ABC\" is a different key: Equals returns false");
        }

        /// <summary>Equal hash codes put both keys onto the same probe sequence.</summary>
        [TestMethod]
        public void OpenAddressingHashTable_KeysEqualOnlyByCompareTo_AreDifferentKeys()
        {
            AssertBothKeysAreStored(new OpenAddressingHashTable<CaseKey, string>(), new CaseKey("abc", 1), new CaseKey("ABC", 1));
        }

        /// <summary>
        /// A table declared for two keys has a single first-level slot, and the first key gets a second-level
        /// table of one slot, so the second key meets the first one. The hash codes differ:
        /// perfect hashing cannot separate keys with equal numbers.
        /// </summary>
        [TestMethod]
        public void PerfectHashTable_KeysEqualOnlyByCompareTo_AreDifferentKeys()
        {
            AssertBothKeysAreStored(new PerfectHashTable<CaseKey, string>(2), new CaseKey("abc", 1), new CaseKey("ABC", 2));
        }

        /// <summary>ChainingHashTable already compares with Equals; the test keeps it that way.</summary>
        [TestMethod]
        public void ChainingHashTable_KeysEqualOnlyByCompareTo_AreDifferentKeys()
        {
            AssertBothKeysAreStored(new ChainingHashTable<CaseKey, string>(), new CaseKey("abc", 1), new CaseKey("ABC", 1));
        }

        [TestMethod]
        public void OpenAddressingHashTable_DeletingOneOfSuchKeys_KeepsTheOther()
        {
            var t = new OpenAddressingHashTable<CaseKey, string>();
            var lower = new CaseKey("abc", 1);
            var upper = new CaseKey("ABC", 1);
            t.Insert(lower, "lower");
            t.Insert(upper, "upper");

            t.Delete(upper);

            TableAssert.Found(t, lower, "lower");
            TableAssert.NotFound(t, upper);
        }
    }
}

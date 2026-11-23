using HashTables;

namespace TestingForHashingCs
{
    /// <summary>Gives the tests the hash function of one method for a table of the given size.</summary>
    public sealed class HashMethodProbe<TKey> : HashTable<TKey, int>
    {
        private HashMethodProbe(HashMethod method, uint size) : base(method)
        {
            m = size;
        }

        public static HashMethodProbe<TKey> Division(uint size) => new(HashMethod.Division, size);
        public static HashMethodProbe<TKey> Multiplication(uint size) => new(HashMethod.Multiplication, size);
        public static HashMethodProbe<TKey> Universal(uint size) => new(HashMethod.UniversalHashing, size);

        public uint Hash(TKey key) => GetHash(key);

        public override void Insert(TKey key, int value) => throw new NotSupportedException();
        public override void Delete(TKey key) => throw new NotSupportedException();
        public override bool Search(TKey key, out int value) => throw new NotSupportedException();
    }

    /// <summary>
    /// Every hash method must give a slot index in [0, m) and spread consecutive keys evenly.
    /// The tables use universal hashing; division and multiplication are checked here directly,
    /// multiplication is also the step of double hashing.
    /// </summary>
    [TestClass]
    public class HashFunctionTests
    {
        private static IEnumerable<int> Keys()
        {
            for (int k = -1000; k <= 1000; k++) yield return k;
            yield return int.MinValue;
            yield return int.MinValue + 1;
            yield return int.MaxValue - 1;
            yield return int.MaxValue;
        }

        private static void AssertInRange(HashMethodProbe<int> probe, uint m, string method)
        {
            foreach (var key in Keys())
            {
                var h = probe.Hash(key);
                Assert.IsTrue(h < m, $"{method}: key {key} gave slot {h}, the table has {m} slots");
            }
        }

        [DataTestMethod]
        [DataRow(1u)]
        [DataRow(16u)]
        [DataRow(17u)]
        [DataRow(1024u)]
        [DataRow(1_000_003u)]
        [DataRow(1u << 31)]
        public void Division_GivesSlotInRange(uint m) => AssertInRange(HashMethodProbe<int>.Division(m), m, "Division");

        [DataTestMethod]
        [DataRow(1u)]
        [DataRow(16u)]
        [DataRow(17u)]
        [DataRow(1024u)]
        [DataRow(1_000_003u)]
        [DataRow(1u << 31)]
        public void Multiplication_GivesSlotInRange(uint m) => AssertInRange(HashMethodProbe<int>.Multiplication(m), m, "Multiplication");

        [DataTestMethod]
        [DataRow(1u)]
        [DataRow(16u)]
        [DataRow(17u)]
        [DataRow(1024u)]
        [DataRow(1_000_003u)]
        [DataRow(1u << 31)]
        public void Universal_GivesSlotInRange(uint m)
        {
            // a and b are random: check several tables
            for (int i = 0; i < 20; i++) AssertInRange(HashMethodProbe<int>.Universal(m), m, "Universal");
        }

        /// <summary>
        /// 10 000 consecutive keys into 1024 slots: about 10 keys per slot. A slot with 4 times the average
        /// means the function groups the keys. For universal hashing the chance of that is negligible.
        /// </summary>
        [DataTestMethod]
        [DataRow("Division")]
        [DataRow("Multiplication")]
        [DataRow("Universal")]
        public void ConsecutiveKeys_AreSpreadEvenly(string method)
        {
            const uint m = 1024;
            const int n = 10_000;
            var probe = method switch
            {
                "Division" => HashMethodProbe<int>.Division(m),
                "Multiplication" => HashMethodProbe<int>.Multiplication(m),
                _ => HashMethodProbe<int>.Universal(m),
            };

            var load = new int[m];
            for (int k = 0; k < n; k++) load[probe.Hash(k)]++;

            int max = load.Max();
            double average = (double)n / m;
            Assert.IsTrue(max < 4 * average, $"{method}: a slot got {max} keys, the average is {average:F1}");
        }

        /// <summary>The hash of a key does not change while the table lives: Search relies on it.</summary>
        [TestMethod]
        public void Universal_SameKey_GivesSameSlot()
        {
            var probe = HashMethodProbe<string>.Universal(1_000_003);
            foreach (var word in new[] { "", "a", "hash", "table", "привет" })
                Assert.AreEqual(probe.Hash(word), probe.Hash(new string(word.ToCharArray())), $"Key \"{word}\"");
        }
    }
}

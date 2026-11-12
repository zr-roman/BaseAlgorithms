using HashTables;
using System.Numerics;

namespace TestingForHashingCs
{
    /// <summary>Gives the tests access to the protected helpers of the Hashing base class.</summary>
    public sealed class HashingProbe<TKey> : HashTable<TKey, int>
    {
        public HashingProbe() : base(HashMethod.UniversalHashing) { }

        public override void Insert(TKey key, int value) => throw new NotSupportedException();
        public override void Delete(TKey key) => throw new NotSupportedException();
        public override bool Search(TKey key, out int value) => throw new NotSupportedException();

        public uint NumericRepresentation(TKey key) => GetNumericRepresentationOfKey(key);
        public uint RandomUInt(uint start, uint end) => GenerateRandomUInt(start, end);
        public uint UniversalHash(TKey key, uint a, uint b, uint m) => GetHash(key, a, b, m);
    }

    [TestClass]
    public class HashingBaseTests
    {
        private CultureInfo savedCulture;

        [TestInitialize]
        public void SaveCulture() => savedCulture = CultureInfo.CurrentCulture;

        [TestCleanup]
        public void RestoreCulture() => CultureInfo.CurrentCulture = savedCulture;

        [DataTestMethod]
        [DataRow(10u, 20u)]
        [DataRow(1u, 1000u)]
        [DataRow(0u, 1u << 20)]
        public void GenerateRandomUInt_StaysInRange(uint start, uint end)
        {
            var p = new HashingProbe<int>();
            for (int i = 0; i < 10_000; i++)
            {
                var x = p.RandomUInt(start, end);
                Assert.IsTrue(x >= start && x <= end, $"GenerateRandomUInt({start}, {end}) returned {x}");
            }
        }

        /// <summary>Parameter b of universal hashing is drawn from [1, uint.MaxValue]: it must be able to be both odd and even.</summary>
        [TestMethod]
        public void GenerateRandomUInt_OverFullRange_GivesOddAndEvenNumbers()
        {
            var p = new HashingProbe<int>();
            int odd = 0;
            for (int i = 0; i < 10_000; i++) if (p.RandomUInt(1, uint.MaxValue) % 2 == 1) odd++;
            Assert.IsTrue(odd > 4_000 && odd < 6_000, $"{odd} odd numbers out of 10000");
        }

        /// <summary>
        /// Universal hashing (CLRS 11.3.3): h(k) = ((a*k + b) mod p) mod m, p = 4294967311.
        /// a*k does not fit into long for large a and k, so the reference uses BigInteger.
        /// </summary>
        [DataTestMethod]
        [DataRow(3u, 7u, 17u)]
        [DataRow(4_000_000_000u, 123_456_789u, 17u)]
        [DataRow(uint.MaxValue - 1, uint.MaxValue - 1, 1_000_003u)]
        public void UniversalHash_MatchesFormula(uint a, uint b, uint m)
        {
            var p = new HashingProbe<int>();
            BigInteger prime = 4294967311;
            // Negative keys too: their numeric representation is large whatever the prehash is.
            for (int key = -500; key < 500; key++)
            {
                BigInteger k = p.NumericRepresentation(key);
                var expected = (uint)(((a * k + b) % prime) % m);
                Assert.AreEqual(expected, p.UniversalHash(key, a, b, m), $"key {key} (numeric {k})");
            }
        }

        [TestMethod]
        public void NumericRepresentation_DoesNotDependOnCulture()
        {
            var p = new HashingProbe<double>();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var invariant = p.NumericRepresentation(1.5);
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            Assert.AreEqual(invariant, p.NumericRepresentation(1.5));
        }
    }
}

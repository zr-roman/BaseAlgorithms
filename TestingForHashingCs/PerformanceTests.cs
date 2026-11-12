using HashTables;
using System.Diagnostics;

namespace TestingForHashingCs
{
    /// <summary>
    /// A hash table must do Insert, Delete and Search in O(1) on average, whatever happened to it before.
    /// Every test here does a fixed amount of work that takes milliseconds when an operation is O(1)
    /// and many seconds when it is O(n). The limit is deliberately generous: it is not a benchmark.
    /// Run only these tests:   dotnet test --filter TestCategory=Performance
    /// Skip them:              dotnet test --filter TestCategory!=Performance
    /// </summary>
    [TestClass]
    [TestCategory("Performance")]
    public class PerformanceTests
    {
        private static readonly TimeSpan Limit = TimeSpan.FromSeconds(3);

        /// <summary>Runs step(0) ... step(steps - 1) and fails as soon as the time limit is exceeded.</summary>
        private static void RunWithinLimit(string what, int steps, Action<int> step)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < steps; i++)
            {
                step(i);
                if ((i & 63) == 0 && sw.Elapsed > Limit)
                    Assert.Fail($"{what}: only {i} of {steps} steps were done in {Limit.TotalSeconds} s");
            }
        }

        private static int[] DistinctRandomKeys(int count, int seed)
        {
            var rnd = new Random(seed);
            var keys = new HashSet<int>();
            while (keys.Count < count) keys.Add(rnd.Next(0, int.MaxValue / 2));
            return keys.ToArray();
        }

        /// <summary>
        /// The number of keys stays at 5000, but every round deletes one key and inserts a new one.
        /// Deleted records must not make later operations slower.
        /// </summary>
        [TestMethod]
        public void DicOpenAddr_ManyDeletesAndInserts_StayFast()
        {
            const int live = 5000, rounds = 200_000;
            var t = new OpenAddressingHashTable<int, int>();
            for (int k = 0; k < live; k++) t.Insert(k, k);

            RunWithinLimit("DicOpenAddr, delete one key and insert another (5000 keys in the table)", rounds, r =>
            {
                t.Delete(r);
                t.Insert(live + r, live + r);
            });

            for (int k = rounds; k < rounds + live; k++) Assert.AreEqual(Tuple.Create(true, k), t.Search(k));
            Assert.IsFalse(t.Search(0).Item1);
        }

        /// <summary>
        /// Searching for a key that is not in the table must be fast for any number of stored keys.
        /// The numbers of keys are just below 16 * 2^12 and 17 * 2^12: with these the table is almost full
        /// if it starts with 16 or 17 slots and doubles only when there is no free slot left.
        /// </summary>
        [DataTestMethod]
        [DataRow(16 * 4096 - 32)]
        [DataRow(17 * 4096 - 32)]
        public void DicOpenAddr_SearchOfAbsentKeys_IsFastForAnyNumberOfKeys(int numberOfKeys)
        {
            var t = new OpenAddressingHashTable<int, int>();
            var keys = DistinctRandomKeys(numberOfKeys, seed: 3);
            RunWithinLimit($"DicOpenAddr, insert {numberOfKeys} keys", keys.Length, i => t.Insert(keys[i], i));

            // keys are below int.MaxValue / 2, so these are absent
            RunWithinLimit($"DicOpenAddr, search of absent keys ({numberOfKeys} keys in the table)", 300_000, i =>
            {
                Assert.IsFalse(t.Search(int.MaxValue / 2 + 1 + i).Item1);
            });
        }

        /// <summary>The table must grow with the number of keys, otherwise the chains become long.</summary>
        [TestMethod]
        public void DicChaining_ManyKeys_StayFast()
        {
            const int n = 300_000;
            var t = new ChainingHashTable<int, int>();
            var keys = DistinctRandomKeys(n, seed: 5);

            RunWithinLimit("DicChaining, insert", n, i => t.Insert(keys[i], i));
            RunWithinLimit("DicChaining, search", n, i => Assert.AreEqual(Tuple.Create(true, i), t.Search(keys[i])));
        }

        /// <summary>
        /// Perfect hashing is built for a known number of keys. When more keys come than was declared,
        /// the table must either refuse them (throw) or stay fast. It must not silently slow down:
        /// each second-level table is rebuilt on every insert and takes (keys in the slot)^2 memory.
        /// </summary>
        [TestMethod]
        public void DicPerfect_MoreKeysThanDeclared_AreRefusedOrStayFast()
        {
            const int declared = 100, n = 20_000;
            var t = new PerfectHashTable<int, int>(declared);
            for (int k = 0; k < declared; k++) t.Insert(k, k);

            bool refused = false;
            int inserted = declared;
            RunWithinLimit($"DicPerfect({declared}), insert of keys beyond the declared number", n - declared, i =>
            {
                if (refused) return;
                try { t.Insert(declared + i, declared + i); inserted++; }
                catch (Exception e) when (e is not OutOfMemoryException) { refused = true; }
            });

            // whatever the table chose, the keys it accepted must be there
            for (int k = 0; k < inserted; k++) Assert.AreEqual(Tuple.Create(true, k), t.Search(k), $"Key {k}");
        }
    }
}

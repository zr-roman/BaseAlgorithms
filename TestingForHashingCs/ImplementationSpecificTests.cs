using HashTables;

namespace TestingForHashingCs
{
    // Behaviour that is specific to one table. What all tables have in common, including the error
    // for a duplicate key, is in DictionaryContractTests.

    [TestClass]
    public class DicOpenAddrTests
    {
        [TestMethod]
        [Timeout(20_000)]
        public void TwentyThousandKeys_AreAllFound()
        {
            var t = new OpenAddressingHashTable<int, int>();
            for (int k = 0; k < 20_000; k++) t.Insert(k, k);
            for (int k = 0; k < 20_000; k++) Assert.IsTrue(t.Search(k).Item1, $"Key {k} must be found");
        }
    }

    [TestClass]
    public class DicPerfectTests
    {
        /// <summary>The constructor computes m = max_num_of_keys / 5, which is 0 for fewer than 5 keys.</summary>
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void SmallCapacity_Works(int n)
        {
            var t = new PerfectHashTable<int, int>(n);
            for (int k = 0; k < n; k++) t.Insert(k, k * 10);
            for (int k = 0; k < n; k++) Assert.AreEqual(Tuple.Create(true, k * 10), t.Search(k));
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(1_000)]
        [DataRow(5_000)]
        [DataRow(10_000)]
        [DataRow(15_000)]
        [DataRow(20_000)]
        [Timeout(60_000)]
        public void TableFilledToCapacity_FindsAllKeys(int n)
        {
            var t = new PerfectHashTable<int, int>(n);
            for (int k = 0; k < n; k++) t.Insert(k, k);
            for (int k = 0; k < n; k++) Assert.AreEqual(Tuple.Create(true, k), t.Search(k), $"Key {k}");
        }

        private static PerfectHashTable<int, string> FullTableOfThreeKeys()
        {
            var t = new PerfectHashTable<int, string>(3);
            t.Insert(1, "a");
            t.Insert(2, "b");
            t.Insert(3, "c");
            return t;
        }

        /// <summary>
        /// A full table has two different errors: a key that is already there is a duplicate (ArgumentException),
        /// a new key does not fit (InvalidOperationException). Neither of them changes the table.
        /// </summary>
        [TestMethod]
        public void FullTable_RejectsDuplicateAndNewKeyWithDifferentErrors()
        {
            var t = FullTableOfThreeKeys();

            Assert.ThrowsException<ArgumentException>(() => t.Insert(2, "again"), "Duplicate of key 2");
            Assert.ThrowsException<InvalidOperationException>(() => t.Insert(4, "d"), "The fourth key in a table for three");

            Assert.AreEqual(Tuple.Create(true, "a"), t.Search(1));
            Assert.AreEqual(Tuple.Create(true, "b"), t.Search(2));
            Assert.AreEqual(Tuple.Create(true, "c"), t.Search(3));
            Assert.IsFalse(t.Search(4).Item1);
        }

        /// <summary>Delete frees a place: the limit is on the number of keys in the table, not on the number of inserts.</summary>
        [TestMethod]
        public void FullTable_AcceptsNewKeyAfterDelete()
        {
            var t = FullTableOfThreeKeys();

            t.Delete(2);
            t.Insert(4, "d");

            Assert.AreEqual(Tuple.Create(true, "d"), t.Search(4));
            Assert.IsFalse(t.Search(2).Item1);
            Assert.ThrowsException<InvalidOperationException>(() => t.Insert(5, "e"), "The table is full again");
        }
    }
}

namespace TestingForHashingCs
{
    /// <summary>Behaviour that differs between the tables: what a duplicate key does, capacity limits.</summary>
    [TestClass]
    public class DicChainingTests
    {
        /// <summary>DicChaining.Insert throws "Key duplicating is not allowed!" for an existing key.</summary>
        [TestMethod]
        public void DuplicateOfTheOnlyKey_Throws()
        {
            var t = new DicChaining<int, string>();
            t.Insert(1, "one");
            Assert.ThrowsException<Exception>(() => t.Insert(1, "uno"));
        }

        [TestMethod]
        public void DuplicateOfAnyKey_Throws()
        {
            var t = new DicChaining<int, string>();
            for (int k = 0; k < 100; k++) t.Insert(k, "v" + k);
            for (int k = 0; k < 100; k++)
                Assert.ThrowsException<Exception>(() => t.Insert(k, "dup"), $"Duplicate of key {k} was accepted");
        }
    }

    [TestClass]
    public class DicOpenAddrTests
    {
        /// <summary>DicOpenAddr.Insert ignores an existing key and keeps the first value.</summary>
        [TestMethod]
        public void DuplicateKey_KeepsFirstValue()
        {
            var t = new DicOpenAddr<int, string>();
            t.Insert(1, "one");
            t.Insert(1, "uno");
            Assert.AreEqual("one", t.Search(1).Item2);
        }

        [TestMethod]
        [Timeout(20_000)]
        public void TwentyThousandKeys_AreAllFound()
        {
            var t = new DicOpenAddr<int, int>();
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
            var t = new DicPerfect<int, int>(n);
            for (int k = 0; k < n; k++) t.Insert(k, k * 10);
            for (int k = 0; k < n; k++) Assert.AreEqual(Tuple.Create(true, k * 10), t.Search(k));
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(1000)]
        [DataRow(20_000)]
        [Timeout(60_000)]
        public void TableFilledToCapacity_FindsAllKeys(int n)
        {
            var t = new DicPerfect<int, int>(n);
            for (int k = 0; k < n; k++) t.Insert(k, k);
            for (int k = 0; k < n; k++) Assert.AreEqual(Tuple.Create(true, k), t.Search(k), $"Key {k}");
        }

        /// <summary>DicPerfect.Insert ignores an existing key and keeps the first value.</summary>
        [TestMethod]
        public void DuplicateKey_KeepsFirstValue()
        {
            var t = new DicPerfect<int, string>(100);
            t.Insert(1, "one");
            t.Insert(1, "uno");
            Assert.AreEqual("one", t.Search(1).Item2);
        }
    }
}

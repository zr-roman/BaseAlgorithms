namespace TestingForHashingCs
{
    /// <summary>
    /// Common behaviour of every hash table: it must work like a Dictionary.
    /// The test methods are inherited by DicChainingContract, DicOpenAddrContract and DicPerfectContract,
    /// so every test runs once for each implementation.
    /// </summary>
    public abstract class DictionaryContractTests
    {
        /// <summary>Capacity passed to DicPerfect; the other tables ignore it.</summary>
        protected const int Capacity = 1000;

        protected abstract Hashing<int, string> CreateIntTable();
        protected abstract Hashing<string, int> CreateStringTable();
        protected abstract Hashing<double, string> CreateDoubleTable();

        private CultureInfo savedCulture;

        [TestInitialize]
        public void SaveCulture()
        {
            savedCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestCleanup]
        public void RestoreCulture() => CultureInfo.CurrentCulture = savedCulture;

        private static void AssertFound<TKey, TValue>(Hashing<TKey, TValue> table, TKey key, TValue expected)
        {
            var r = table.Search(key);
            Assert.IsTrue(r.Item1, $"Key {key} must be found");
            Assert.AreEqual(expected, r.Item2, $"Wrong value for key {key}");
        }

        private static void AssertNotFound<TKey, TValue>(Hashing<TKey, TValue> table, TKey key)
        {
            Assert.IsFalse(table.Search(key).Item1, $"Key {key} must not be found");
        }

        [TestMethod]
        public void EmptyTable_FindsNothing()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 100; k++) AssertNotFound(t, k);
        }

        [TestMethod]
        public void SingleKey_IsFound()
        {
            var t = CreateIntTable();
            t.Insert(1, "one");
            AssertFound(t, 1, "one");
        }

        [TestMethod]
        public void ManyKeys_AreAllFoundWithTheirValues()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 500; k++) t.Insert(k, "v" + k);
            for (int k = 0; k < 500; k++) AssertFound(t, k, "v" + k);
        }

        [TestMethod]
        public void KeysThatWereNotInserted_AreNotFound()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 500; k += 2) t.Insert(k, "v" + k);
            for (int k = 1; k < 500; k += 2) AssertNotFound(t, k);
        }

        [TestMethod]
        public void NegativeKeys_AreFound()
        {
            var t = CreateIntTable();
            for (int k = -100; k < 0; k++) t.Insert(k, "v" + k);
            for (int k = -100; k < 0; k++) AssertFound(t, k, "v" + k);
        }

        [TestMethod]
        public void StringKeys_AreFound()
        {
            var t = CreateStringTable();
            var words = new[] { "", "a", "b", "ab", "ba", "abc", "hash", "table", "Hash", "TABLE", "привет", "мир" };
            for (int i = 0; i < words.Length; i++) t.Insert(words[i], i);
            for (int i = 0; i < words.Length; i++) AssertFound(t, words[i], i);
            AssertNotFound(t, "absent");
        }

        [TestMethod]
        public void Delete_RemovesOnlyThatKey()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 100; k++) t.Insert(k, "v" + k);

            t.Delete(50);

            AssertNotFound(t, 50);
            for (int k = 0; k < 100; k++)
                if (k != 50) AssertFound(t, k, "v" + k);
        }

        [TestMethod]
        public void Delete_OfAbsentKey_ChangesNothing()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 10; k++) t.Insert(k, "v" + k);

            t.Delete(12345);

            for (int k = 0; k < 10; k++) AssertFound(t, k, "v" + k);
        }

        [TestMethod]
        public void Delete_OfEveryKey_LeavesTableEmpty()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 200; k++) t.Insert(k, "v" + k);
            for (int k = 0; k < 200; k++) t.Delete(k);
            for (int k = 0; k < 200; k++) AssertNotFound(t, k);
        }

        [TestMethod]
        public void KeyInsertedAfterDelete_IsFoundWithNewValue()
        {
            var t = CreateIntTable();
            t.Insert(7, "old");
            t.Delete(7);
            t.Insert(7, "new");
            AssertFound(t, 7, "new");
        }

        [TestMethod]
        public void RepeatedInsertAndDeleteOfOneKey_Works()
        {
            var t = CreateIntTable();
            for (int i = 0; i < 20; i++)
            {
                t.Insert(1, "v" + i);
                AssertFound(t, 1, "v" + i);
                t.Delete(1);
                AssertNotFound(t, 1);
            }
        }

        [TestMethod]
        public void TableEmptiedAndRefilled_FindsAllKeys()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 300; k++) t.Insert(k, "a" + k);
            for (int k = 0; k < 300; k++) t.Delete(k);
            for (int k = 0; k < 300; k++) t.Insert(k, "b" + k);
            for (int k = 0; k < 300; k++) AssertFound(t, k, "b" + k);
        }

        /// <summary>Random inserts, deletes and searches compared against System.Collections.Generic.Dictionary.</summary>
        [TestMethod]
        [Timeout(20_000)]
        public void RandomOperations_BehaveLikeDictionary()
        {
            var rnd = new Random(42);
            var t = CreateIntTable();
            var reference = new Dictionary<int, string>();

            for (int op = 0; op < 20_000; op++)
            {
                int k = rnd.Next(300);
                switch (rnd.Next(3))
                {
                    case 0:
                        if (!reference.ContainsKey(k)) { t.Insert(k, "v" + op); reference[k] = "v" + op; }
                        break;
                    case 1:
                        t.Delete(k);
                        reference.Remove(k);
                        break;
                    default:
                        var r = t.Search(k);
                        bool expected = reference.TryGetValue(k, out var value);
                        Assert.AreEqual(expected, r.Item1, $"Operation #{op}: Search({k}) found = {r.Item1}, expected {expected}");
                        if (expected) Assert.AreEqual(value, r.Item2, $"Operation #{op}: Search({k}) returned a wrong value");
                        break;
                }
            }
        }

        /// <summary>
        /// The hash must not depend on CultureInfo.CurrentCulture: 1.5 is "1.5" in Invariant and "1,5" in ru-RU.
        /// Many keys are used because a single key can land in the same slot by chance.
        /// </summary>
        [TestMethod]
        public void DoubleKeys_InsertedUnderOneCulture_AreFoundUnderAnother()
        {
            var t = CreateDoubleTable();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            for (int k = 0; k < 50; k++) t.Insert(k + 0.5, "v" + k);

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            for (int k = 0; k < 50; k++) AssertFound(t, k + 0.5, "v" + k);
        }

        /// <summary>sv-SE uses U+2212 (minus sign) instead of '-' for negative numbers.</summary>
        [TestMethod]
        public void NegativeIntKeys_InsertedUnderOneCulture_AreFoundUnderAnother()
        {
            var t = CreateIntTable();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            for (int k = -40; k < 0; k++) t.Insert(k, "v" + k);

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
            for (int k = -40; k < 0; k++) AssertFound(t, k, "v" + k);
        }
    }

    [TestClass]
    public class DicChainingContract : DictionaryContractTests
    {
        protected override Hashing<int, string> CreateIntTable() => new DicChaining<int, string>();
        protected override Hashing<string, int> CreateStringTable() => new DicChaining<string, int>();
        protected override Hashing<double, string> CreateDoubleTable() => new DicChaining<double, string>();
    }

    [TestClass]
    public class DicOpenAddrContract : DictionaryContractTests
    {
        protected override Hashing<int, string> CreateIntTable() => new DicOpenAddr<int, string>();
        protected override Hashing<string, int> CreateStringTable() => new DicOpenAddr<string, int>();
        protected override Hashing<double, string> CreateDoubleTable() => new DicOpenAddr<double, string>();
    }

    [TestClass]
    public class DicPerfectContract : DictionaryContractTests
    {
        protected override Hashing<int, string> CreateIntTable() => new DicPerfect<int, string>(Capacity);
        protected override Hashing<string, int> CreateStringTable() => new DicPerfect<string, int>(Capacity);
        protected override Hashing<double, string> CreateDoubleTable() => new DicPerfect<double, string>(Capacity);
    }
}

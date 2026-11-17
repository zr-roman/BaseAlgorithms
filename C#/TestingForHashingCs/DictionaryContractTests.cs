using HashTables;

namespace TestingForHashingCs
{
    /// <summary>
    /// Common behaviour of every hash table: it must work like a Dictionary.
    /// The test methods are inherited by ChainingHashTableContract, OpenAddressingHashTableContract and PerfectHashTableContract,
    /// so every test runs once for each implementation.
    /// </summary>
    public abstract class DictionaryContractTests
    {
        /// <summary>Capacity passed to PerfectHashTable; the other tables ignore it.</summary>
        protected const int Capacity = 1000;

        protected abstract HashTable<int, string> CreateIntTable();
        protected abstract HashTable<string, int> CreateStringTable();
        protected abstract HashTable<double, string> CreateDoubleTable();

        private CultureInfo savedCulture;

        [TestInitialize]
        public void SaveCulture()
        {
            savedCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestCleanup]
        public void RestoreCulture() => CultureInfo.CurrentCulture = savedCulture;

        private static void AssertFound<TKey, TValue>(HashTable<TKey, TValue> table, TKey key, TValue expected)
        {
            TableAssert.Found(table, key, expected);
        }

        private static void AssertNotFound<TKey, TValue>(HashTable<TKey, TValue> table, TKey key)
        {
            TableAssert.NotFound(table, key);
        }

        /// <summary>
        /// Insert of a key that is already in the table is an error: every table throws
        /// ArgumentException("Key duplicating is not allowed!"). If the type of the exception changes, change it here.
        /// </summary>
        private static void AssertDuplicateIsRejected(HashTable<int, string> table, int key)
        {
            Assert.ThrowsException<ArgumentException>(() => table.Insert(key, "duplicate"), $"Duplicate of key {key} was accepted");
        }

        [TestMethod]
        public void DuplicateOfTheOnlyKey_Throws()
        {
            var t = CreateIntTable();
            t.Insert(1, "one");
            AssertDuplicateIsRejected(t, 1);
        }

        [TestMethod]
        public void DuplicateOfAnyKey_Throws()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 100; k++) t.Insert(k, "v" + k);
            for (int k = 0; k < 100; k++) AssertDuplicateIsRejected(t, k);
        }

        [TestMethod]
        public void RejectedDuplicate_LeavesTheTableUnchanged()
        {
            var t = CreateIntTable();
            for (int k = 0; k < 100; k++) t.Insert(k, "v" + k);
            for (int k = 0; k < 100; k++) AssertDuplicateIsRejected(t, k);

            for (int k = 0; k < 100; k++) AssertFound(t, k, "v" + k);

            // the table keeps working after the errors
            t.Delete(50);
            AssertNotFound(t, 50);
            t.Insert(50, "new");
            AssertFound(t, 50, "new");
            t.Insert(100, "v100");
            AssertFound(t, 100, "v100");
        }

        /// <summary>A null key is an error, as in Dictionary: Insert throws ArgumentNullException that names the parameter.</summary>
        [TestMethod]
        public void InsertOfNullKey_Throws()
        {
            var t = CreateStringTable();

            var e = Assert.ThrowsException<ArgumentNullException>(() => t.Insert(null, 1));

            Assert.AreEqual("key", e.ParamName, "The exception must name the parameter, as ArgumentNullException.ThrowIfNull( key ) does");
        }

        [TestMethod]
        public void RejectedNullKey_LeavesTheTableUnchanged()
        {
            var t = CreateStringTable();
            t.Insert("a", 1);
            t.Insert("b", 2);

            Assert.ThrowsException<ArgumentNullException>(() => t.Insert(null, 3));

            AssertFound(t, "a", 1);
            AssertFound(t, "b", 2);
            t.Insert("c", 3);
            AssertFound(t, "c", 3);
        }

        /// <summary>
        /// Search and Delete do not reject null: such a key cannot be in the table, so nothing is found
        /// and nothing is deleted. The test records this behaviour; Dictionary throws here as well.
        /// </summary>
        [TestMethod]
        public void SearchAndDeleteOfNullKey_FindNothing()
        {
            var t = CreateStringTable();
            t.Insert("a", 1);

            AssertNotFound(t, null);
            t.Delete(null);

            AssertFound(t, "a", 1);
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
                        bool found = t.Search(k, out var actual);
                        bool expected = reference.TryGetValue(k, out var value);
                        Assert.AreEqual(expected, found, $"Operation #{op}: Search({k}) found = {found}, expected {expected}");
                        Assert.AreEqual(value, actual, $"Operation #{op}: Search({k}) returned a wrong value");
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
            for (int k = -40; k < 0; k++) t.Insert(k, "v" + (k + 40));  // not "v" + k: under sv-SE it is formatted with another minus sign

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
            for (int k = -40; k < 0; k++) AssertFound(t, k, "v" + (k + 40));
        }
    }

    [TestClass]
    public class ChainingHashTableContract : DictionaryContractTests
    {
        protected override HashTable<int, string> CreateIntTable() => new ChainingHashTable<int, string>();
        protected override HashTable<string, int> CreateStringTable() => new ChainingHashTable<string, int>();
        protected override HashTable<double, string> CreateDoubleTable() => new ChainingHashTable<double, string>();
    }

    [TestClass]
    public class OpenAddressingHashTableContract : DictionaryContractTests
    {
        protected override HashTable<int, string> CreateIntTable() => new OpenAddressingHashTable<int, string>();
        protected override HashTable<string, int> CreateStringTable() => new OpenAddressingHashTable<string, int>();
        protected override HashTable<double, string> CreateDoubleTable() => new OpenAddressingHashTable<double, string>();
    }

    /// <summary>The same tests for open addressing with linear probing (the default is quadratic).</summary>
    [TestClass]
    public class OpenAddressingLinearProbingContract : DictionaryContractTests
    {
        protected override HashTable<int, string> CreateIntTable() => new OpenAddressingHashTable<int, string>(ProbingMethod.LinearProbing);
        protected override HashTable<string, int> CreateStringTable() => new OpenAddressingHashTable<string, int>(ProbingMethod.LinearProbing);
        protected override HashTable<double, string> CreateDoubleTable() => new OpenAddressingHashTable<double, string>(ProbingMethod.LinearProbing);
    }

    /// <summary>The same tests for open addressing with double hashing.</summary>
    [TestClass]
    public class OpenAddressingDoubleHashingContract : DictionaryContractTests
    {
        protected override HashTable<int, string> CreateIntTable() => new OpenAddressingHashTable<int, string>(ProbingMethod.DoubleHashing);
        protected override HashTable<string, int> CreateStringTable() => new OpenAddressingHashTable<string, int>(ProbingMethod.DoubleHashing);
        protected override HashTable<double, string> CreateDoubleTable() => new OpenAddressingHashTable<double, string>(ProbingMethod.DoubleHashing);
    }

    [TestClass]
    public class PerfectHashTableContract : DictionaryContractTests
    {
        protected override HashTable<int, string> CreateIntTable() => new PerfectHashTable<int, string>(Capacity);
        protected override HashTable<string, int> CreateStringTable() => new PerfectHashTable<string, int>(Capacity);
        protected override HashTable<double, string> CreateDoubleTable() => new PerfectHashTable<double, string>(Capacity);
    }
}

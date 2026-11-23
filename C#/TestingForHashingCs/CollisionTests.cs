using HashTables;

namespace TestingForHashingCs
{
    /// <summary>
    /// A key whose hash code is set explicitly, so that a test can make keys collide.
    /// Two keys are equal when their Id is equal; a test must give equal Ids equal hash codes.
    /// </summary>
    public sealed class CollidingKey
    {
        public CollidingKey(int id, int hash)
        {
            Id = id;
            Hash = hash;
        }

        public int Id { get; }
        public int Hash { get; }

        public override bool Equals(object obj) => obj is CollidingKey other && Id == other.Id;
        public override int GetHashCode() => Hash;
        public override string ToString() => $"#{Id} (hash {Hash})";
    }

    /// <summary>
    /// Keys with equal hash codes: they get one chain (chaining) or one probe sequence (open addressing,
    /// with every probing method, because the step of double hashing is also computed from the hash code).
    /// These tests check what happens inside a chain or a probe sequence: deleting its first, middle and last key,
    /// searching past a deleted record and rejecting a duplicate that lies behind a deleted record.
    /// Six or more keys are used, so that a delete does not shrink the open addressing table:
    /// shrinking rebuilds the table and removes the deleted records, and the test would not see them.
    /// </summary>
    public abstract class CollisionTests
    {
        protected abstract HashTable<CollidingKey, string> CreateTable();
        protected abstract HashTable<int, string> CreateIntTable();

        private const int SameHash = 42;

        private static CollidingKey Same(int id) => new(id, SameHash);

        private HashTable<CollidingKey, string> TableOfCollidingKeys(int count)
        {
            var t = CreateTable();
            for (int i = 0; i < count; i++) t.Insert(Same(i), "v" + i);
            return t;
        }

        [TestMethod]
        public void FiftyKeysWithEqualHashCodes_AreAllFound()
        {
            var t = TableOfCollidingKeys(50);
            for (int i = 0; i < 50; i++) TableAssert.Found(t, Same(i), "v" + i);
            TableAssert.NotFound(t, Same(50));
        }

        /// <summary>The first, a middle and the last key of the chain or the probe sequence.</summary>
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(2)]
        [DataRow(5)]
        public void DeleteOfOneCollidingKey_KeepsTheOthers(int deleted)
        {
            var t = TableOfCollidingKeys(6);

            t.Delete(Same(deleted));

            TableAssert.NotFound(t, Same(deleted));
            for (int i = 0; i < 6; i++)
                if (i != deleted) TableAssert.Found(t, Same(i), "v" + i, $"Key {i} after the delete of key {deleted}");
        }

        /// <summary>Search must go past a deleted record: the keys behind it are still in the table.</summary>
        [TestMethod]
        public void DeletingEveryOtherCollidingKey_KeepsTheRest()
        {
            var t = TableOfCollidingKeys(50);

            for (int i = 0; i < 50; i += 2) t.Delete(Same(i));

            for (int i = 0; i < 50; i++)
                if (i % 2 == 0) TableAssert.NotFound(t, Same(i));
                else TableAssert.Found(t, Same(i), "v" + i);

            for (int i = 0; i < 50; i += 2) t.Insert(Same(i), "new" + i);

            for (int i = 0; i < 50; i++) TableAssert.Found(t, Same(i), (i % 2 == 0 ? "new" : "v") + i);
        }

        /// <summary>
        /// The first key of the probe sequence is deleted, the second one is inserted again.
        /// Insert must find the second key behind the deleted record and reject it,
        /// not put a second copy into the freed slot.
        /// </summary>
        [TestMethod]
        public void DuplicateBehindDeletedKey_IsRejected()
        {
            var t = TableOfCollidingKeys(6);
            t.Delete(Same(0));

            Assert.ThrowsException<ArgumentException>(() => t.Insert(Same(1), "duplicate"));

            TableAssert.Found(t, Same(1), "v1");

            // there is exactly one copy of key 1: after its delete it is not found
            t.Delete(Same(1));
            TableAssert.NotFound(t, Same(1));
            for (int i = 2; i < 6; i++) TableAssert.Found(t, Same(i), "v" + i);
        }

        [TestMethod]
        public void KeyReinsertedIntoItsFreedSlot_IsStoredOnce()
        {
            var t = TableOfCollidingKeys(6);

            t.Delete(Same(2));
            t.Insert(Same(2), "new");
            TableAssert.Found(t, Same(2), "new");

            t.Delete(Same(2));
            TableAssert.NotFound(t, Same(2));
            for (int i = 0; i < 6; i++)
                if (i != 2) TableAssert.Found(t, Same(i), "v" + i);
        }

        [TestMethod]
        public void NullValue_IsStoredAndFound()
        {
            var t = CreateIntTable();
            t.Insert(1, null);
            t.Insert(2, "two");

            Assert.IsTrue(t.Search(1, out var value), "A key with the null value is in the table");
            Assert.IsNull(value);
            TableAssert.Found(t, 2, "two");
        }

        /// <summary>
        /// Random operations on 200 keys that share only 4 hash codes, compared against Dictionary.
        /// Long chains and long probe sequences full of deleted records are the usual place for bugs.
        /// </summary>
        [TestMethod]
        [Timeout(20_000)]
        public void RandomOperationsOnCollidingKeys_BehaveLikeDictionary()
        {
            var rnd = new Random(7);
            var t = CreateTable();
            var reference = new Dictionary<int, string>();

            static CollidingKey Key(int id) => new(id, id % 4);

            for (int op = 0; op < 20_000; op++)
            {
                int id = rnd.Next(200);
                switch (rnd.Next(3))
                {
                    case 0:
                        if (reference.ContainsKey(id))
                        {
                            Assert.ThrowsException<ArgumentException>(() => t.Insert(Key(id), "dup"), $"Operation #{op}: duplicate of key {id} was accepted");
                        }
                        else
                        {
                            t.Insert(Key(id), "v" + op);
                            reference[id] = "v" + op;
                        }
                        break;
                    case 1:
                        t.Delete(Key(id));
                        reference.Remove(id);
                        break;
                    default:
                        bool found = t.Search(Key(id), out var actual);
                        bool expected = reference.TryGetValue(id, out var value);
                        Assert.AreEqual(expected, found, $"Operation #{op}: Search({id}) found = {found}, expected {expected}");
                        Assert.AreEqual(value, actual, $"Operation #{op}: Search({id}) returned a wrong value");
                        break;
                }
            }

            for (int id = 0; id < 200; id++)
            {
                if (reference.TryGetValue(id, out var value)) TableAssert.Found(t, Key(id), value);
                else TableAssert.NotFound(t, Key(id));
            }
        }
    }

    [TestClass]
    public class ChainingCollisionTests : CollisionTests
    {
        protected override HashTable<CollidingKey, string> CreateTable() => new ChainingHashTable<CollidingKey, string>();
        protected override HashTable<int, string> CreateIntTable() => new ChainingHashTable<int, string>();
    }

    [TestClass]
    public class OpenAddressingQuadraticCollisionTests : CollisionTests
    {
        protected override HashTable<CollidingKey, string> CreateTable() => new OpenAddressingHashTable<CollidingKey, string>(ProbingMethod.QuadraticProbing);
        protected override HashTable<int, string> CreateIntTable() => new OpenAddressingHashTable<int, string>(ProbingMethod.QuadraticProbing);
    }

    [TestClass]
    public class OpenAddressingLinearCollisionTests : CollisionTests
    {
        protected override HashTable<CollidingKey, string> CreateTable() => new OpenAddressingHashTable<CollidingKey, string>(ProbingMethod.LinearProbing);
        protected override HashTable<int, string> CreateIntTable() => new OpenAddressingHashTable<int, string>(ProbingMethod.LinearProbing);
    }

    [TestClass]
    public class OpenAddressingDoubleHashingCollisionTests : CollisionTests
    {
        protected override HashTable<CollidingKey, string> CreateTable() => new OpenAddressingHashTable<CollidingKey, string>(ProbingMethod.DoubleHashing);
        protected override HashTable<int, string> CreateIntTable() => new OpenAddressingHashTable<int, string>(ProbingMethod.DoubleHashing);
    }

    /// <summary>
    /// Perfect hashing cannot hold keys with equal hash codes, so its keys collide only on the first level.
    /// A table for 4 keys has a single first-level slot (m = max(1, 4 / 5) = 1): all its keys share one
    /// second-level table, which is rebuilt on every insert and delete.
    /// </summary>
    [TestClass]
    public class PerfectHashTableSingleSlotTests
    {
        private const int Capacity = 4;

        private static PerfectHashTable<int, string> FullTable()
        {
            var t = new PerfectHashTable<int, string>(Capacity);
            for (int k = 1; k <= Capacity; k++) t.Insert(k, "v" + k);
            return t;
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void DeleteOfOneKey_KeepsTheOthersAndFreesAPlace(int deleted)
        {
            var t = FullTable();

            t.Delete(deleted);

            TableAssert.NotFound(t, deleted);
            for (int k = 1; k <= Capacity; k++)
                if (k != deleted) TableAssert.Found(t, k, "v" + k);

            t.Insert(100, "new");
            TableAssert.Found(t, 100, "new");
            for (int k = 1; k <= Capacity; k++)
                if (k != deleted) TableAssert.Found(t, k, "v" + k);
        }

        [TestMethod]
        public void TableEmptiedAndRefilled_FindsAllKeys()
        {
            var t = FullTable();
            for (int k = 1; k <= Capacity; k++) t.Delete(k);
            for (int k = 1; k <= Capacity; k++) TableAssert.NotFound(t, k);

            for (int k = 11; k <= 10 + Capacity; k++) t.Insert(k, "b" + k);
            for (int k = 11; k <= 10 + Capacity; k++) TableAssert.Found(t, k, "b" + k);
        }

        [TestMethod]
        public void NullValue_IsStoredAndFound()
        {
            var t = new PerfectHashTable<int, string>(Capacity);
            t.Insert(1, null);

            Assert.IsTrue(t.Search(1, out var value), "A key with the null value is in the table");
            Assert.IsNull(value);
        }

        /// <summary>Random operations on one second-level table, compared against Dictionary.</summary>
        [TestMethod]
        [Timeout(20_000)]
        public void RandomOperations_BehaveLikeDictionary()
        {
            var rnd = new Random(11);
            var t = new PerfectHashTable<int, string>(Capacity);
            var reference = new Dictionary<int, string>();

            for (int op = 0; op < 5_000; op++)
            {
                int k = rnd.Next(10);
                switch (rnd.Next(3))
                {
                    case 0:
                        if (reference.ContainsKey(k))
                            Assert.ThrowsException<ArgumentException>(() => t.Insert(k, "dup"), $"Operation #{op}: duplicate of key {k}");
                        else if (reference.Count == Capacity)
                            Assert.ThrowsException<InvalidOperationException>(() => t.Insert(k, "extra"), $"Operation #{op}: key {k} in a full table");
                        else
                        {
                            t.Insert(k, "v" + op);
                            reference[k] = "v" + op;
                        }
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
    }
}

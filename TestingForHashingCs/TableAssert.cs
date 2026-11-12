using HashTables;

namespace TestingForHashingCs
{
    /// <summary>Checks of Search( key, out value ) shared by all the test classes.</summary>
    internal static class TableAssert
    {
        /// <summary>The key is in the table and has the expected value.</summary>
        public static void Found<TKey, TValue>(HashTable<TKey, TValue> table, TKey key, TValue expected, string message = null)
        {
            bool found = table.Search(key, out var value);
            Assert.IsTrue(found, message ?? $"Key {key} must be found");
            Assert.AreEqual(expected, value, message ?? $"Wrong value for key {key}");
        }

        /// <summary>The key is not in the table, and the out value is default, as in Dictionary.TryGetValue.</summary>
        public static void NotFound<TKey, TValue>(HashTable<TKey, TValue> table, TKey key, string message = null)
        {
            bool found = table.Search(key, out var value);
            Assert.IsFalse(found, message ?? $"Key {key} must not be found");
            Assert.AreEqual(default, value, $"Search of the absent key {key} must set the out value to default");
        }
    }
}

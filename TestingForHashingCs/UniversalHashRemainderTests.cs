using HashTables;
using System.Numerics;

namespace TestingForHashingCs
{
    /// <summary>
    /// h(k) = ((a*k + b) mod p) mod m with p = 4294967311.
    /// p is greater than uint.MaxValue (4294967295), so (a*k + b) mod p can be 4294967296 ... 4294967310,
    /// and these 15 remainders do not fit into uint. They must not be truncated before "mod m".
    /// </summary>
    [TestClass]
    public class UniversalHashRemainderTests
    {
        private static readonly BigInteger Prime = 4294967311;

        [DataTestMethod]
        [DataRow(0u)]
        [DataRow(7u)]
        [DataRow(14u)]
        public void RemainderAboveUIntMaxValue_IsNotTruncated(uint offset)
        {
            var p = new HashingProbe<int>();
            int checkedKeys = 0;

            for (int key = -500; key < 500; key++)
            {
                BigInteger k = p.NumericRepresentation(key);
                if (k == 0) continue;

                // With a = 1 choose b so that a*k + b = 2^32 + offset, which is below p and above uint.MaxValue.
                BigInteger bBig = 4294967296 + offset - k;
                if (bBig < 0 || bBig > uint.MaxValue) continue;
                uint b = (uint)bBig;

                foreach (uint m in new[] { 17u, 1000u, 1_000_003u })
                {
                    var expected = (uint)(((1 * k + b) % Prime) % m);
                    Assert.AreEqual(expected, p.UniversalHash(key, 1, b, m),
                        $"key {key} (numeric {k}), a = 1, b = {b}, m = {m}: (a*k + b) mod p = {(k + b) % Prime}");
                }
                checkedKeys++;
            }

            Assert.IsTrue(checkedKeys > 0, "No key was suitable for the check");
        }
    }
}

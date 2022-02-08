using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class Heapsort: TestingBase {

        [TestMethod]
        public void TestAsc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.Heapsort( arr, Order.ASC );
                AssertionAsc();
            }
        }

        [TestMethod]
        public void TestDesc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.Heapsort( arr, Order.DESC );
                AssertionDesc();
            }
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class MergeSort: TestingBase {

        [TestMethod]
        public void TestAsc() {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.MergeSort( arr, Order.ASC, int.MinValue, int.MaxValue );
                AssertionAsc();
            }
        }

        [TestMethod]
        public void TestDesc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.MergeSort( arr, Order.DESC, int.MinValue, int.MaxValue );
                AssertionDesc();
            }
        }

    }
}

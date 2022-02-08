using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class InsertionSort: TestingBase {

        [TestMethod]
        public void TestAsc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.InsertionSort(arr, Order.ASC);
                AssertionAsc();
            }
        }

        [TestMethod]
        public void TestDesc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.InsertionSort(arr, Order.DESC);
                AssertionDesc();
            }
        }

    }
}

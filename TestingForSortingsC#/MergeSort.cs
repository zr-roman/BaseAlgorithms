using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class MergeSort: TestingBase {

        [TestMethod]
        public void TestAsc() {
            while (--arr_Size > 0)
            {
                GenerateNewArray();
                var order = Order.ASC;
                Lib.MergeSort( arr, order );
                Assertion(order);
            }
        }

        [TestMethod]
        public void TestDesc()
        {
            while (--arr_Size > 0)
            {
                GenerateNewArray();
                var order = Order.DESC;
                Lib.MergeSort( arr, order );
                Assertion(order);
            }
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing
{

    [TestClass]
    public class IterativeMergeSort : TestingBase
    {
        [TestMethod]
        public void TestAsc()
        {
            while (--arr_Size > 0)
            {
                var order = Order.ASC;
                GenerateNewArray();
                Lib.IterativeMergeSort(arr, order);
                Assertion(order);
            }
        }

        [TestMethod]
        public void TestDesc()
        {
            while (--arr_Size > 0)
            {
                var order = Order.DESC;
                GenerateNewArray();
                Lib.IterativeMergeSort(arr, order);
                Assertion(order);
            }
        }

    }
}

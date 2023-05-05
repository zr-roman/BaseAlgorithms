using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class ShellSort : TestingBase {

         [TestMethod]
        public void TestAsc()
        {
            while (--arr_Size > 0)
            {
                GenerateNewArray();
                var order = Order.ASC;
                Lib.ShellSort(arr, order);
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
                Lib.ShellSort(arr, order);
                Assertion(order);
            }
        }

    }
}

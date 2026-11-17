using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class QuickSort: TestingBase {

        [TestMethod]
        public void TestPivotFirst() {
            while ( --arr_Size > 0 ) {
                GenerateNewArray();
                var order = Order.ASC;
                Lib.QuickSort( arr, order, Pivot.FIRST );
                Assertion(order);
            }
        }

        [TestMethod]
        public void TestPivotLast()
        {
            while (--arr_Size > 0) {
                GenerateNewArray();
                var order = Order.ASC;
                Lib.QuickSort(arr, order, Pivot.LAST);
                Assertion(order);
            }
        }

        [TestMethod]
        public void TestPivotRandom()
        {
            while (--arr_Size > 0)
            {
                GenerateNewArray();
                var order = Order.ASC;
                Lib.QuickSort(arr, order, Pivot.RANDOM);
                Assertion(order);
            }
        }

        [TestMethod]
        public void TestPivotDesc()
        {
            while (--arr_Size > 0)
            {
                GenerateNewArray();
                var order = Order.DESC;
                Lib.QuickSort(arr, order, Pivot.FIRST);
                Assertion(order);
            }
        }

        [TestMethod]
        public void TestStringsAsc()
        {
            var a = new string[] { "Сидоров", "Иванов", "Петров" };
            Lib.QuickSort( a, Order.ASC, Pivot.RANDOM );
            Assert.IsTrue( a[0] == "Иванов" && a[1] == "Петров" && a[2] == "Сидоров" );
            
        }

        [TestMethod]
        public void TestStringsDesc()
        {
            var a = new string[] { "Иванов", "Сидоров", "Петров" };
            Lib.QuickSort(a, Order.DESC, Pivot.RANDOM);
            Assert.IsTrue(a[0] == "Сидоров" && a[1] == "Петров" && a[2] == "Иванов");

        }
    }
}

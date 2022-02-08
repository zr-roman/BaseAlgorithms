using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class QuickSort: TestingBase {

        [TestMethod]
        public void TestPivotFirst() {
            while ( N-- > 0 ) {
                GenerateNewArray();
                Lib.QuickSort( arr, Order.ASC, Pivot.FIRST );
                AssertionAsc();
            }
        }

        [TestMethod]
        public void TestPivotLast()
        {
            while (N--> 0) {
                GenerateNewArray();
                Lib.QuickSort(arr, Order.ASC, Pivot.LAST);
                AssertionAsc();
            }
        }

        [TestMethod]
        public void TestPivotRandom()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.QuickSort(arr, Order.ASC, Pivot.RANDOM);
                AssertionAsc();
            }
        }

        [TestMethod]
        public void TestPivotDesc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.QuickSort(arr, Order.DESC, Pivot.FIRST);
                AssertionDesc();
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

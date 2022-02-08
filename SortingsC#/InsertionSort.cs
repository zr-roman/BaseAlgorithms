
using System;

namespace Sortings {

    public static partial class Lib {
                
        public static void InsertionSort<T>( T[] arr, Order order ) where T: IComparable<T> {
            // InsertionSort - it is a ShellSort with the only one gap = 1
            int[] g = { 1 };
            DoPerformSort( new ShellSortInner<T>( arr, order, g ) );
        }
    }
}

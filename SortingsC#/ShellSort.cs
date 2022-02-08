
using System;

namespace Sortings {

    public static partial class Lib {

        public static void ShellSort<T>( T[] arr, Order order ) where T : IComparable<T> {
            DoPerformSort( new ShellSortInner<T>( arr, order, gaps ) );
        }

        private class ShellSortInner<T> : SortAbstract<T> where T : IComparable<T> {

            public ShellSortInner( T[] arr, Order order, int[] gaps ) : base( arr, order ) {
                this.gaps = gaps;
            }

            protected override void DoSort( uint start, uint end ) {

                bool comparison_succeeds( T key, T comparant ) {
                    return (order == Order.ASC && key.CompareTo(comparant) < 0) || (order == Order.DESC && key.CompareTo(comparant) > 0);
                }

                foreach ( var gap in gaps ) {

                    for ( int i = 0 + gap; i < arr.Length; i++) {
                        var key = arr[ i ];
                        var j = i - gap;
                        while ( j >= 0 && comparison_succeeds( key, arr[ j ] ) ) {
                            arr[ j + gap ] = arr[ j ];
                            j -= gap;
                        }
                        arr[ j + gap ] = key;
                    }
                }
                
            }
            private readonly int[] gaps;
        }
        private static int[] gaps = { 1750, 701, 301, 132, 57, 23, 10, 4, 1 };
    }
}

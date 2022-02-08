
using System;

namespace Sortings {

    public static partial class Lib {

        public static void SelectionSort<T>( T[] arr, Order order ) where T: IComparable<T> {
            DoPerformSort( new SelectionSortInner<T>( arr, order ) );
        }

        private class SelectionSortInner<T> : SortAbstract<T> where T: IComparable<T> {

            public SelectionSortInner( T[] arr, Order order ) : base( arr, order ) { }
        
            protected override void DoSort( uint start, uint end ) {

                bool comparison_succeeds( T key, T comparant ) {
                    return ( order == Order.ASC && comparant.CompareTo( key ) < 0 ) || ( order == Order.DESC && comparant.CompareTo( key ) > 0 );
                }

                for ( uint i = 0; i < arr.Length; i++ ) {
                    var iMin = i;
                    for ( uint j = i + 1; j < arr.Length; j++ ) {
                        if ( comparison_succeeds( arr[ iMin ], arr[ j ] ) ) {
                            iMin = j;
                        }
                    }
                    if ( iMin != i ) {
                        Swap( i, iMin );
                    }
                }
            }
        }
    }
}

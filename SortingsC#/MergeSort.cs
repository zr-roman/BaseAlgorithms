using System;

namespace Sortings {

    public static partial class Lib {
        
        public static void MergeSort<T>( T[] arr, Order order ) where T: IComparable<T> {
            DoPerformSort( new MergeSortInner<T>( arr, order ) );
        }

        private class MergeSortInner<T> : SortAbstract<T> where T: IComparable<T> {
                        
            public MergeSortInner( T[] arr, Order order ) : base( arr, order) {

            }

            protected override void DoSort( uint start, uint end ) {
                if ( start >= end ) {
                    return;
                }
                uint mid = ( start + end ) / 2;
                Sort( start, mid );
                Sort( mid + 1, end );
                Merge( start, mid, end );
            }

            private void Merge( uint start, uint mid, uint end ) {
                
                var n1 = mid - start + 1;
                var n2 = end - mid;

                var arrayB = new T[ n1 ];
                var arrayC = new T[ n2 ];
                
                var bi = 0;
                var ci = 0;

                for ( uint i = start; i <= mid; i++ ) {
                    arrayB[ bi++ ] = arr[ i ];
                }
                                
                for ( uint i = mid + 1; i <= end; i++ ) {
                    arrayC[ ci++ ] = arr[ i ];
                }
                
                bi = ci = 0;

                for ( uint i = start; i <= end; i++ ) {

                    if ( bi >= arrayB.Length ) {
                        arr[ i ] = arrayC[ ci++ ];
                        continue;
                    }

                    if ( ci >= arrayC.Length ) {
                        arr[ i ] = arrayB[ bi++ ];
                        continue;
                    }

                    var compareResult = arrayB[ bi ].CompareTo( arrayC[ ci ] );

                    if ( ( order.Equals( Order.ASC ) && compareResult <= 0 ) || ( order.Equals( Order.DESC ) && compareResult > 0 ) ) {
                        arr[ i ] = arrayB[ bi++ ];
                    } else {
                        arr[ i ] = arrayC[ ci++ ];
                    }
                }

            }
        }
    }
}

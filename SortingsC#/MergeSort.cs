
using System;

namespace Sortings {

    public static partial class Lib {
        
        public static void MergeSort<T>( T[] arr, Order order, T minInfinity, T maxInfinity ) where T: IComparable<T> {
            DoPerformSort( new MergeSortInner<T>( arr, order, minInfinity, maxInfinity ) );
        }

        private class MergeSortInner<T> : SortAbstract<T> where T: IComparable<T> {
            
            private T minInfinity;
            private T maxInfinity;

            public MergeSortInner( T[] arr, Order order, T minInfinity, T maxInfinity ) : base( arr, order ) {
                this.minInfinity = minInfinity;
                this.maxInfinity = maxInfinity;
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

                var arrayB = new T[ n1 + 1 ];
                var arrayC = new T[ n2 + 1 ];
                
                var bi = 0;
                var ci = 0;

                for ( uint i = start; i <= mid; i++ ) {
                    arrayB[ bi++ ] = arr[ i ];
                }
                arrayB[ ^1 ] = order.Equals( Order.ASC ) ? maxInfinity : minInfinity;
                
                for ( uint i = mid + 1; i <= end; i++ ) {
                    arrayC[ ci++ ] = arr[ i ];
                }
                arrayC[ ^1 ] = order.Equals( Order.ASC ) ? maxInfinity : minInfinity;

                bi = ci = 0;

                for ( uint i = start; i <= end; i++ ) {
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

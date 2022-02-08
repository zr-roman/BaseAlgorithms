
using System;

namespace Sortings {

    public static partial class Lib {

        public static void QuickSort<T>( T[] arr, Order order, Pivot pivot ) where T: IComparable<T> {
            DoPerformSort( new QuickSortInner<T>( arr, order, pivot ) );
        }

        private class QuickSortInner<T> : SortAbstract<T> where T: IComparable<T> {
            
            private readonly Pivot pivot;

            public QuickSortInner( T[] arr, Order order, Pivot pivot ) : base( arr, order) {
                this.pivot = pivot;
            }

            protected override void DoSort( uint start, uint end ) {

                if ( start >= end ) {
                    return;
                }

                var q = pivot switch {
                    Pivot.FIRST => PartitionFIRST( start, end ),
                    Pivot.LAST => PartitionLAST( start, end ),
                    Pivot.RANDOM => PartitionRANDOM( start, end ),
                    _ => throw new Exception( "Must not hit into here!" ),
                };

                if ( q != 0 ) { // unsigned int min value
                    Sort( start, q - 1 );
                }

                if ( q != 0xffffffff ) { // unsigned int max value
                    Sort( q + 1, end );
                }

            }

            private uint PartitionRANDOM( uint start, uint end ) {
                var randomIndex = new Random().Next( (int)start, (int)(end + 1) );
                Swap( (uint)randomIndex, end );
                return PartitionLAST( start, end );
            }

            private uint PartitionFIRST( uint start, uint end ) {
                var pivot = arr[ start ];
                var i = start;
                for ( uint j = start + 1; j <= end; j++ ) {
                    var compareResult = arr[ j ].CompareTo( pivot );
                    if ( ( order.Equals( Order.ASC ) && compareResult < 0 ) || ( order.Equals( Order.DESC ) && compareResult > 0 ) ) {
                        i++;
                        Swap( i, j );
                    }
                }
                Swap( i, start );
                return i;
            }

            private uint PartitionLAST( uint start, uint end ) {
                var pivot = arr[ end ];
                var i = start - 1;
                for ( uint j = start; j < end; j++ ) {
                    var compareResult = arr[ j ].CompareTo( pivot );
                    if ( ( order.Equals( Order.ASC ) && compareResult < 0 ) || ( order.Equals( Order.DESC ) && compareResult > 0 ) ) {
                        i++;
                        Swap( i, j );
                    }
                }
                Swap( i + 1, end );
                return i + 1;
            }
        }
    }
}

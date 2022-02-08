using System;

namespace Sortings {

    public static partial class Lib {

        public static void Heapsort<T>( T[] arr, Order order ) where T: IComparable<T> {
            DoPerformSort( new HeapsortInner<T>( arr, order ) );
        }

        private class HeapsortInner<T> : SortAbstract<T> where T: IComparable<T> {

            public HeapsortInner( T[] arr, Order order ) : base( arr, order ) { }

            protected override void DoSort( uint start, uint end ) {
                BuildHeap();
                uint heapSize = (uint)arr.Length;
                for ( int i = arr.Length - 1; i > 0; i-- ) {
                    Swap( 0, heapSize - 1 );
                    heapSize--;
                    Heapify( 0, heapSize );
                }                
            }

            private void BuildHeap() {
                for ( int i = arr.Length/2 - 1; i >= 0; i-- ) {
                    Heapify( (uint)i, (uint)arr.Length );
                }
            }

            private void Heapify( uint i, uint heapSize) {

                uint indexLeftChild = 2 * i + 1;
                uint indexRightChild = 2 * i + 2;

                uint indexTheBiggestOrSmallest;

                if (   indexLeftChild < arr.Length 
                    && indexLeftChild < heapSize 
                    && ( ( order.Equals( Order.ASC ) && arr[ indexLeftChild ].CompareTo( arr[ i ] ) > 0 ) || ( order.Equals( Order.DESC ) && arr[ indexLeftChild ].CompareTo( arr[ i ] ) <= 0 ) ) 
                   ) {
                    indexTheBiggestOrSmallest = indexLeftChild;
                } else {
                    indexTheBiggestOrSmallest = i;
                }

                if (   indexRightChild < arr.Length 
                    && indexRightChild < heapSize 
                    && ( ( order.Equals( Order.ASC ) && arr[ indexRightChild ].CompareTo( arr[ indexTheBiggestOrSmallest ] ) > 0 ) || ( order.Equals( Order.DESC ) && arr[ indexRightChild ].CompareTo( arr[ indexTheBiggestOrSmallest ] ) <= 0 ) ) 
                   ) {
                    indexTheBiggestOrSmallest = indexRightChild;
                }

                if ( indexTheBiggestOrSmallest == i ) {
                    return;
                }

                Swap( i, indexTheBiggestOrSmallest );
                Heapify( indexTheBiggestOrSmallest, heapSize );
            }
        }
    }
}

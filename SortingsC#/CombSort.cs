
using System;

namespace Sortings {

    public static partial class Lib {

        public static void CombSort<T>( T[] arr, Order order ) where T : IComparable<T> {
            DoPerformSort( new CombSortInner<T>( arr, order ) );
        }

        private class CombSortInner<T> : SortAbstract<T> where T: IComparable<T> {
            public CombSortInner( T[] arr, Order order ) : base( arr, order ) { }
        
            protected override void DoSort( uint start, uint end ) {

                int step = arr.Length - 1;
    
                while ( step >= 1 ) {
        
                    bool swap_occured = false;
        
                    for ( uint i = 0; i + step < arr.Length; i++ ) {
                        if (  order == Order.ASC && arr[ i ].CompareTo( arr[ i + step ] ) > 0 || ( order == Order.DESC && arr[ i ].CompareTo( arr[ i + step ] ) < 0 ) ) {
                            Swap( i, (uint)(i + step));
                            swap_occured = true;
                        }
                    }
                    if ( step == 1 && swap_occured ) {
                        continue;
                    }

                    step = (int)( step / shrinkFactor );
                }
            }
        }

        readonly static double shrinkFactor = 1 / ( 1 - Math.Exp( -( (Math.Sqrt( 5 ) + 1 ) / 2 ) ) ); // ≈ 1.2473309501039787
    }
}

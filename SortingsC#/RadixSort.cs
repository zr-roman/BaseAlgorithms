
using System;

namespace Sortings {

    public static partial class Lib {

        public static void RadixSort( int[] arr, Order order ) {
            DoPerformSort( new RadixSortInner( arr, order ) );
        }

        private class RadixSortInner : LinearTimeSort {
            
            public RadixSortInner( int[] arr, Order order ) : base( arr, order ) {
                var a = new int[ 1 ];
                _counting_sort = new CountingSortInner( a, order );
            }

            public override void SpecificSort( int[] arr, uint radix_param = 0 ) { // GoF template method 
                if ( arr == null || arr.Length == 0 ) {
                    return;
                }

                var greatest = GetGreatestKeyValue( arr );
                int num_of_digits = greatest <= 9 && greatest >= 0 ? 1 : (int)Math.Log10( (double)greatest ) + 1;

                var raise_of_ten = 1;

                for ( uint i = 0; i < num_of_digits; i++ ) {
                    _counting_sort.SpecificSort( arr, (uint)raise_of_ten );
                    raise_of_ten *= 10;
                }
            }
            
            private CountingSortInner _counting_sort;
        }
    }
}

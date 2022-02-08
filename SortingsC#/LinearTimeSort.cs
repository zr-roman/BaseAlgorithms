
using System.Collections.Generic;

namespace Sortings {

    public static partial class Lib {

        private abstract class LinearTimeSort : SortAbstract<int> {

            protected LinearTimeSort( int[] arr, Order order ) : base( arr , order ) { }

            protected override void DoSort( uint start, uint end ) {

                List<int> negatives = new List<int>();
                List<int> positives = new List<int>();

                for ( uint i = 0; i < arr.Length; i++ ) {
                    if ( arr[ i ] < 0 ) {
                        negatives.Add( -arr[ i ] );
                        continue;
                    }
                    positives.Add( arr[ i ] );
                }

                int[] negativeArray = negatives.ToArray();
                int[] positiveArray = positives.ToArray();

                SpecificSort( negativeArray );
                SpecificSort( positiveArray );
    
                int index = 0;
                if ( order == Order.ASC ) {
                    for ( int i = negativeArray.Length - 1; i >= 0; i-- ) {
                        arr[ index ] = -negativeArray[ i ];
                        index++;
                    }
                    for ( uint i = 0; i < positiveArray.Length; i++ ) {
                        arr[ index ] = positiveArray[ i ];
                        index++;
                    }
                } else {
                    for ( int i = positiveArray.Length - 1; i >= 0; i-- ) {
                        arr[ index ] = positiveArray[ i ];
                        index++;
                    }
                    for ( uint i = 0; i < negativeArray.Length; i++ ) {
                        arr[ index ] = -negativeArray[ i ];
                        index++;
                    }
                }
            }

            public abstract void SpecificSort( int[] arr, uint radix_param = 0 ); // GoF "Template Method"

        }
    }
}

namespace Sortings; 

public static partial class Lib {

    public static void BubbleSort<T>( T[] arr, Order order ) where T: IComparable<T> {
        DoPerformSort( new BubbleSortInner<T>( arr, order ) );
    }

    private class BubbleSortInner<T>( T[] arr, Order order ) : SortAbstract<T>( arr, order ) where T: IComparable<T> {
        protected override void DoSort( uint start, uint end ) {
            int work_area_size = arr.Length;
            while ( work_area_size > 1 ) {
                for ( uint i = 0; i + 1 < work_area_size; i++ ) {
                    if ( ( order == Order.ASC && arr[ i ].CompareTo( arr[ i + 1 ] ) > 0 ) || ( order == Order.DESC && arr[ i ].CompareTo( arr[ i + 1 ] ) < 0 ) )
                        Swap( i, i + 1 );
                }
                work_area_size--;
            }
        }
    }
}

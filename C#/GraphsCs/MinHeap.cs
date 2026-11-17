namespace GraphsCs;

public class MinHeap<T> where T : IComparable<T>
{
    public MinHeap( ICollection<T> coll ) {

        minHeap = new T[ coll.Count ];
        dic = [];
        var i = 0;

        foreach ( var item in coll ) {
            minHeap[ i ] = item;
            dic.Add( item, i );
            i++;
        }

        minHeapLastIndex = minHeap.Length - 1;
        BuildMinHeap();
    }
    public T ExtractMin() {

        var elm = minHeap[ 0 ];
            
        if ( minHeapLastIndex > 0 )
            SyncronizeDicAndHeap( 0, minHeapLastIndex );

        dic.Remove( minHeap[ minHeapLastIndex ] );

        minHeapLastIndex--;

        Heapify( 0 );

        return elm;
    }

    public void DecreaseKey( T obj )
    {
        if ( minHeapLastIndex == 0 )
            return;
            
        var i = dic[ obj ];

        while ( true ) {
            if ( i == 0 )
                break;

            int ip = ( i - 1 ) / 2; // iparent
            if ( minHeap[ ip ].CompareTo( minHeap[ i ] ) < 0  || minHeap[ ip ].CompareTo( minHeap[ i ] ) == 0 )
                break;

            SyncronizeDicAndHeap( ip, i );
            i = ip;
        }
    }

    public int GetSize() {
        return minHeapLastIndex + 1;
    }

    private void BuildMinHeap() {
        for (int i = minHeap.Length / 2 - 1; i >= 0; i--)
            Heapify( i );
    }

    private void Heapify( int i ) {
        
        var iL = 2 * i + 1;
        var iR = 2 * i + 2;
        int iSmallest;

        if ( iL <= minHeapLastIndex && minHeap[ iL ].CompareTo( minHeap[ i ] ) < 0 )
            iSmallest = iL;
        else
            iSmallest = i;

        if ( iR <= minHeapLastIndex && minHeap[ iR ].CompareTo( minHeap[ iSmallest ] ) < 0 )
            iSmallest = iR;

        if ( iSmallest == i )
            return;

        SyncronizeDicAndHeap( i, iSmallest );
        Heapify( iSmallest );
    }

    private void SyncronizeDicAndHeap( int i, int j )
    {
        dic[ minHeap[ i ] ] = j;
        dic[ minHeap[ j ] ] = i;
        Swap( minHeap, i, j );
    }

    private static void Swap(T[] arr, int i, int j) {
        ( arr[ j ], arr[ i ] ) = ( arr[ i ], arr[ j ] );
    }

    private readonly T[] minHeap;
    private int minHeapLastIndex;
    private readonly Dictionary<T, int> dic;
}

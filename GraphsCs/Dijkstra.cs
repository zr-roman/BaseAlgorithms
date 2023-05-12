
namespace GraphsCs {

    public static partial class Lib {
        
        public static void Dijkstra(int?[,] adjMatrix, Vertex[] vertices, Vertex s) {

            foreach (var v in vertices) {
                v.d = int.MaxValue;
                v.pi = null;
            }
            s.d = 0;

            var Q = new MinHeap<Vertex>( vertices );  // priority queue

            while ( Q.GetSize() > 0 ) {

                var u = Q.ExtractMin();

                int i = u.GetAdjId();
                
                // for each edge that is adjacent to u
                for (int j = 0; j < adjMatrix.GetLength(1); j++) {

                    var w = adjMatrix[i, j];

                    if ( w == null ) {
                        continue;
                    }
                    var v = vertices[ j ];

                    // relax edge and decrease key
                    if ( v.d > u.d + w ) {
                        v.d = u.d + w.Value;
                        v.pi = u;
                        Q.DecreaseKey( v );
                    }
                }
            }
        }
    }

    public class MinHeap<T> where T : IComparable<T>
    {
        public MinHeap(ICollection<T> coll) {

            minHeap = new T[coll.Count];
            dic = new Dictionary<T, int>();
            var i = 0;
            foreach (var item in coll) {
                minHeap[i] = item;
                dic.Add(item, i);
                i++;
            }
            minHeapLastIndex = minHeap.Length - 1;
            BuildMinHeap();
        }
        public T ExtractMin() {

            var elm = minHeap[0];
            
            if (minHeap.Length > 1) {
                SyncronizeDicAndHeap(0, minHeapLastIndex);
            }
            
            dic.Remove( minHeap[minHeapLastIndex]);

            minHeapLastIndex--;

            Heapify(0);

            return elm;
        }

        public void DecreaseKey(T obj)
        {
            if ( minHeap.Length == 1 ) {
                return;
            }
            
            var i = dic[ obj ];

            while ( true ) {
                if ( i == 0 ) {
                    break;
                }
                int ip = ( i - 1 ) / 2; // iparent
                if ( minHeap[ ip ].CompareTo(minHeap[ i ]) < 0  || minHeap[ip].CompareTo(minHeap[i]) == 0) {
                    break;
                }
                SyncronizeDicAndHeap( ip, i );
                i = ip;
            }
        }

        public int GetSize() {
            return minHeapLastIndex +1;
        }

        private void BuildMinHeap() {
            for (int i = minHeap.Length / 2 - 1; i >= 0; i--) {
                Heapify(i);
            }
        }

        private void Heapify( int i ) {
        
            var iL = 2 * i + 1;
            var iR = 2 * i + 2;
            int iSmallest;

            if ( iL <= minHeapLastIndex && minHeap[ iL ].CompareTo( minHeap[ i ]) < 0 ) { 
                iSmallest = iL;
            } else {
                iSmallest = i;
            }

            if ( iR <= minHeapLastIndex && minHeap[ iR ].CompareTo(minHeap[ iSmallest ]) < 0 ) {
                iSmallest = iR;
            }

            if ( iSmallest == i ) {
                return;
            }
            SyncronizeDicAndHeap(i, iSmallest);
            Heapify( iSmallest );
        }

        private void SyncronizeDicAndHeap(int i, int j)
        {
            dic[ minHeap[ i ] ] = j;
            dic[minHeap[ j ] ] = i;
            Swap(minHeap, i, j);
        }

        private void Swap(T[] arr, int i, int j) { 
            var tmp = arr[ i ]; 
            arr[ i ] = arr[ j ]; 
            arr[ j ] = tmp;
        }

        private readonly T[] minHeap;
        private int minHeapLastIndex;
        readonly Dictionary<T, int> dic;
    }
}


namespace GraphsCs {

    public static partial class Lib {
        
        public static ICollection<string> Dijkstra(int?[,] adjMatrix, Vertex[] vertices, Vertex s) {

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

            // creating the result
            var res = new List<string>();
            foreach (var v in vertices) {
                if (v.pi != null) {
                    res.Add(v.pi.GetAdjId() + Delimiter + v.GetAdjId());
                }
            }
            return res;
        }
    }
}

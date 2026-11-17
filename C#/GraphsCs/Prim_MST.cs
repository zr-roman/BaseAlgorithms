
namespace GraphsCs {

    public static partial class Lib { 

        public static ICollection<string> Prim_MST(IList<Vertex> vertices, int?[,] adjMatrix) { 

            var r = vertices.First();

            foreach ( var vertex in vertices ) {
                vertex.key = int.MaxValue;
                vertex.pi = null;
            }
            r.key = 0;

            var priorityQueue = new MinHeap<Vertex>(vertices);

            var extracted = new HashSet<Vertex>();

            while (priorityQueue.GetSize() > 0) {

                var u = priorityQueue.ExtractMin();
                extracted.Add(u);

                var i = u.GetAdjId();

                // for each edge v that is adjacent to u
                for (int j = 0; j < adjMatrix.GetLength(1); j++) {

                    var w = adjMatrix[i, j];

                    if ( w == null ) {
                        continue;
                    }
                    var v = vertices[ j ];

                    if ( extracted.Contains( v ) ) {
                        continue;
                    }
                    
                    if ( v.key > w ) {
                        v.key = w.Value;
                        v.pi = u;
                        priorityQueue.DecreaseKey( v );
                    }
                }
            }

            // creating the result
            var res = new List<string>();
            foreach (var v in vertices) {
                if (v.pi != null)   {
                    if (v.pi.GetAdjId() < v.GetAdjId()) {
                        res.Add(v.pi.GetAdjId() + Delimiter + v.GetAdjId());
                    } else {
                        res.Add(v.GetAdjId() + Delimiter + v.pi.GetAdjId());
                    }
                }
            }
            return res;
        }
    }
}

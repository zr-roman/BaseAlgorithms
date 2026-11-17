
namespace GraphsCs {

    public static partial class Lib {

        public static (bool, ICollection<string>) BellmanFord(int?[,] adjMatrix, Vertex[] vertices, Vertex s) {

            foreach (var v in vertices) {
                v.d = int.MaxValue;
                v.pi = null;
            }

            s.d = 0;

            for ( int i = 0; i < vertices.Length - 1 ; i++ ) {

                for (int k = 0; k < adjMatrix.GetLength(0); k++) {

                    for (int j = 0; j < adjMatrix.GetLength(1); j++) {

                        if ( adjMatrix[ k, j ] == null ) {
                            continue;
                        }

                        var u = vertices[ k ];
                        var v = vertices[ j ];
                        var w = adjMatrix[ k, j ];

                        Relax(u, v, w!.Value);
                    }
                }
            }

            for ( int k = 0; k < adjMatrix.GetLength(0); k++ ) {

                for ( int j = 0; j < adjMatrix.GetLength(1); j++ ) {

                    if ( adjMatrix[ k, j ] == null ) {
                        continue;
                    }

                    var u = vertices[ k ];
                    var v = vertices[ j ];
                    var w = adjMatrix[ k, j ];

                    if ( v.d > u.d + w ) {
                        (bool, ICollection<string>) value = (false, null);
                        return value; // negative cycle detected
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

            return (true, res); // negative cycle not detected
        }

        private static void Relax(Vertex u, Vertex v, int w) {
            if (v.d > u.d + w) {
                v.d = u.d + w;
                v.pi = u;
            }
        }

    }
}

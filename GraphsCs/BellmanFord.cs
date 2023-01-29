
namespace GraphsCs {

    public static partial class Lib {

        public static bool BellmanFord(int[,] adjMatrix, Vertex[] vertices, Vertex s) {

            foreach (var v in vertices) {
                v.d = int.MaxValue;
                v.pi = null;
            }

            s.d = 0;

            for ( int i = 0; i < vertices.Length - 1 ; i++ ) {

                for (int k = 0; k < adjMatrix.GetLength(0); k++) {

                    for (int j = 0; j < adjMatrix.GetLength(1); j++) {

                        if ( adjMatrix[ k, j ] == int.MaxValue ) {
                            continue;
                        }

                        var u = vertices[ k ];
                        var v = vertices[ j ];
                        var w = adjMatrix[ k, j ];

                        Relax(u, v, w);
                    }
                }
            }

            for ( int k = 0; k < adjMatrix.GetLength(0); k++ ) {

                for ( int j = 0; j < adjMatrix.GetLength(1); j++ ) {

                    if ( adjMatrix[ k, j ] == int.MaxValue ) {
                        continue;
                    }

                    var u = vertices[ k ];
                    var v = vertices[ j ];
                    var w = adjMatrix[ k, j ];

                    if ( v.d > u.d + w ) {
                        return false; // negative loop detected
                    }
                }
            }
            return true; // negative loop not detected
        }

        private static void Relax(Vertex u, Vertex v, int w) {
            if (v.d > u.d + w) {
                v.d = u.d + w;
                v.pi = u;
            }
        }

    }
}

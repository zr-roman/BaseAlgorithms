
namespace GraphsCs {

    public static partial class Lib {

        // All-Pairs Shortest Paths Floyd-Warshall algorithm, O(V^3), DP
        public static bool FloydWarshall( List<Vertex> vertices, int[,] adjMatrix, out string[,] paths, out int[,] distances ) {

            // Adjacency Matrix: 0 means self vertex, ∞ means no edge exists, negative and positive weights are both allowed
            //    a  b  c  d
            // a  0  3  6  15
            // b  ∞  0 -2  ∞
            // c  ∞  ∞  0  2
            // d  1  ∞  ∞  0

            distances = new int[ adjMatrix.GetLength( 0 ), adjMatrix.GetLength( 1 ) ];
            paths = new string[ adjMatrix.GetLength( 0 ), adjMatrix.GetLength( 1 ) ];

            for ( int i = 0; i < adjMatrix.GetLength( 0 ); i++ ) {
                for ( int j = 0; j < adjMatrix.GetLength( 1 ); j++ ) {

                    distances[ i, j ] = i == j ? 0 : adjMatrix[ i, j ];
                    if ( adjMatrix[ i, j ] != 0 && adjMatrix[ i, j ] != int.MaxValue ) {
                        paths[ i, j ] = vertices.FirstOrDefault( x => x.GetAdjId() == i )!.GetName();
                    } else {
                        paths[ i, j ] = "no path";
                    }
                }
            }   

            for ( int k = 0; k < vertices.Count; k++ ) {
                for ( int i = 0; i < vertices.Count; i++ ) {
                    for ( int j = 0; j < vertices.Count; j++ ) {

                        var new_dist = distances[ i, k ] + distances[ k, j ];

                        if ( distances[ i, j ] > new_dist ) {
                            distances[ i, j ] = new_dist;
                            paths[ i, j ] = paths[ k, j ];
                        }

                    }
                }
            }

            for ( int i = 0, j = 0; i < distances.GetLength( 0 ) && j < distances.GetLength( 1 ); i++, j++ ) {

                if ( distances[ i, j ] < 0 ) {
                    return false; // negative cycle detected
                }
            }

            return true; // negative cycle not detected

        }
    }
}


namespace GraphsCs {

    public static partial class Lib {

        // Ford-Fulkerson method, Edmonds-Karp algorithm O(V * E^2)
        public static int MaxFlow(int[,] adjMatrix, List<Vertex> vertices) {

            // adjacency matrix of capacities, source is the 0th element, and sink is the (n-1)th element
            //  s a b c d t
            //s - 3 - 2 - -
            //a 0 - 3 - - -
            //b - 0 - 0 - 2
            //c 0 - 3 - 3 -
            //d - - - 0 - 2
            //t - - 0 - 0 -
                        
            var max_flow = 0;

            while ( BFS( vertices, adjMatrix, out Dictionary<Vertex, Vertex> parent ) ) {

                // find min capacity of the found augmenting path
                var min_capacity = int.MaxValue;
                var curr = vertices.FirstOrDefault( x => x.GetAdjId() == vertices.Count - 1 );
                while ( curr!.GetAdjId() != 0 ) { // while curr is not sink
                    var par = parent[ curr! ];
                    var curr_capacity = adjMatrix[ par.GetAdjId(), curr.GetAdjId() ];
                    if ( min_capacity > curr_capacity ) {
                        min_capacity = curr_capacity;
                    }
                    curr = par;
                }

                // traverse from sink to source along the found augmenting path to correct residual capacity
                curr = vertices.FirstOrDefault( x => x.GetAdjId() == vertices.Count - 1 );
                while ( curr!.GetAdjId() != 0 ) { // while curr is not sink
                    var par = parent[ curr! ];
                    adjMatrix[ par.GetAdjId(), curr.GetAdjId() ] -= min_capacity;
                    adjMatrix[ curr.GetAdjId(), par.GetAdjId() ] += min_capacity;
                    curr = par;
                }

                max_flow += min_capacity;
            }
            return max_flow;
        }

        private static bool BFS( List<Vertex> vertices, int[,] adjMatrix, out Dictionary<Vertex, Vertex> parent ) {

            var s = vertices.FirstOrDefault( x => x.GetAdjId() == 0 ); // source

            if ( s == null ) {
                throw new Exception( "Source is not found in adjacency matrix" );
            }

            var augmenting_path_found = false;

            var frontier = new List<Vertex>() { s };
            parent = new Dictionary<Vertex, Vertex>() { { s, null! } };
            var level = new Dictionary<Vertex, int>() { { s, 0 } };
            var i = 1;

            while ( frontier.Count > 0 ) {

                var new_frontier = new List<Vertex>();
                foreach ( var u in frontier ) {

                    var row = u.GetAdjId();

                    for ( int j = 0; j < adjMatrix.GetLength( 1 ); j++ ) {

                        if ( adjMatrix[ row, j ] <= 0 ) {
                            continue;
                        }

                        var v = vertices.FirstOrDefault( x => x.GetAdjId() == j );
                        if ( v == null ) {
                            throw new Exception( "Error in adjacency matrix" );
                        }

                        if ( level.ContainsKey( v ) ) {
                            continue;
                        }
                            
                        level[ v ] = i;
                        parent[ v ] = u;
                        new_frontier.Add( v );
                    }
                }

                var sink = new_frontier.FirstOrDefault( x => x.GetAdjId() == vertices.Count - 1 );

                // if sink is reached it means that one augmenting path is found, so we don't need to proceed
                if ( sink != null ) {
                    augmenting_path_found = true;
                    break;
                }

                frontier = new_frontier;
                i++;
            }
            return augmenting_path_found;
        }
    }
}

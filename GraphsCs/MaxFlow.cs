
namespace GraphsCs {

    public static partial class Lib {

        // Ford-Fulkerson method, Edmonds-Karp algorithm
        public static int MaxFlow(int[,] adjMatrix, List<Vertex> vertices, Vertex s) {

            // adjacency matrix of capacities, source is the 0th element, and sink is the (n-1)th element
            //  s a b c d t
            //s - 3 - 2 - -
            //a 0 - 3 - - -
            //b - 0 - 0 - 2
            //c 0 - 3 - 3 -
            //d - - - 0 - 2
            //t - - 0 - 0 -

            var max_flow = 0;

            while (true) {

                var sink_in_new_frontier = false;

                var frontier = new List<Vertex>() { s };
                var parent = new Dictionary<Vertex, Vertex>() { { s, null! } };
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

                    // is sink is reached it means that one augmenting path is found, so we don't need to proceed
                    if ( sink != null ) {
                        sink_in_new_frontier = true;
                        break;
                    }

                    frontier = new_frontier;
                    i++;
                }

                // if sink was not in new_frontier it means that no more aumenting paths exist, so the algorithm stops here
                if ( !sink_in_new_frontier ) {
                    break;
                }

                // find min capacity of the found augmenting path
                var min_capacity = int.MaxValue;
                var curr = vertices.FirstOrDefault( x => x.GetAdjId() == vertices.Count - 1 );
                var par = parent[ curr! ];
                while ( curr != null && par != null ) {
                    var curr_cap = adjMatrix[ par.GetAdjId(), curr.GetAdjId() ];
                    if ( min_capacity > curr_cap ) {
                        min_capacity = curr_cap;
                    }
                    curr = par;
                    par = parent[ curr! ];
                }

                // traverse from sink to source along the found augmenting path to correct residual capacity
                curr = vertices.FirstOrDefault( x => x.GetAdjId() == vertices.Count - 1 );
                par = parent[ curr! ];
                while ( curr != null && par != null ) {
                    adjMatrix[ par.GetAdjId(), curr.GetAdjId() ] -= min_capacity;
                    adjMatrix[ curr.GetAdjId(), par.GetAdjId() ] += min_capacity;
                    curr = par;
                    par = parent[ curr! ];
                }

                max_flow += min_capacity;
            }
            return max_flow;
        }
    }
}

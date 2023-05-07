
namespace GraphsCs {

    public static partial class Lib {
        
        private static LinkedList<Vertex> list = new LinkedList<Vertex>();

        /// <summary>
        /// Depth first search (поиск в глубину)
        /// </summary>
        /// <param name="vertices">List of vertices (список вершин графа)</param>
        /// <param name="adjMatrix">Adjacency matrix (матрица смежности)</param>
        /// <returns> 
        /// Topologically sorted vertices (топологически отсортированные вершины графа)
        /// </returns> 
        public static LinkedList<Vertex> DFS( List<Vertex> vertices, bool[,] adjMatrix ) {

            var parent = new Dictionary<Vertex, Vertex>();

            foreach ( var v in vertices ) {
                if ( !parent.ContainsKey( v ) ) {
                    parent.Add( v, null! );
                    DFS_Visit( v, vertices, adjMatrix, parent );
                    list.AddFirst( v );
                }
            }
            return list;
        }

        private static void DFS_Visit( Vertex s, List<Vertex> vertices, bool[,] adjMatrix, Dictionary<Vertex, Vertex> parent ) {

            var adj_id = s.GetAdjId();

            for ( int j = 0; j < adjMatrix.GetLength( 1 ); j++ ) {

                if ( adjMatrix[ adj_id, j ] ) {

                    var v = vertices.FirstOrDefault( x => x.GetAdjId() == j );
                    if ( v == null ) {
                        throw new Exception( "Error in adjacency matrix" );
                    }
                    if ( !parent.ContainsKey( v ) ) {
                        parent.Add( v, s );
                        DFS_Visit( v, vertices, adjMatrix, parent );
                        list.AddFirst(v);
                    }
                }
            }
        }
    }
}

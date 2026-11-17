
namespace GraphsCs {
    
    public static partial class Lib {

        public static ICollection<string> Kruskal_MST(ICollection<Vertex> vertices, int?[,] adjMatrix) {

            var res = new List<string>();
            
            var A = new DisjointSet<Vertex>();

            foreach (var v in vertices){
                A.MakeSet(v);
            }

            var dicEdges = new Dictionary<string, int>(); // key: edge in format u{Delimiter}v, value: weight of the edge

            // create dictionary of edges
            for (int i = 0; i < adjMatrix.GetLength(0); i++) {
                for (int j = 0; j < adjMatrix.GetLength(1); j++) {
                    if (i == j || adjMatrix[i,j] == null) {
                        continue;
                    }
                    var key = i + Delimiter + j;
                    var keyReverse = j + Delimiter + i;
                    if ( !dicEdges.ContainsKey( key ) && !dicEdges.ContainsKey( keyReverse ) ) {
                        dicEdges.Add(key, adjMatrix[i,j]!.Value);
                    }
                } 
            }

            // Get sorted dictionary of edges
            dicEdges = dicEdges.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            var prevEdge = string.Empty;
            foreach ( var edge in dicEdges.Keys )
            {
                // check that the order is not changed during enumeration of the collection
                if (!string.IsNullOrEmpty(prevEdge) && dicEdges[edge] < dicEdges[prevEdge]) {
                    throw new Exception("Sorted order is broken!");
                }

                var arr = edge.Split(Delimiter);
                var adj_u = int.Parse(arr[0]);
                var adj_v = int.Parse(arr[1]);

                var vertex_u = vertices.First(x => x.GetAdjId() == adj_u);
                var vertex_v = vertices.First(x => x.GetAdjId() == adj_v);

                if ( A.FindSet(vertex_u) == A.FindSet(vertex_v) ) {
                    continue;
                }
                A.UnionSets(vertex_u, vertex_v);
                res.Add(edge);
                prevEdge = edge;
            }

            return res;
        }
    }

    // not optimal implementation, just a sketch    
    public class DisjointSet<T> {

        private readonly Dictionary<int, HashSet<T>> disjointSet = new();
        private int counter = 0;

        public void MakeSet(T obj)
        {
            disjointSet.Add(counter++, new HashSet<T> { obj });
        }

        public int FindSet( T obj ) {
            foreach (var key in disjointSet.Keys) {
                if (disjointSet[key].Contains(obj) ) {
                    return key;
                }
            }
            throw new Exception("must not go here");
        }

        public void UnionSets(T o1, T o2)
        {
            var key1 = FindSet(o1);
            var key2 = FindSet(o2);

            disjointSet[key1].UnionWith(disjointSet[key2]);

            disjointSet.Remove(key2);
        }
    }
}

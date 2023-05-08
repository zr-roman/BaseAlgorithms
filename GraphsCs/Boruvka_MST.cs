
namespace GraphsCs;

public static partial class Lib {

    private static object lockObject = new object();

    /// <summary>
    /// Boruvka MST algorithm
    /// </summary>
    /// <param name="vertices">List of vertices of a graph</param>
    /// <param name="adjMatrix">Adjacency matrix</param>
    /// <returns>List of edges of MST in format "u|v"</returns>
    public static ICollection<string> Boruvka_MST(List<Vertex> vertices, int?[,] adjMatrix) {

        var dic = new Dictionary<string, int>();

        foreach (var vertex in vertices) {
            _ = GetCheapestEdge(vertex, adjMatrix, dic);
        }
   
        while (true) {

            var set = new HashSet<int>();
            var exceptList = new HashSet<string>();
            var components = new List<HashSet<string>>();

            while (set.Count != vertices.Count) {

                var component = new HashSet<string>();
                var seed = dic.Keys.First(x => !exceptList.Contains(x)).Split('|');
                var frontier = new HashSet<int>() { int.Parse(seed[0]), int.Parse(seed[1]) };
                
                while (frontier.Any()) {

                    var new_frontier = new HashSet<int>();

                    foreach (var item in frontier) {

                        var res = dic.Keys.Where(x => !exceptList.Contains(x) && (x.Substring(0, x.IndexOf('|')) == item.ToString() || x.Substring(x.IndexOf('|') + 1) == item.ToString())).ToList();

                        foreach (var key in res) {

                            exceptList.Add(key);

                            var arr = key.Split('|');
                            var u = int.Parse(arr[0]);
                            var v = int.Parse(arr[1]);

                            component.Add(key);

                            if (!frontier.Contains(u) && !set.Contains(u)) {
                                new_frontier.Add(u);
                            }

                            if (!frontier.Contains(v) && !set.Contains(v)) {
                                new_frontier.Add(v);
                            }

                            set.Add(u);
                            set.Add(v);
                        }
                    }
                    frontier = new_frontier;
                }
                components.Add(component);
            }

            if (components.Count == 1 && components[0].Count == vertices.Count - 1) {
                return dic.Keys.ToArray();
            }

            var tasksList = new List<Task>();

            foreach (var component in components) {
                
                var collectionOfVertices = new HashSet<Vertex>();

                foreach (var edge in component) {
                    var vert1 = vertices.First(x => x.GetAdjId().ToString() == edge.Substring(0, edge.IndexOf('|')));
                    var vert2 = vertices.First(x => x.GetAdjId().ToString() == edge.Substring(edge.IndexOf('|') + 1));
                    collectionOfVertices.Add(vert1);
                    collectionOfVertices.Add(vert2);
                }

                tasksList.Add(
                    Task.Factory.StartNew(() => {
                        _ = GetCheapestEdge(collectionOfVertices, adjMatrix, dic);
                    }));
            }
            Task.WaitAll(tasksList.ToArray());
        }
    }

    private static (int, int)? GetCheapestEdge(ICollection<Vertex> vertices, int?[,] adjMatrix, Dictionary<string, int> dic) {
    
        lock (lockObject) {

            var weight = int.MaxValue;
            var adj_id_u = -1;
            var adj_id_v = -1;

            foreach (var vertex in vertices) {
            
                var i = vertex.GetAdjId();
            
                for (int j = 0; j < adjMatrix.GetLength(1); j++) {
           
                    if ( i == j || adjMatrix[i, j] == null) {
                        continue;
                    }

                    var t = j;
                    if ( vertices.Where(x => x.GetAdjId() == i).Any() && vertices.Where( x => x.GetAdjId() == t ).Any() ) {
                        continue;
                    }

                    if (adjMatrix[i, j] < weight) {
                        weight = adjMatrix[i, j]!.Value;
                        adj_id_v = j;
                        adj_id_u = i;
                    }
                }
            }

            if (adjMatrix[ adj_id_v, adj_id_u ] == null) {
                return null;
            }
        
            var k = adj_id_v + "|" + adj_id_u;
            var kReverse = adj_id_u + "|" + adj_id_v;
            if ( !dic.ContainsKey( k ) && !dic.ContainsKey( kReverse ) ) {
                dic.Add( k, weight );
            }
            return (adj_id_u, adj_id_v);
        }
    }

    private static (int, int)? GetCheapestEdge(Vertex vertex, int?[,] adjMatrix, Dictionary<string, int> dic)
    {
        var weight = int.MaxValue;
        var adj_id_u = vertex.GetAdjId();
        var adj_id_v = -1;

        for ( int j = 0; j < adjMatrix.GetLength( 1 ); j++ ) {
            
            if ( adj_id_u == j || adjMatrix[ adj_id_u, j ] == null ) {
                continue;
            }

            if (adjMatrix[ adj_id_u, j ] < weight) {
                weight = adjMatrix[ adj_id_u, j ]!.Value;
                adj_id_v = j;
            }

            // to avoid cycles
            var key = adj_id_v + "|" + adj_id_u;
            if ( dic.ContainsKey( key ) ) {
                if ( weight >= dic[ key ] ) {
                    return null;
                }
            }
        }

        var k = adj_id_v + "|" + adj_id_u;
        var kReverse = adj_id_u + "|" + adj_id_v;
        if (!dic.ContainsKey( k ) && !dic.ContainsKey( kReverse ) ) {
            dic.Add( k, weight );
        }

        return ( adj_id_u, adj_id_v );
    }
}

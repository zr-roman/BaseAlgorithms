
namespace GraphsCs;

public static partial class Lib {
        
    /// <summary>
    /// Boruvka MST algorithm
    /// </summary>
    /// <param name="vertices">List of vertices of a graph</param>
    /// <param name="adjMatrix">Adjacency matrix</param>
    /// <returns>List of edges of MST in format "u{Delimiter}v"</returns>
    public static ICollection<string> Boruvka_MST(List<Vertex> vertices, int?[,] adjMatrix) {

        var dicMSTedges = new Dictionary<string, int>(); // key - edge in format u{<Delimiter}v, e.g. u|v, value - weight of the edge
        
        // 1. Detecting the cheapest edge for each vertex of the graph

        foreach (var vertex in vertices) {
            _ = GetCheapestEdge(vertex, adjMatrix, dicMSTedges);
        }

        while (true) {

            // 2. Detecting components of the graph

            var components = GetComponentsOfTheGraph(vertices, dicMSTedges);

            if (components.Count == 1 && components[0].Count == vertices.Count - 1) {
                break;
            }

            // 3. Detecting cheapest edges between components of the graph

            SetCheapestEdgesBetweenComponentsOfTheGraph(components, vertices, adjMatrix, dicMSTedges);
        }
        return dicMSTedges.Keys.ToArray();
    }

    private static void SetCheapestEdgesBetweenComponentsOfTheGraph(List<HashSet<string>> components, List<Vertex> vertices, int?[,] adjMatrix, Dictionary<string, int> dicMSTedges) {

        foreach (var component in components) {

            var verticesOfComponent = new HashSet<Vertex>();

            foreach (var edge in component) {

                var adjId1 = int.Parse(edge.Substring(0, edge.IndexOf(Delimiter)));
                var adjId2 = int.Parse(edge.Substring(edge.IndexOf(Delimiter) + 1));

                var verts = vertices.FindAll(vertex => vertex.GetAdjId() == adjId1 || vertex.GetAdjId() == adjId2);

                foreach (var v in verts) {
                    verticesOfComponent.Add(v);
                }
            }
            _ = GetCheapestEdge(verticesOfComponent, adjMatrix, dicMSTedges);
        }
    }

    private static List<HashSet<string>> GetComponentsOfTheGraph(List<Vertex> vertices, Dictionary<string, int> dicMSTedges) { 

        var set = new HashSet<int>();
        var exceptList = new HashSet<string>();
        var components = new List<HashSet<string>>();

        while (set.Count != vertices.Count) {

            var component = new HashSet<string>();
            var seed = dicMSTedges.Keys.First(edge => !exceptList.Contains(edge)).Split(Delimiter);
            var frontier = new HashSet<int>() { int.Parse(seed[0]), int.Parse(seed[1]) };
                
            while (frontier.Any()) {

                var new_frontier = new HashSet<int>();

                foreach (var vertex in frontier) {

                    var edges = dicMSTedges.Keys.Where(edge => !exceptList.Contains(edge) && (edge.Substring(0, edge.IndexOf(Delimiter)) == vertex.ToString() || edge.Substring(edge.IndexOf(Delimiter) + 1) == vertex.ToString())).ToList();

                    foreach (var edge in edges) {

                        exceptList.Add(edge);

                        var arr = edge.Split(Delimiter);
                        var u = int.Parse(arr[0]);
                        var v = int.Parse(arr[1]);

                        component.Add(edge);

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
        return components;
    }

    /// <summary>
    /// Get cheapest edge for a component
    /// </summary>
    /// <param name="component">Component</param>
    /// <param name="adjMatrix">Adjacency matrix</param>
    /// <param name="dic">Dictionary of cheapest edges</param>
    /// <returns></returns>
    private static (int, int)? GetCheapestEdge(ICollection<Vertex> component, int?[,] adjMatrix, Dictionary<string, int> dicMSTedges) {
        
        var weight = int.MaxValue;
        var adj_id_u = -1;
        var adj_id_v = -1;

        foreach (var vertex in component) {
            
            var i = vertex.GetAdjId();
            
            for (int j = 0; j < adjMatrix.GetLength(1); j++) {
           
                if ( i == j || adjMatrix[i, j] == null) {
                    continue;
                }
                    
                if (component.Where(vertex => vertex.GetAdjId() == i).Any() && component.Where( vertex => vertex.GetAdjId() == j ).Any() ) {
                    continue;
                }

                // smaller -> bigger
                if (adjMatrix[i, j] < weight) {
                    weight = adjMatrix[i, j]!.Value;
                    if (i < j) {
                        adj_id_u = i;
                        adj_id_v = j;
                    } else {
                        adj_id_u = j;
                        adj_id_v = i;
                    }
                }
            }
        }

        if (adjMatrix[ adj_id_v, adj_id_u ] == null) {
            return null;
        }
        
        var k = adj_id_u + Delimiter + adj_id_v;
        var kReverse = adj_id_v + Delimiter + adj_id_u;
        if ( !dicMSTedges.ContainsKey( k ) && !dicMSTedges.ContainsKey( kReverse ) ) {
            dicMSTedges.Add( k, weight );
        }

        return (adj_id_u, adj_id_v);
    }

    /// <summary>
    /// Get cheapest edge for a vertex
    /// </summary>
    /// <param name="vertex">Vertex</param>
    /// <param name="adjMatrix">Adjacency matrix</param>
    /// <param name="dic">Dictionary of cheapest edges</param>
    /// <returns>Cheapest edge</returns>
    private static (int, int)? GetCheapestEdge(Vertex vertex, int?[,] adjMatrix, Dictionary<string, int> dicMSTedges)
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
            var key = adj_id_v + Delimiter + adj_id_u;
            if (dicMSTedges.ContainsKey( key ) ) {
                if ( weight >= dicMSTedges[ key ] ) {
                    return null;
                }
            }
        }

        // smaller -> bigger
        if (adj_id_u > adj_id_v) {
            var tmp = adj_id_u;
            adj_id_u = adj_id_v;
            adj_id_v = tmp;
        }

        var k = adj_id_u + Delimiter + adj_id_v;
        var kReverse = adj_id_v + Delimiter + adj_id_u;
        if (!dicMSTedges.ContainsKey( k ) && !dicMSTedges.ContainsKey(kReverse) ) {
            dicMSTedges.Add( k, weight );
        }

        return ( adj_id_u, adj_id_v );
    }

    private const string Delimiter = "|";
}

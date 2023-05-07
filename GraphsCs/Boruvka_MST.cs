
namespace GraphsCs;

public static partial class Lib {

    public static ICollection<string> Boruvka_MST(List<Vertex> vertices, int?[,] adjMatrix) {

        var dic = new Dictionary<string, int>();

        foreach (var vertex in vertices) {
            _ = GetCheapestEdge(vertex , adjMatrix, dic);
        }
                
        var set = new HashSet<int>();

        var exceptList = new HashSet<string>();

        while (true) {
            
            var seed = dic.Keys.First(x => !exceptList.Contains(x) ).Split('|');

            var frontier = new HashSet<int>() { int.Parse(seed[0]), int.Parse( seed[1] ) };
        
            while ( frontier.Any() ) {

                var new_frontier = new HashSet<int>();

                foreach (var item in frontier) {

                    var res = dic.Keys.Where( x => !exceptList.Contains( x ) && x.Contains( item.ToString()) ).ToList();

                    foreach ( var key in res ) {

                        exceptList.Add( key );

                        var arr = key.Split('|');
                        var u = int.Parse(arr[0]);
                        var v = int.Parse(arr[1]);

                        if ( !frontier.Contains( u ) && !set.Contains( u ) ) {
                            new_frontier.Add( u );
                        }

                        if ( !frontier.Contains( v ) && !set.Contains( v ) ) {
                            new_frontier.Add( v );
                        }

                        set.Add( u );
                        set.Add( v );
                    }
                }
                frontier = new_frontier;
            }

            if ( set.Count == vertices.Count ) {
                break;
            }

            var collectionOfVertices = vertices.Where(x => set.Contains( x.GetAdjId() )).ToList();
            _ = GetCheapestEdge(collectionOfVertices, adjMatrix, dic);
        }

        return dic.Keys.ToArray();
    }

    private static (int, int)? GetCheapestEdge(ICollection<Vertex> vertices, int?[,] adjMatrix, Dictionary<string, int> dic) {

        var weight = int.MaxValue;
        var adj_id_u = -1;
        var adj_id_v = -1;

        foreach (var vertex in vertices) {
            
            var i = vertex.GetAdjId();
            
            for (int j = 0; j < adjMatrix.GetLength(1); j++) {
            
                if ( i == j || adjMatrix[i, j] == null) {
                    continue;
                }

                if (dic.ContainsKey(i + "|" + j) || dic.ContainsKey(j + "|" + i)) {
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

        return ( adj_id_u, adj_id_v );

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

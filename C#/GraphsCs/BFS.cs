
namespace GraphsCs
{
    public static partial class Lib
    {
        public static void BFS(bool[,] adjMatrix, List<Vertex> vertices, Vertex s) {

            var frontier = new List<Vertex>() { s };
            var parent = new Dictionary<Vertex, Vertex>() { { s, null! } };
            var level = new Dictionary<Vertex, int>() { { s, 0 } };
            var i = 1;

            while (frontier.Count > 0) {
                var new_frontier = new List<Vertex>();
                foreach ( var u in frontier) {
                    
                    //visit(u);
                                                            
                    var row = u.GetAdjId();
                    for (int adj_id = 0; adj_id < adjMatrix.GetLength(1); adj_id++) {
                        if (!adjMatrix[row, adj_id]) {
                            continue;
                        }
                        var v = vertices.FirstOrDefault(x => x.GetAdjId() == adj_id);
                        if (v == null) {
                            throw new Exception("Error in adjacency matrix");
                        }
                        if (!level.ContainsKey(v)) {
                            level[v] = i;
                            parent[v] = u;
                            new_frontier.Add(v);
                        }
                    }
                }
                frontier = new_frontier;
                i++;
            }
        }
    }
}

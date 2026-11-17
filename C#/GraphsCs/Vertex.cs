/// <summary>
/// What a <see cref="GraphsCs.Vertex"/> is ordered by in MinHeap.
/// Declared in the global namespace: inside namespace GraphsCs, "Type.Prim" in the tests
/// would be ambiguous with System.Type (ImplicitUsings brings in "using System").
/// </summary>
public enum Type {
    /// <summary>By <see cref="GraphsCs.Vertex.d"/>: Dijkstra, Bellman-Ford.</summary>
    ShortestPath,
    /// <summary>By <see cref="GraphsCs.Vertex.key"/>: Prim's minimum spanning tree.</summary>
    Prim
}

namespace GraphsCs {

    /// <summary>
    /// A vertex of a graph. adj_id is the index of the vertex in the adjacency matrix
    /// and in the list of vertices passed to the algorithms.
    /// </summary>
    public class Vertex : IComparable<Vertex> {

        public Vertex(string name, string guid, int adj_id, Type type = Type.ShortestPath) {
            this.name = name;
            this.guid = guid;
            this.adjId = adj_id;
            this.type = type;
        }

        private readonly string name;
        private readonly string guid;
        private readonly int adjId;
        private readonly Type type;

        public string GetName() => name;
        public string GetGuid() => guid;
        public int GetAdjId() => adjId;

        /// <summary>Upper bound of the weight of a shortest path from the source (int.MaxValue = infinity).</summary>
        public int d = int.MaxValue;

        /// <summary>Key in Prim's algorithm: the minimum weight of an edge connecting the vertex to the tree.</summary>
        public int key = int.MaxValue;

        /// <summary>Predecessor in the shortest-path tree or in the minimum spanning tree.</summary>
        public Vertex? pi;

        /// <summary>Equality stays reference equality: vertices are used as dictionary keys.</summary>
        public int CompareTo(Vertex? other) {
            if (other is null) return 1;
            return type == Type.Prim ? key.CompareTo(other.key) : d.CompareTo(other.d);
        }

        public override string ToString() => $"{adjId} {name}";
    }
}

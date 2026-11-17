using GraphsCs;

namespace TestingForGraphsCs;

public static class GraphAlgorithmsTestsHelper
{
    public static Vertex[] CreateVertices(int count, Type type = Type.ShortestPath) =>
        Enumerable.Range(0, count)
            .Select(id => new Vertex($"v{id}", $"guid{id}", id, type))
            .ToArray();
}

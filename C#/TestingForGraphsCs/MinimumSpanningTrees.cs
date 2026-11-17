using GraphsCs;

namespace TestingForGraphsCs;

[TestClass]
public class MinimumSpanningTrees
{
    [TestMethod]
    public void MinimumSpanningTrees_HandleNegativeAndZeroWeightEdges()
    {
        int?[,] matrix =
        {
            { null, -2, 3 },
            { -2, null, 0 },
            { 3, 0, null }
        };

        var kruskal = Lib.Kruskal_MST(GraphAlgorithmsTestsHelper.CreateVertices(3), matrix);
        var prim = Lib.Prim_MST(GraphAlgorithmsTestsHelper.CreateVertices(3, Type.Prim), matrix);
        var boruvka = Lib.Boruvka_MST(GraphAlgorithmsTestsHelper.CreateVertices(3).ToList(), matrix);

        Assert.AreEqual(2, kruskal.Count);
        Assert.AreEqual(-2, TotalWeight(kruskal, matrix));
        Assert.AreEqual(2, prim.Count);
        Assert.AreEqual(-2, TotalWeight(prim, matrix));
        Assert.AreEqual(2, boruvka.Count);
        Assert.AreEqual(-2, TotalWeight(boruvka, matrix));
    }

    private static int TotalWeight(IEnumerable<string> edges, int?[,] matrix) =>
        edges.Sum(edge =>
        {
            var endpoints = edge.Split('|');
            var from = int.Parse(endpoints[0]);
            var to = int.Parse(endpoints[1]);
            return matrix[from, to]!.Value;
        });
}

using GraphsCs;
using NuGet.Frameworks;

namespace TestingForGraphsCs
{
    [TestClass]
    public class Dijkstra
    {
        private int repeat = 3001;

        [TestMethod]
        public void Test1() { 

            var p0 = new Vertex("Ivanov_s", "assasdsdf", 0);
            var p1 = new Vertex("Ivanov_t", "assasdsdf", 1);
            var p2 = new Vertex("Ivanov_y", "assasdsdf", 2);
            var p3 = new Vertex("Ivanov_x", "assasdsdf", 3);
            var p4 = new Vertex("Ivanov_z", "assasdsdf", 4);

            Vertex[] vertices = { p0, p1, p2, p3, p4 };

            int? M = null;

            // adjacency matrix // в матрице хранятся веса
            int?[,] adj_matrix = {
                        // 0  1  2  3  4
                        // s  t  y  x  z
                /* 0 s*/ { M, 10,5, M, M },
                /* 1 t*/ { M, M, 2, 1, M },
                /* 2 y*/ { M, 3, M, 9, 2 },
                /* 3 x*/ { M, M, M, M, 4 },
                /* 4 z*/ { 7, M, M, 6, M },
                   };

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Dijkstra(adj_matrix, vertices, p0);

                Assert.AreEqual(4, res.Count);
                Assert.IsTrue(res.Contains("2|1"));
                Assert.IsTrue(res.Contains("0|2"));
                Assert.IsTrue(res.Contains("1|3"));
                Assert.IsTrue(res.Contains("2|4"));
            }            
        }
    }

}

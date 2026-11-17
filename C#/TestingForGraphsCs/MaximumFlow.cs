using GraphsCs;

namespace TestingForGraphsCs
{
    [TestClass]
    public class MaximumFlow
    {
        [TestMethod]
        public void FordFulkersonEdmondsKarp1()
        {
            // example 1, max flow = 4
            var s = new Vertex("source", "source", 0);
            var a = new Vertex("a", "a", 1);
            var b = new Vertex("b", "b", 2);
            var c = new Vertex("c", "c", 3);
            var d = new Vertex("d", "d", 4);
            var t = new Vertex("sink", "sink", 5);

            var vertices = new List<Vertex>() { s, a, b, c, d, t };

            int[,] adjMatrix = new int[6, 6] {
                     //  s  a   b   c   d   t
                /*s*/ { -1, 3, -1,  2, -1, -1 }, // -1 means no edge exists
                /*a*/ { 0, -1,  3, -1, -1, -1 },
                /*b*/ { -1, 0, -1,  0, -1,  2 },
                /*c*/ { 0, -1,  3, -1,  3, -1 },
                /*d*/ { -1,-1, -1,  0, -1,  2 },
                /*t*/ { -1,-1,  0, -1,  0, -1 }
            };

            var res = Lib.MaxFlow(adjMatrix, vertices);
            Assert.AreEqual(4, res);
        }

        [TestMethod]
        public void FordFulkersonEdmondsKarp2()
        {
            // example 2, max flow = 5
            var s = new Vertex("source", "source", 0);
            var a = new Vertex("a", "a", 1);
            var b = new Vertex("b", "b", 2);
            var c = new Vertex("c", "c", 3);
            var d = new Vertex("d", "d", 4);
            var e = new Vertex("e", "e", 5);
            var t = new Vertex("sink", "sink", 6);

            var vertices = new List<Vertex>() { s, a, b, c, d, e, t };

            int[,] adjMatrix = new int[7, 7] {
                       //  s   a   b  c   d   e   t
                   /*s*/ { -1, 3, -1, 2, -1, -1, -1 }, // -1 means no edge exists
                   /*a*/ { 0, -1, 3, -1, -1, -1, -1 },
                   /*b*/ { -1, 0, -1, 0, -1,  1,  2 },
                   /*c*/ { 0, -1,  3, -1, 3, -1, -1 },
                   /*d*/ { -1,-1, -1, 0, -1, -1,  2 },
                   /*e*/ { -1,-1,  0, -1, -1,-1, 1 },
                   /*t*/ { -1,-1,  0, -1,  0, 0, -1 }
                };

            var res = Lib.MaxFlow(adjMatrix, vertices);
            Assert.AreEqual(5, res);
        }

        [TestMethod]
        public void FordFulkersonEdmondsKarp3()
        {
            //example 3, max flow = 5
            var a = new Vertex("source", "source", 0);
            var b = new Vertex("b", "b", 1);
            var c = new Vertex("c", "c", 2);
            var d = new Vertex("d", "d", 3);
            var e = new Vertex("e", "e", 4);
            var f = new Vertex("f", "f", 5);
            var g = new Vertex("sink", "sink", 6);

            var vertices = new List<Vertex>() { a, b, c, d, e, f, g };

            int[,] adjMatrix = new int[7, 7] {
                    //  a  b  c  d   e   f   g
                /*a*/ {-1, 3, 0, 3, -1, -1, -1 }, // -1 means no edge exists
                /*b*/ { 0,-1, 4,-1,  0, -1, -1 },
                /*c*/ { 3, 0,-1, 1,  2, -1, -1 },
                /*d*/ { 0, -1,0,-1,  2,  6, -1 },
                /*e*/ {-1, 1, 2, 0, -1, -1,  1 },
                /*f*/ {-1,-1,-1, 0, -1, -1,  9 },
                /*g*/ {-1,-1,-1, -1, 0,  0, -1 }
            };
            var res = Lib.MaxFlow(adjMatrix, vertices);
            Assert.AreEqual(5, res);
        }

    }
}

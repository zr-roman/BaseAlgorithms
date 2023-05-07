using GraphsCs;

namespace TestingForGraphsCs
{
    [TestClass]
    public class Boruvka_MST
    {
        [TestMethod]
        public void Test1() {

            var vertexA = new Vertex("A", Guid.NewGuid().ToString(), 0);
            var vertexB = new Vertex("B", Guid.NewGuid().ToString(), 1);
            var vertexC = new Vertex("C", Guid.NewGuid().ToString(), 2);
            var vertexD = new Vertex("D", Guid.NewGuid().ToString(), 3);
            var vertexE = new Vertex("E", Guid.NewGuid().ToString(), 4);
            var vertexF = new Vertex("F", Guid.NewGuid().ToString(), 5);
            var vertexG = new Vertex("G", Guid.NewGuid().ToString(), 6);
            var vertexH = new Vertex("H", Guid.NewGuid().ToString(), 7);

            var list = new List<Vertex> { vertexA, vertexB, vertexC, vertexD, vertexE, vertexF, vertexG, vertexH };

            int?[,] adj_Matrix = new int?[8, 8] { 
                        // A    B    C    D     E    F     G     H
                /* A*/  { null, 6,  null, 12, null, null, null, null },
                /* B*/  { 6,   null, 3,   9,  16,   null, null, null },
                /* C*/  { null, 3, null, null, 8,   null, null,  20 },
                /* D*/  { 12,   9, null, null, null,  7,    11,  null },
                /* E*/  { null, 16,  8,  null, null, null,  5,  13 },
                /* F*/  { null,null, null, 7,  null, null, 14,  null },
                /* G*/  { null,null, null,11,  5,    14,  null, 18 },
                /* H*/  { null, null, 20,null, 13,  null, 18,   null }
            };

            var res = Lib.Boruvka_MST(list, adj_Matrix);

            Assert.AreEqual(7, res.Count);
            Assert.IsTrue( res.Contains("1|0"));
            Assert.IsTrue(res.Contains("2|1"));   
            Assert.IsTrue(res.Contains("5|3"));
            Assert.IsTrue(res.Contains("6|4"));
            Assert.IsTrue(res.Contains("4|7"));
            Assert.IsTrue(res.Contains("4|2"));
            Assert.IsTrue(res.Contains("3|1"));
        }

        [TestMethod]
        public void Test2() {

            var vertexA = new Vertex("A", Guid.NewGuid().ToString(), 0);
            var vertexB = new Vertex("B", Guid.NewGuid().ToString(), 1);
            var vertexC = new Vertex("C", Guid.NewGuid().ToString(), 2);
            var vertexD = new Vertex("D", Guid.NewGuid().ToString(), 3);
            var vertexE = new Vertex("E", Guid.NewGuid().ToString(), 4);
            var vertexF = new Vertex("F", Guid.NewGuid().ToString(), 5);
            var vertexG = new Vertex("G", Guid.NewGuid().ToString(), 6);
            var vertexH = new Vertex("H", Guid.NewGuid().ToString(), 7);

            var list = new List<Vertex> { vertexA, vertexB, vertexC, vertexD, vertexE, vertexF, vertexG, vertexH };

            int?[,] adj_Matrix = new int?[8, 8] { 
                        // A    B    C    D   E    F     G     H
                /* A*/  { null, 8,  null, 4, null, 2,   null, null },
                /* B*/  { 8,   null, 14,  6, 22,  null, null, null },
                /* C*/  { null, 14, null, 26, 11, null, null,  null },
                /* D*/  { 4,   6,  26,  null, 3,  16,    5,  28 },
                /* E*/  { null, 22, 11, 3,  null, null,  25,  20 },
                /* F*/  { 2,  null , null, 16,null,null, 10,  null },
                /* G*/  { null,null,null, 5,  25,  10,  null, 27 },
                /* H*/  { null, null,null,28, 20,  null, 27,  null }
            };

            var res = Lib.Boruvka_MST(list, adj_Matrix);

            Assert.AreEqual(7, res.Count);
            Assert.IsTrue(res.Contains("5|0"));
            Assert.IsTrue(res.Contains("3|1"));
            Assert.IsTrue(res.Contains("4|2"));
            Assert.IsTrue(res.Contains("4|3"));
            Assert.IsTrue(res.Contains("3|6"));
            Assert.IsTrue(res.Contains("4|7"));
            Assert.IsTrue(res.Contains("3|0"));
        }

        [TestMethod]
        public void Test3() {

            var vertexSFO = new Vertex("SFO", Guid.NewGuid().ToString(), 0);
            var vertexLAX = new Vertex("LAX", Guid.NewGuid().ToString(), 1);
            var vertexDFW = new Vertex("DFW", Guid.NewGuid().ToString(), 2);
            var vertexORD = new Vertex("ORD", Guid.NewGuid().ToString(), 3);
            var vertexMIA = new Vertex("MIA", Guid.NewGuid().ToString(), 4);
            var vertexBWI = new Vertex("BWI", Guid.NewGuid().ToString(), 5);
            var vertexJFK = new Vertex("JFK", Guid.NewGuid().ToString(), 6);
            var vertexPVD = new Vertex("PVD", Guid.NewGuid().ToString(), 7);
            var vertexBOS = new Vertex("BOS", Guid.NewGuid().ToString(), 8);

            var list = new List<Vertex> { vertexSFO, vertexLAX, vertexDFW, vertexORD, vertexMIA, vertexBWI, vertexJFK, vertexPVD, vertexBOS };

            int?[,] adj_Matrix = new int?[9, 9] { 
                         // SFO  LAX  DFW  ORD  MIA  BWI  JFK  PVD  BOS
                /* SFO*/  { null,337, 1464,1846,null,null,null,null,2704 },
                /* LAX*/  { 337, null,1235,null,2342,null,null,null,null },
                /* DFW*/  {1464, 1235,null,802, 1121,null,1391,null,null },
                /* ORD*/  { 1846,null,802, null,null,621,740, 849, 867 },
                /* MIA*/  { null,2342,1121,null,null,946, 1090,null,1258 },
                /* BWI*/  { null,null,null,621,946, null, 184,null, null },
                /* JFK*/  { null,null,1391,740,1090,184, null,144, 187 },
                /* PVD*/  { null,null,null, 849,null,null,144,null,null },
                /* BOS*/  { 2704,null,null,867, 1258,null,187,null, null }
            };

            var res = Lib.Boruvka_MST(list, adj_Matrix);
            Assert.AreEqual(8, res.Count);
            Assert.IsTrue(res.Contains("1|0"));
            Assert.IsTrue(res.Contains("3|2"));
            Assert.IsTrue(res.Contains("5|3"));
            Assert.IsTrue(res.Contains("5|4"));
            Assert.IsTrue(res.Contains("6|5"));
            Assert.IsTrue(res.Contains("7|6"));
            Assert.IsTrue(res.Contains("6|8"));
            Assert.IsTrue(res.Contains("2|1"));
        }

    }
}

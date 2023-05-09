using GraphsCs;

namespace TestingForGraphsCs
{
    [TestClass]
    public class Boruvka_MST
    {
        private int repeat = 3001;

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

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Boruvka_MST(list, adj_Matrix);

                Assert.AreEqual(list.Count - 1, res.Count);
                Assert.IsTrue(res.Contains("1|0") || res.Contains("0|1"));
                Assert.IsTrue(res.Contains("2|1") || res.Contains("1|2"));
                Assert.IsTrue(res.Contains("5|3") || res.Contains("3|5"));
                Assert.IsTrue(res.Contains("6|4") || res.Contains("4|6"));
                Assert.IsTrue(res.Contains("4|7") || res.Contains("7|4"));
                Assert.IsTrue(res.Contains("4|2") || res.Contains("2|4"));
                Assert.IsTrue(res.Contains("3|1") || res.Contains("1|3"));
            }
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

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Boruvka_MST(list, adj_Matrix);

                Assert.AreEqual(list.Count - 1, res.Count);
                Assert.IsTrue(res.Contains("5|0") || res.Contains("0|5"));
                Assert.IsTrue(res.Contains("3|1") || res.Contains("1|3"));
                Assert.IsTrue(res.Contains("4|2") || res.Contains("2|4") );
                Assert.IsTrue(res.Contains("4|3") || res.Contains("3|4"));
                Assert.IsTrue(res.Contains("3|6") || res.Contains("6|3"));
                Assert.IsTrue(res.Contains("4|7") || res.Contains("7|4"));
                Assert.IsTrue(res.Contains("3|0") || res.Contains("0|3"));
            }
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

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Boruvka_MST(list, adj_Matrix);

                Assert.AreEqual(list.Count - 1, res.Count);
                Assert.IsTrue(res.Contains("1|0") || res.Contains("0|1"));
                Assert.IsTrue(res.Contains("3|2") || res.Contains("2|3"));
                Assert.IsTrue(res.Contains("5|3") || res.Contains("3|5"));
                Assert.IsTrue(res.Contains("5|4") || res.Contains("4|5"));
                Assert.IsTrue(res.Contains("6|5") || res.Contains("5|6"));
                Assert.IsTrue(res.Contains("7|6") || res.Contains("6|7"));
                Assert.IsTrue(res.Contains("6|8") || res.Contains("8|6"));
                Assert.IsTrue(res.Contains("2|1") || res.Contains("1|2"));
            }
        }

        [TestMethod]
        public void Test4() {
            var list = new List<Vertex>();
            for (int i = 0; i < 87; i++)
            {
                list.Add( new Vertex($"{i}", Guid.NewGuid().ToString(), i) );
            }

            int?[,] adj_Matrix = new int?[87, 87];

            setMatrix(adj_Matrix, 0, 1, 54);
            setMatrix(adj_Matrix, 0, 16, 38);
            setMatrix(adj_Matrix, 1, 2, 64);
            setMatrix(adj_Matrix, 1, 15, 75);
            setMatrix(adj_Matrix, 1, 56, 59);
            setMatrix(adj_Matrix, 2, 3, 57);
            setMatrix(adj_Matrix, 3, 4, 51);
            setMatrix(adj_Matrix, 4, 5, 95);
            setMatrix(adj_Matrix, 4, 8, 57);
            setMatrix(adj_Matrix, 5, 6, 66);
            setMatrix(adj_Matrix, 5, 11, 110);
            setMatrix(adj_Matrix, 6, 7, 63);
            setMatrix(adj_Matrix, 6, 11, 49);
            setMatrix(adj_Matrix, 8, 9, 76);
            setMatrix(adj_Matrix, 8, 12, 48);
            setMatrix(adj_Matrix, 9, 10, 70);
            setMatrix(adj_Matrix, 9, 11, 74);
            setMatrix(adj_Matrix, 10, 12, 129);
            setMatrix(adj_Matrix, 10, 24, 63);
            setMatrix(adj_Matrix, 12, 13, 76);
            setMatrix(adj_Matrix, 12, 15, 86);
            setMatrix(adj_Matrix, 13, 14, 67);
            setMatrix(adj_Matrix, 14, 24, 42);
            setMatrix(adj_Matrix, 16, 17, 64);
            setMatrix(adj_Matrix, 16, 30, 51);
            setMatrix(adj_Matrix, 17, 18, 80);
            setMatrix(adj_Matrix, 17, 27, 77);
            setMatrix(adj_Matrix, 18, 19, 72);
            setMatrix(adj_Matrix, 19, 20, 45);
            setMatrix(adj_Matrix, 20, 21, 76);
            setMatrix(adj_Matrix, 20, 25, 82);
            setMatrix(adj_Matrix, 20, 26, 47);
            setMatrix(adj_Matrix, 21, 22, 51);
            setMatrix(adj_Matrix, 23, 24, 88);
            setMatrix(adj_Matrix, 23, 25, 76);
            setMatrix(adj_Matrix, 23, 29, 48);
            setMatrix(adj_Matrix, 27, 28, 73);
            setMatrix(adj_Matrix, 28, 29, 62);
            setMatrix(adj_Matrix, 30, 31, 50);
            setMatrix(adj_Matrix, 30, 44, 101);
            setMatrix(adj_Matrix, 31, 32, 49);
            setMatrix(adj_Matrix, 31, 53, 75);
            setMatrix(adj_Matrix, 32, 33, 54);
            setMatrix(adj_Matrix, 32, 50, 61);
            setMatrix(adj_Matrix, 33, 34, 59);
            setMatrix(adj_Matrix, 34, 35, 41);
            setMatrix(adj_Matrix, 34, 38, 69);
            setMatrix(adj_Matrix, 35, 36, 58);
            setMatrix(adj_Matrix, 36, 37, 41);
            setMatrix(adj_Matrix, 36, 40, 41);
            setMatrix(adj_Matrix, 36, 43, 33);
            setMatrix(adj_Matrix, 38, 39, 65);
            setMatrix(adj_Matrix, 39, 40, 45);
            setMatrix(adj_Matrix, 39, 41, 74);
            setMatrix(adj_Matrix, 41, 42, 52);
            setMatrix(adj_Matrix, 42, 43, 33);
            setMatrix(adj_Matrix, 44, 45, 58);
            setMatrix(adj_Matrix, 45, 46, 80);
            setMatrix(adj_Matrix, 45, 53, 85);
            setMatrix(adj_Matrix, 45, 78, 65);
            setMatrix(adj_Matrix, 45, 85, 63);
            setMatrix(adj_Matrix, 46, 47, 60);
            setMatrix(adj_Matrix, 47, 48, 89);
            setMatrix(adj_Matrix, 47, 51, 100);
            setMatrix(adj_Matrix, 48, 49, 74);
            setMatrix(adj_Matrix, 50, 51, 60);
            setMatrix(adj_Matrix, 51, 52, 48);
            setMatrix(adj_Matrix, 51, 54, 93);
            setMatrix(adj_Matrix, 54, 55, 48);
            setMatrix(adj_Matrix, 56, 57, 94);
            setMatrix(adj_Matrix, 57, 58, 51);
            setMatrix(adj_Matrix, 57, 60, 77);
            setMatrix(adj_Matrix, 57, 74, 83);
            setMatrix(adj_Matrix, 58, 59, 62);
            setMatrix(adj_Matrix, 58, 64, 78);
            setMatrix(adj_Matrix, 58, 66, 66);
            setMatrix(adj_Matrix, 60, 61, 79);
            setMatrix(adj_Matrix, 60, 71, 54);
            setMatrix(adj_Matrix, 61, 62, 69);
            setMatrix(adj_Matrix, 61, 65, 45);
            setMatrix(adj_Matrix, 62, 63, 56);
            setMatrix(adj_Matrix, 62, 68, 50);
            setMatrix(adj_Matrix, 64, 65, 59);
            setMatrix(adj_Matrix, 66, 67, 81);
            setMatrix(adj_Matrix, 68, 69, 58);
            setMatrix(adj_Matrix, 69, 70, 63);
            setMatrix(adj_Matrix, 70, 86, 68);
            setMatrix(adj_Matrix, 71, 72, 54);
            setMatrix(adj_Matrix, 71, 76, 63);
            setMatrix(adj_Matrix, 72, 73, 56);
            setMatrix(adj_Matrix, 73, 80, 57);
            setMatrix(adj_Matrix, 73, 76, 70);
            setMatrix(adj_Matrix, 73, 81, 55);
            setMatrix(adj_Matrix, 74, 75, 55);
            setMatrix(adj_Matrix, 74, 76, 57);
            setMatrix(adj_Matrix, 75, 78, 52);
            setMatrix(adj_Matrix, 76, 77, 44);
            setMatrix(adj_Matrix, 77, 80, 66);
            setMatrix(adj_Matrix, 78, 79, 70);
            setMatrix(adj_Matrix, 79, 80, 45);
            setMatrix(adj_Matrix, 81, 82, 52);
            setMatrix(adj_Matrix, 81, 86, 54);
            setMatrix(adj_Matrix, 82, 83, 60);
            setMatrix(adj_Matrix, 83, 84, 58);
            setMatrix(adj_Matrix, 83, 85, 60);

            var res = Lib.Boruvka_MST(list, adj_Matrix);
            Assert.AreEqual(list.Count - 1, res.Count);

        }

        [TestMethod]
        public void Test5() {

            //https://www.geeksforgeeks.org/boruvkas-algorithm-greedy-algo-9/

            var vertexA = new Vertex("0", Guid.NewGuid().ToString(), 0);
            var vertexB = new Vertex("1", Guid.NewGuid().ToString(), 1);
            var vertexC = new Vertex("2", Guid.NewGuid().ToString(), 2);
            var vertexD = new Vertex("3", Guid.NewGuid().ToString(), 3);
            var vertexE = new Vertex("4", Guid.NewGuid().ToString(), 4);
            var vertexF = new Vertex("5", Guid.NewGuid().ToString(), 5);
            var vertexG = new Vertex("6", Guid.NewGuid().ToString(), 6);
            var vertexH = new Vertex("7", Guid.NewGuid().ToString(), 7);
            var vertexI = new Vertex("8", Guid.NewGuid().ToString(), 8);

            var list = new List<Vertex> { vertexA, vertexB, vertexC, vertexD, vertexE, vertexF, vertexG, vertexH, vertexI };

            int?[,] adj_Matrix = new int?[9, 9] { 
                        // 0    1     2    3     4     5     6     7   8
                /* 0*/  { null, 4,  null, null, null, null, null, 8, null },
                /* 1*/  { 4,   null, 8,   null, null, null, null, 11, null },
                /* 2*/  { null, 8,  null,  7,    null, 4,   null,  null, 2 },
                /* 3*/  { null, null, 7, null,   9,    14,  null, null, null },
                /* 4*/  { null, null,null, 9,  null,  10,    null, null, null },
                /* 5*/  { null,null, 4,    14,  10,  null,   2,   null, null  },
                /* 6*/  { null,null, null,null,  null, 2,  null, 1, 6 },
                /* 7*/  {   8,  11,  null,null, null,null,   1,   null, 7 },
                /* 8*/  { null, null, 2, null, null, null,   6,  7, null }
            };

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Boruvka_MST(list, adj_Matrix);

                Assert.AreEqual(8, res.Count);
                Assert.IsTrue(res.Contains("1|0") || res.Contains("0|1"));
                Assert.IsTrue(res.Contains("8|2") || res.Contains("2|8"));
                Assert.IsTrue(res.Contains("2|3") || res.Contains("3|2"));
                Assert.IsTrue(res.Contains("3|4") || res.Contains("4|3"));
                Assert.IsTrue(res.Contains("6|5") || res.Contains("5|6"));
                Assert.IsTrue(res.Contains("7|6") || res.Contains("6|7"));
                // this is because 2 edges {1,2} and {0,7} have the same weiht (8),
                // and competing threads may grab one or another edge
                Assert.IsTrue(res.Contains("2|1") || res.Contains("1|2") || res.Contains("0|7") || res.Contains("7|0"));
                Assert.IsTrue(res.Contains("5|2") || res.Contains("2|5"));
            }
        }

        [TestMethod]
        public void Test6() {

            var vertexA = new Vertex("A", Guid.NewGuid().ToString(), 0);
            var vertexB = new Vertex("B", Guid.NewGuid().ToString(), 1);
            var vertexC = new Vertex("C", Guid.NewGuid().ToString(), 2);
            var vertexD = new Vertex("D", Guid.NewGuid().ToString(), 3);
            var vertexE = new Vertex("E", Guid.NewGuid().ToString(), 4);
            var vertexF = new Vertex("F", Guid.NewGuid().ToString(), 5);
            var vertexG = new Vertex("G", Guid.NewGuid().ToString(), 6);
            var vertexH = new Vertex("H", Guid.NewGuid().ToString(), 7);

            int?[,] adj_Matrix = new int?[8, 8] { 
                        // A    B    C    D     E    F     G     H
                /* A*/  { null, 1,  null, 1, null, null, null, null },
                /* B*/  { 1,   null, 1,   1,  1,   null, null, null },
                /* C*/  { null, 1, null, null, 1,   null, null,  1 },
                /* D*/  { 1,   1, null, null,null,  1,    1,  null },
                /* E*/  { null, 1,  1,   null,null, null,  1,  1 },
                /* F*/  { null,null, null, 1,  null, null, 1,  null },
                /* G*/  { null,null, null,1,  1,    1,  null, 1 },
                /* H*/  { null, null, 1,null, 1,  null, 1,   null }
            };

            var list = new List<Vertex> { vertexA, vertexB, vertexC, vertexD, vertexE, vertexF, vertexG, vertexH };

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Boruvka_MST(list, adj_Matrix);

                Assert.AreEqual(list.Count - 1, res.Count);
                Assert.IsTrue(res.Contains("0|1"));
                Assert.IsTrue(res.Contains("1|2"));
                Assert.IsTrue(res.Contains("0|3"));
                Assert.IsTrue(res.Contains("1|4"));
                Assert.IsTrue(res.Contains("3|5"));
                Assert.IsTrue(res.Contains("3|6"));
                Assert.IsTrue(res.Contains("2|7"));
            }
        }

        [TestMethod]
        public void Test7() {

            var list = new List<Vertex>();
            for (int i = 0; i < 16; i++)
            {
                list.Add(new Vertex($"{i}", Guid.NewGuid().ToString(), i));
            }

            int?[,] adj_Matrix = new int?[16, 16];

            setMatrix(adj_Matrix, 0, 1, 54);
            setMatrix(adj_Matrix, 0, 13, 38);
            setMatrix(adj_Matrix, 1, 2, 64);
            setMatrix(adj_Matrix, 1, 14, 75);
            setMatrix(adj_Matrix, 1, 15, 59);
            setMatrix(adj_Matrix, 2, 3, 57);
            setMatrix(adj_Matrix, 3, 4, 51);
            setMatrix(adj_Matrix, 4, 5, 95);
            setMatrix(adj_Matrix, 4, 8, 57);
            setMatrix(adj_Matrix, 5, 6, 66);
            setMatrix(adj_Matrix, 5, 11, 110);
            setMatrix(adj_Matrix, 6, 7, 63);
            setMatrix(adj_Matrix, 6, 11, 49);
            setMatrix(adj_Matrix, 8, 9, 76);
            setMatrix(adj_Matrix, 8, 12, 48);
            setMatrix(adj_Matrix, 9, 10, 70);
            setMatrix(adj_Matrix, 9, 11, 74);
            setMatrix(adj_Matrix, 10, 12, 129);

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Boruvka_MST(list, adj_Matrix);

                Assert.AreEqual(list.Count - 1, res.Count);
                Assert.IsTrue( res.Contains("0|13"));
                Assert.IsTrue(res.Contains("0|1") );
                Assert.IsTrue( res.Contains("2|3"));
                Assert.IsTrue( res.Contains("3|4"));
                Assert.IsTrue( res.Contains("5|6"));
                Assert.IsTrue(res.Contains("6|11"));
                Assert.IsTrue(res.Contains("6|7") );
                Assert.IsTrue( res.Contains("8|12"));
                Assert.IsTrue(res.Contains("9|10"));
                Assert.IsTrue(res.Contains("1|14") );
                Assert.IsTrue(res.Contains("1|15") );
                Assert.IsTrue( res.Contains("1|2"));
                Assert.IsTrue( res.Contains("4|8"));
                Assert.IsTrue(res.Contains("9|11"));
                Assert.IsTrue(res.Contains("8|9"));
            }
        }

        [TestMethod]
        public void Test8() {

            var vertexA = new Vertex("A", Guid.NewGuid().ToString(), 0);
            var vertexB = new Vertex("B", Guid.NewGuid().ToString(), 1);
            var vertexC = new Vertex("C", Guid.NewGuid().ToString(), 2);
            var vertexD = new Vertex("D", Guid.NewGuid().ToString(), 3);
            var vertexE = new Vertex("E", Guid.NewGuid().ToString(), 4);
            var vertexF = new Vertex("F", Guid.NewGuid().ToString(), 5);
            var vertexG = new Vertex("G", Guid.NewGuid().ToString(), 6);
            var vertexH = new Vertex("H", Guid.NewGuid().ToString(), 7);            
            var vertexI = new Vertex("H", Guid.NewGuid().ToString(), 8);

            var list = new List<Vertex> { vertexA, vertexB, vertexC, vertexD, vertexE, vertexF, vertexG, vertexH, vertexI };

            int?[,] adj_Matrix = new int?[9, 9] { 
                        // A    B    C    D     E    F    G     H    I
                /* A*/  { null, 2, null, null, null, 5,   4,  null, null },
                /* B*/  { 2,   null, 2, null,  null, 4,  null,null, null },
                /* C*/  { null, 2, null, 3,    null, 4,  null,null, null },
                /* D*/  { null, null, 3, null,  2,   5,  null,null, null },
                /* E*/  { null, null, null,2, null,  3,  null,null,  4 },
                /* F*/  { 5,    4,   4,   5,    3,  null, 3,   5,    5 },
                /* G*/  { 4,  null, null, null, null,3,  null, 3,  null },
                /* H*/  { null,null,null,null,null, 5, 3,  null, 2 },
                /* I*/  { null, null,null,null,4,   5,null, 2,  null }
            };

            for (int i = 0; i < repeat; i++)
            {
                var res = Lib.Boruvka_MST(list, adj_Matrix);

                Assert.AreEqual(list.Count - 1, res.Count);
                Assert.IsTrue(res.Contains("1|0") || res.Contains("0|1"));
                Assert.IsTrue(res.Contains("1|2") || res.Contains("2|1"));
                Assert.IsTrue(res.Contains("4|3") || res.Contains("3|4"));
                Assert.IsTrue(res.Contains("4|5") || res.Contains("5|4"));
                Assert.IsTrue(res.Contains("5|6") || res.Contains("6|5"));
                Assert.IsTrue(res.Contains("8|7") || res.Contains("7|8"));
                Assert.IsTrue(res.Contains("3|2") || res.Contains("2|3"));
                Assert.IsTrue(res.Contains("6|7") || res.Contains("7|6"));
            }
        }

        private void setMatrix(int?[,] adj_Matrix, int i, int j, int w) {
            adj_Matrix[i, j] = w;
            adj_Matrix[j, i] = w;
        }
    }
}

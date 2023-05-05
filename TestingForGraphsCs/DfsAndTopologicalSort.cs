using GraphsCs;

namespace TestingForGraphsCs
{
    [TestClass]
    public class DfsAndTopologicalSort
    {

        [TestMethod]
        public void ExampleFromCLRS() {
            
            var watch = new Vertex("watch (часы)", Guid.NewGuid().ToString(), adj_id: 0);
            var socks = new Vertex("socks (носки)", Guid.NewGuid().ToString(), adj_id: 1);
            var boots = new Vertex("boots (туфли)", Guid.NewGuid().ToString(), adj_id: 2);
            var shirt = new Vertex("shirt (рубашка)", Guid.NewGuid().ToString(), adj_id: 3);
            var tie = new Vertex("tie (галстук)", Guid.NewGuid().ToString(), adj_id: 4);
            var jacket = new Vertex("jacket (пиджак)", Guid.NewGuid().ToString(), adj_id: 5);
            var underwear = new Vertex("underwear (трусы)", Guid.NewGuid().ToString(), adj_id: 6);
            var trousers = new Vertex("trousers (брюки)", Guid.NewGuid().ToString(), adj_id: 7);
            var belt = new Vertex("belt (ремень)", Guid.NewGuid().ToString(), adj_id: 8);

            var list = new List<Vertex> { shirt, tie, jacket, belt, watch, underwear, trousers, boots, socks };

            bool[,] adjMatrix = new bool[9, 9] {
            //         0      1      2      3      4      5      6      7      8 
            /* 0*/{  false, false, false, false, false, false, false, false, false},
            /* 1*/{  false, false, true, false, false, false, false, false, false},
            /* 2*/{  false, false, false, false, false, false, false, false, false},
            /* 3*/{  false, false, false, false, true, false, false, false, true},
            /* 4*/{  false, false, false, false, false, true, false, false, false},
            /* 5*/{  false, false, false, false, false, false, false, false, false},
            /* 6*/{  false, false, true, false, false, false, false, true, false},
            /* 7*/{  false, false, true, false, false, false, false, false, true},
            /* 8*/{  false, false, false, false, false, true, false, false, false},
        };

            var res = Lib.DFS(list, adjMatrix);

            Assert.AreEqual(9, res.Count);
            Assert.AreEqual(socks, res.First!.Value);
            Assert.AreEqual(underwear, res.First!.Next!.Value);
            Assert.AreEqual(trousers, res.First!.Next!.Next!.Value);
            Assert.AreEqual(boots, res.First!.Next!.Next!.Next!.Value);
            Assert.AreEqual(watch, res.First!.Next!.Next!.Next!.Next!.Value);
            Assert.AreEqual(shirt, res.First!.Next!.Next!.Next!.Next!.Next!.Value);
            Assert.AreEqual(belt, res.First!.Next!.Next!.Next!.Next!.Next!.Next!.Value);
            Assert.AreEqual(tie, res.First!.Next!.Next!.Next!.Next!.Next!.Next!.Next!.Value);
            Assert.AreEqual(jacket, res.First!.Next!.Next!.Next!.Next!.Next!.Next!.Next!.Next!.Value);
        }

    }
}

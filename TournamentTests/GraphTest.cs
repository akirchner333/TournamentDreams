using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest
{
    [TestClass]
    public class GraphTest
    {
        [TestMethod]
        public void TwoPointsTest()
        {
            var graph = new Graph(2);
            graph.AddConnection(0, 1);
            var matches = graph.FindMatches();
            // 0=0
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual((0, 1), matches[0]);
        }

        [TestMethod]
        public void ThreePointsTest()
        {
            var graph = new Graph(3);
            graph.AddConnection(0, 1);
            graph.AddConnection(1, 2);
            var matches = graph.FindMatches();
            // 0=0-0
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual((0, 1), matches[0]);
            var exposed = graph.Exposed();
            Assert.AreEqual(1, exposed.Count);
            Assert.AreEqual(2, exposed[0]);
        }

        [TestMethod]
        public void FourPointsTest()
        {
            var graph = new Graph(4);
            graph.AddConnection(0, 1);
            graph.AddConnection(1, 2);
            graph.AddConnection(2, 3);
            var matches = graph.FindMatches();
            // 0=0-0=0
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual((0, 1), matches[0]);
            Assert.AreEqual((2, 3), matches[1]);
        }

        [TestMethod]
        public void TreeTest()
        {
            var graph = new Graph(8);
            graph.AddConnection(0, 1);
            graph.AddConnection(1, 2);
            graph.AddConnection(2, 3);
            graph.AddConnection(1, 4);
            graph.AddConnection(4, 5);
            graph.AddConnection(4, 6);
            graph.AddConnection(6, 7);

            var matches = graph.FindMatches();
            Assert.AreEqual(4, matches.Count);
            Assert.AreEqual((0, 1), matches[0]);
            Assert.AreEqual((2, 3), matches[1]);
            Assert.AreEqual((4, 5), matches[2]);
            Assert.AreEqual((6, 7), matches[3]);
        }

        [TestMethod]
        public void TriangleTest()
        {
            /* 0 - 2 - 3
             * |  /
             * 1
             */
            var graph = new Graph(4);
            graph.AddConnection(0, 1);
            graph.AddConnection(0, 2);
            graph.AddConnection(1, 2);
            graph.AddConnection(2, 3);

            var matches = graph.FindMatches();
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual((0, 1), matches[0]);
            Assert.AreEqual((2, 3), matches[1]);
        }

        [TestMethod]
        public void PentagonTest()
        {
            var graph = new Graph(10);
            graph.AddConnection(0, 1);
            graph.AddConnection(0, 8);
            graph.AddConnection(1, 2);
            graph.AddConnection(2, 3);
            graph.AddConnection(3, 4);
            graph.AddConnection(3, 7);
            graph.AddConnection(4, 5);
            graph.AddConnection(5, 6);
            graph.AddConnection(6, 7);
            graph.AddConnection(7, 9);

            var matches = graph.FindMatches();
            Assert.AreEqual(5, matches.Count);
            Assert.AreEqual((0, 8), matches[0]);
            Assert.AreEqual((1, 2), matches[1]);
            Assert.AreEqual((3, 4), matches[2]);
            Assert.AreEqual((5, 6), matches[3]);
            Assert.AreEqual((7, 9), matches[4]);
        }
    }
}

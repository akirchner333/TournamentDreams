using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest
{
    [TestClass]
    public class SwissTest
    {
        [TestMethod]
        public void SortTest()
        {
            var swiss = new Swiss<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            var ranking = swiss.CurrentRound();
            Assert.AreEqual(4, ranking.Count);
            Assert.AreEqual((0, 1), ranking[0]);
            Assert.AreEqual((2, 3), ranking[1]);
            Assert.AreEqual((4, 5), ranking[2]);
            Assert.AreEqual((6, 7), ranking[3]);

            swiss.GameResults(Result.A_WINS);
            swiss.GameResults(Result.B_WINS);
            swiss.GameResults(Result.B_WINS);
            swiss.GameResults(Result.A_WINS);

            ranking = swiss.CurrentRound();
            Assert.AreEqual((1, 2), ranking[0]);
            Assert.AreEqual((4, 7), ranking[1]);
            Assert.AreEqual((0, 3), ranking[2]);
            Assert.AreEqual((5, 6), ranking[3]);
        }

        [TestMethod]
        public void DrawTest()
        {
            var swiss = new Swiss<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });

            swiss.GameResults(Result.A_WINS);
            swiss.GameResults(Result.DRAW);
            swiss.GameResults(Result.B_WINS);
            swiss.GameResults(Result.A_WINS);

            // Scores:
            // 2: 0, 5, 6
            // 1: 2, 3
            // 0: 1, 4, 7
            var ranking = swiss.CurrentRound();
            Console.WriteLine(String.Join(' ', ranking));
            Assert.AreEqual((1, 4), ranking[0]);
            Assert.AreEqual((7, 2), ranking[1]);
            Assert.AreEqual((3, 0), ranking[2]);
            Assert.AreEqual((5, 6), ranking[3]);
        }

        [TestMethod]
        public void ByeTest()
        {
            var swiss = new Swiss<int>(new int[5] { 0, 1, 2, 3, 4 });
            var ranking = swiss.CurrentRound();
            Assert.AreEqual(2, ranking.Count);
            Assert.AreEqual((0, 1), ranking[0]);
            Assert.AreEqual((2, 3), ranking[1]);

            swiss.GameResults(Result.A_WINS);
            swiss.GameResults(Result.B_WINS);

            ranking = swiss.CurrentRound();
            Assert.AreEqual(2, ranking.Count);
            Assert.AreEqual((1, 2), ranking[0]);
            Assert.AreEqual((3, 4), ranking[1]);
        }
    }
}

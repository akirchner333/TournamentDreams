using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest
{
    [TestClass]
    public class SingleEliminationTest
    {
        [TestMethod]
        public void TurnsInRoundTest()
        {
            var square = new SingleElimination<int>(new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 } );
            Assert.AreEqual(4, square.TurnsInRound());
            square.Round++;
            Assert.AreEqual(2, square.TurnsInRound());
            square.Round++;
            Assert.AreEqual(1, square.TurnsInRound());

            var notSquare = new SingleElimination<int>(new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            Assert.AreEqual(1, notSquare.TurnsInRound());
            notSquare.Round++;
            Assert.AreEqual(4, notSquare.TurnsInRound());
            notSquare.Round++;
            Assert.AreEqual(2, notSquare.TurnsInRound());
            notSquare.Round++;
            Assert.AreEqual(1, notSquare.TurnsInRound());
        }

        [TestMethod]
        public void ByesTest()
        {
            var notSquare = new SingleElimination<int>(new int[12] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120 });
            var firstRound = notSquare.CurrentRound();
            Assert.AreEqual(4, firstRound.Count());
            Assert.AreEqual((50, 120), firstRound[0]);
            Assert.AreEqual((60, 110), firstRound[1]);
            Assert.AreEqual((70, 100), firstRound[2]);
            Assert.AreEqual((80, 90), firstRound[3]);
        }

        [TestMethod]
        public void EliminationTest()
        {
            var elimination = new SingleElimination<int>(new int[4] { 10, 20, 30, 40 });
            Assert.AreEqual(0, elimination.Round);
            Assert.AreEqual(0, elimination.Turn);
            Assert.AreEqual((10, 40), elimination.NextGame());
            elimination.GameResults(Result.A_WINS);

            Assert.AreEqual(0, elimination.Round);
            Assert.AreEqual(1, elimination.Turn);
            Assert.AreEqual((20, 30), elimination.NextGame());
            elimination.GameResults(Result.B_WINS);

            Assert.AreEqual(1, elimination.Round);
            Assert.AreEqual(0, elimination.Turn);
            Assert.AreEqual((10, 30), elimination.NextGame());
            elimination.GameResults(Result.A_WINS);
            Assert.IsTrue(elimination.Finished());
        }
    }
}

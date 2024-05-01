using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest
{
    [TestClass]
    public class MultipleGamesTest
    {
        [TestMethod]
        public void NextGameTest()
        {
            var players = new int[] { 10, 20, 30, 40 };
            var tournament = new MultipleGames<int>(new RoundRobin<int>(players), 2);

            var (a, b) = tournament.NextGame();
            Assert.AreEqual(40, a);
            Assert.AreEqual(30, b);
            tournament.GameResults(Result.A_WINS);

            (a, b) = tournament.NextGame();
            Assert.AreEqual(30, a);
            Assert.AreEqual(40, b);
            tournament.GameResults(Result.A_WINS);

            (a, b) = tournament.NextGame();
            Assert.AreEqual(10, a);
            Assert.AreEqual(20, b);
            tournament.GameResults(Result.A_WINS);

            (a, b) = tournament.NextGame();
            Assert.AreEqual(20, a);
            Assert.AreEqual(10, b);
            tournament.GameResults(Result.A_WINS);

            Assert.AreEqual(1, tournament.Round);
            Assert.AreEqual(0, tournament.Turn);
        }
    }
}

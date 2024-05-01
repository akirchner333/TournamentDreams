using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest.Series
{
    [TestClass]
    public class RepeatGamesTest
    {
        [TestMethod]
        public void NextGameTest()
        {
            var tourney = new RepeatGames<int>(100, 200, 4);

            Assert.AreEqual((100, 200), tourney.NextGame());
            tourney.GameResults(Result.A_WINS);

            Assert.AreEqual((200, 100), tourney.NextGame());
            tourney.GameResults(Result.A_WINS);

            Assert.AreEqual((100, 200), tourney.NextGame());
            tourney.GameResults(Result.A_WINS);

            Assert.AreEqual((200, 100), tourney.NextGame());
            tourney.GameResults(Result.A_WINS);

            Assert.IsTrue(tourney.Finished());
        }

        [TestMethod]
        public void RecordTest()
        {
            var tourney = new RepeatGames<int>(100, 200, 4);

            tourney.GameResults(Result.A_WINS);
            tourney.GameResults(Result.A_WINS);
            tourney.GameResults(Result.DRAW);
            tourney.GameResults(Result.B_WINS);

            var (_player, record) = tourney.Players[0];
            Assert.AreEqual(2, record.Wins);
            Assert.AreEqual(1, record.Losses);
            Assert.AreEqual(1, record.Draws);

            (_player, record) = tourney.Players[1];
            Assert.AreEqual(1, record.Wins);
            Assert.AreEqual(2, record.Losses);
            Assert.AreEqual(1, record.Draws);
        }
    }
}

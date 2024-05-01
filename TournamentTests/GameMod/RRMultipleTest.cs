using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest.GameMod
{
    [TestClass]
    public class RRMultipleTest
    {
        [TestMethod]
        public void RecordTest()
        {
            var tourney = new RRMultiple<int>(new int[] { 100, 200, 300 }, 3);

            tourney.GameResults(Result.A_WINS);
            tourney.GameResults(Result.A_WINS);
            tourney.GameResults(Result.A_WINS);

            var players = tourney.RoundRobin.Players;
            var (player, record) = players[0];
            Assert.AreEqual(100, player);
            Assert.AreEqual(2, record.Wins);
            Assert.AreEqual(1, record.Losses);
            Assert.AreEqual(0, record.Draws);

            (player, record) = players[1];
            Assert.AreEqual(200, player);
            Assert.AreEqual(1, record.Wins);
            Assert.AreEqual(2, record.Losses);
            Assert.AreEqual(0, record.Draws);
        }
    }
}

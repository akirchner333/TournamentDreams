using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Rankings;

namespace TournamentTest.Rankings
{
    [TestClass]
    public class EloTest
    {
        [TestMethod]
        public void ProbabilityTest()
        {
            Assert.AreEqual(10d / 11d, Elo.Probability(400, 0));
            Assert.AreEqual(1d / 11d, Elo.Probability(0, 400));
            Assert.AreEqual(0.7597469266479578, Elo.Probability(1200, 1000));
            Assert.AreEqual(0.2402530733520421, Elo.Probability(1000, 1200));
        }

        [TestMethod]
        public void NewRatingsTest()
        {
            var (a, b) = Elo.NewRatings(1200, 1000, Result.A_WINS);
            Assert.AreEqual(1207.6880983472654, a);
            Assert.AreEqual(992.3119016527346, b);

            (a, b) = Elo.NewRatings(1200, 1000, Result.B_WINS);
            Assert.AreEqual(1175.6880983472654, a);
            Assert.AreEqual(1024.3119016527346, b);

            (a, b) = Elo.NewRatings(1200, 1000, Result.DRAW);
            Assert.AreEqual(1191.6880983472654, a);
            Assert.AreEqual(1008.3119016527346, b);
        }
    }
}

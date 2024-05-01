namespace TournamentTest
{
    [TestClass]
    public class RoundRobinTest
    {
        [TestMethod]
        public void FirstRoundTest()
        {
            var players = new int[] { 10, 20, 30, 40, 50, 60 };
            var tournament = new RoundRobin<int>(players);

            var (a, b) = tournament.NextGame();
            Assert.AreEqual(60, a);
            Assert.AreEqual(50, b);
            tournament.GameResults(Result.A_WINS);

            (a, b) = tournament.NextGame();
            Assert.AreEqual(10, a);
            Assert.AreEqual(40, b);
            tournament.GameResults(Result.A_WINS);

            (a, b) = tournament.NextGame();
            Assert.AreEqual(20, a);
            Assert.AreEqual(30, b);
            tournament.GameResults(Result.A_WINS);

            Assert.AreEqual(1, tournament.Round);
            Assert.AreEqual(0, tournament.Turn);
        }

        [TestMethod]
        public void PlayDistributionTest()
        {
            var players = new int[] { 10, 20, 30, 40, 50, 60, 70 };
            var tournament = new RoundRobin<int>(players);

            var playCount = new int[players.Length];
            while (!tournament.Finished())
            {
                var (a, b) = tournament.NextIndexes();
                Console.WriteLine($"{a} {b}");
                playCount[a]++;
                playCount[b]++;
                tournament.GameResults(Result.A_WINS);
            }

            for (var i = 0; i < players.Length; i++)
            {
                Assert.AreEqual(6, playCount[i], $"Player index {i}");
            }
        }

        [TestMethod]
        public void TurnsInRoundTest()
        {
            var evenTournament = new RoundRobin<int>(new int[4]);
            Assert.AreEqual(2, evenTournament.TurnsInRound());

            var oddTournament = new RoundRobin<int>(new int[5]);
            Assert.AreEqual(2, oddTournament.TurnsInRound());
        }

        [TestMethod]
        public void RankingTest()
        {
            var tournament = new RoundRobin<int>(new int[] { 1, 2, 3, 4, 5 });
            tournament.Players = new (int player, WinRecord record)[]
            {
                (1, new WinRecord() { Wins = 5, Draws = 2, Losses = 3}),
                (2, new WinRecord() { Wins = 0, Draws = 1, Losses = 9}),
                (3, new WinRecord() { Wins = 10, Draws = 0, Losses = 0}),
                (4, new WinRecord() { Wins = 2, Draws = 5, Losses = 3}),
                (5, new WinRecord() { Wins = 2, Draws = 0, Losses = 8})
            };

            var rankings = tournament.Rankings();
            Assert.AreEqual((3, 1, 20), rankings[0]);
            Assert.AreEqual((1, 2, 12), rankings[1]);
            Assert.AreEqual((4, 3,  9), rankings[2]);
            Assert.AreEqual((5, 4,  4), rankings[3]);
            Assert.AreEqual((2, 5,  1), rankings[4]);
        }

        [TestMethod]
        public void PlaceTest()
        {
            var tournament = new RoundRobin<int>(new int[8] { 11, 21, 31, 41, 51, 61, 71, 81 });
            tournament.Players = new (int player, WinRecord record)[]
            {
                (11, new WinRecord() { Wins = 10, Draws = 0, Losses = 0}),
                (21, new WinRecord() { Wins = 10, Draws = 0, Losses = 0}),
                (31, new WinRecord() { Wins = 5, Draws = 0, Losses = 5}),
                (41, new WinRecord() { Wins = 3, Draws = 4, Losses = 3}),
                (51, new WinRecord() { Wins = 1, Draws = 0, Losses = 9}),
                (61, new WinRecord() { Wins = 0, Draws = 0, Losses = 10}),
                (71, new WinRecord() { Wins = 0, Draws = 0, Losses = 10}),
                (81, new WinRecord() { Wins = 0, Draws = 0, Losses = 10})
            };

            var rankings = tournament.Rankings();
            Assert.AreEqual((11, 1, 20), rankings[0]);
            Assert.AreEqual((21, 1, 20), rankings[1]);
            Assert.AreEqual((31, 3, 10), rankings[2]);
            Assert.AreEqual((41, 3, 10), rankings[3]);
            Assert.AreEqual((51, 5,  2), rankings[4]);
            Assert.AreEqual((61, 6,  0), rankings[5]);
            Assert.AreEqual((71, 6,  0), rankings[6]);
            Assert.AreEqual((81, 6,  0), rankings[7]);
        }
    }
}
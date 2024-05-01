using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament
{
    // Pairs players based on score - players face off against whoever has the same score as them
    // Players cannot play the same person more than once
    // TODO: When there are byes players receive extra wins for the bye rounds, meaning their final score is not completely accurate
    //          -Do something about that?
    public class Swiss<Player> : AbstractTournament<Player>
    {
        public List<(int, int)> ThisRound = new List<(int, int)>();
        private bool[,] _playHistory;
        //private int _byeId = -1;

        public Swiss(Player[] players) : base(players)
        {
            var count = players.Length % 2 == 0 ? Players.Length : Players.Length + 1;
            _playHistory = new bool[count, count];

            SetupRound();
        }

        public override void SetupRound()
        {
            ThisRound.Clear();

            var records = Players.Select((player, index) => (player.record, index));
            var byeId = -1;
            if (Players.Length % 2 == 1)
            {
                byeId = Players.Length;
                records = records.Append((new WinRecord(), byeId));
            }

            var ranked = records
                .OrderBy(p => p.record.Scores)
                .GroupBy(p => p.record.Scores)
                .Select(p => p.Select(p2 => p2.index))
                .ToList();

            var sparePlayers = Enumerable.Empty<int>();
            for (var i = 0; i < ranked.Count; i++)
            {
                var cohort = sparePlayers.Concat(ranked[i]).ToList();
                var graph = new Graph(cohort.Count);
                for (var j = 0; j < cohort.Count; j++)
                {
                    for (var k = 0; k < cohort.Count; k++)
                    {
                        if (CanPlay(cohort[j], cohort[k]))
                            graph.AddConnection(j, k);
                    }
                }

                var matches = graph.FindMatches();
                foreach (var (x, y) in matches)
                {
                    var (a, b) = (cohort[x], cohort[y]);
                    if(a == byeId || b == byeId)
                    {
                        var playerId = a == byeId ? b : a;
                        Players[playerId].record.Wins++;
                    }
                    else
                    {
                        ThisRound.Add((a, b));
                    }
                    _playHistory[a, b] = true;
                    _playHistory[b, a] = true;
                }
                sparePlayers = graph.Exposed().Select(i => cohort[i]);
            }
        }

        public bool CanPlay(int a, int b)
        {
            return !_playHistory[a, b];
        }

        public override List<(Player, Player)> CurrentRound()
        {
            return ThisRound.Select(indexes =>
            {
                var (a, b) = indexes;
                return (Players[a].player, Players[b].player);
            }).ToList();
        }

        public override void GameResults(Result result)
        {
            var (a, b) = NextIndexes();

            base.GameResults(result);
        }

        public override (int, int) TurnIndexes(int turn)
        {
            return ThisRound[turn];
        }

        public override int TurnsInRound()
        {
            return Players.Length / 2;
        }

        public override bool Finished()
        {
            return Round > Math.Log2(Players.Length);
        }
    }
}

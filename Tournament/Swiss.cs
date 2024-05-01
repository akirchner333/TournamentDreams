using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament
{
    // Pairs players based on score - players face off against whoever has the same score as them
    // Players cannot play the same person more than once
    public class Swiss<Player> : AbstractTournament<Player>
    {
        public List<(int, int)> ThisRound = new List<(int, int)>();
        private bool[,] _playHistory;
        private int _byeId = -1;

        public Swiss(Player[] players) : base(players)
        {
            var count = Players.Length;
            if(players.Length % 2 == 1)
            {
                count++;
                _byeId = Players.Length;
            }
            _playHistory = new bool[count, count];

            SetupRound();
        }

        public override void SetupRound()
        {
            ThisRound.Clear();

            var records = Players.Select((player, index) => (player.record, index));
            // If there are an odd number of players, insert a fake player to serve as the bye
            //var byeId = -1;
            if (Players.Length % 2 == 1)
            {
                //byeId = Players.Length;
                // the bye has never won anything, so they get a completely new WinRecord
                records = records.Append((new WinRecord(), _byeId));
            }

            // Divides the players up into groups based on their current score - i.e. everyone with a score 2 together, everyone with 1, etc
            var ranked = records
                .OrderBy(p => p.record.Scores)
                .GroupBy(p => p.record.Scores)
                .Select(p => p.Select(p2 => p2.index))
                .ToList();

            // If a player cannot be assigned within their cohort, they'll be assigned to the nearest one/the next one
            var sparePlayers = Enumerable.Empty<int>();
            for (var i = 0; i < ranked.Count; i++)
            {
                var cohort = sparePlayers.Concat(ranked[i]).ToList();
                var graph = new Graph(cohort.Count);
                for (var j = 0; j < cohort.Count; j++)
                {
                    for (var k = 0; k < cohort.Count; k++)
                    {
                        // if two players can play each other, connect them on the graph
                        if (CanPlay(cohort[j], cohort[k]))
                            graph.AddConnection(j, k);
                    }
                }

                var matches = graph.FindMatches();
                foreach (var (x, y) in matches)
                {
                    var (a, b) = (cohort[x], cohort[y]);
                    if(a == _byeId || b == _byeId)
                    {
                        // If a player is assigned the bye, they don't get added to the round and automatically win
                        var playerId = a == _byeId ? b : a;
                        Players[playerId].record.Wins++;
                    }
                    else
                    {
                        ThisRound.Add((a, b));
                    }
                    _playHistory[a, b] = true;
                    _playHistory[b, a] = true;
                }
                // Any remaining players (who will be exposed nodes on the graph) are added to SparePlayers to be assigned next round
                sparePlayers = graph.Exposed().Select(i => cohort[i]);
            }
        }

        // Two players can play so long as they haven't played each other before
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

        // TO DO
        // When calculating rankings, check if the player has played _byeId
        // If so, subtract 2 from their score
    }
}

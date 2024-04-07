using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament
{
    // From FIDE Rules C.04.1 Basic Rules for Swiss Systems (https://handbook.fide.com/chapter/C0401)
    // X Two players SHALL NOT player each other more than once
    // _ For each player, the difference between white games played and black games played SHALL NOT be greater than 2 or less than negative 2
    // _ Players SHALL NOT play the same color three times in a row
    // _ Players SHOULD be paired to others with the same score
    // _ Players SHOULD be given the color they've played less
    //      - Failing that, they SHOULD be given the color that alternates from the last one they played

    // You know, a lot of these requirements do not apply to me
    public enum SideReq
    {
        WHITE_MAND,
        WHITE_PREF,
        BLACK_MAND,
        BLACK_PREF
    }

    public static class SideReqExtensions
    {
        public static bool Mandatory(this SideReq req)
        {
            return req == SideReq.WHITE_MAND || req == SideReq.BLACK_MAND;
        }

        public static bool Color(this SideReq req)
        {
            return req == SideReq.WHITE_MAND || req == SideReq.WHITE_PREF;
        }

        public static bool Matching(this SideReq req1, SideReq req2)
        {
            if (!req1.Mandatory() || !req2.Mandatory())
                return true;

            if (req1.Color() != req2.Color())
                return true;

            return false;
        }
    }

    public struct PlayHistory
    {
        public int WhitePlays { get; set; }
        public int BlackPlays { get; set; }
        public bool[] PrevPlay { get; set; }
        public int Id { get; set; }
        public List<int> Opponents { get; private set; }

        public PlayHistory(int id)
        {
            Id = id;
            WhitePlays = 0;
            BlackPlays = 0;
            PrevPlay = new bool[2];
            Opponents = new List<int>();
        }

        public void AddPlay(bool side, int OppId)
        {
            if(side)
                WhitePlays++;
            else
                BlackPlays++;

            PrevPlay[1] = PrevPlay[0];
            PrevPlay[0] = side;

            Opponents.Add(OppId);
        }

        public SideReq NextSide()
        {
            if (WhitePlays + BlackPlays < 2)
                return SideReq.WHITE_PREF;

            if (PrevPlay[0] && PrevPlay[1])
                return SideReq.BLACK_MAND;
            if (!PrevPlay[0] && !PrevPlay[1])
                return SideReq.WHITE_MAND;

            if (WhitePlays - BlackPlays >= 2)
                return SideReq.BLACK_MAND;
            if (BlackPlays - WhitePlays >= 2)
                return SideReq.WHITE_MAND;

            if (PrevPlay[0])
                return SideReq.BLACK_PREF;
            return SideReq.WHITE_PREF;
        }

        public bool CanPlay(PlayHistory opponent)
        {
            if (Opponents.Contains(opponent.Id))
                return false;

            return NextSide().Matching(opponent.NextSide());
        }
    }


    public class Swiss<Player> : AbstractTournament<Player>
    {
        public PlayHistory[] History { get; private set; }
        public List<(int, int)> ThisRound = new List<(int, int)>();

        public Swiss(Player[] players) : base(players)
        {
            History = new PlayHistory[players.Length];
            for(var i = 0; i < players.Length; i++)
            {
                History[i] = new PlayHistory(i);
            }
            SetupRound();
        }

        // I feel like I could give more weight to preferences
            // If a player has multiple valid matches, they should be matched in a manner to maximize preferences fulfilled
        public override void SetupRound()
        {
            ThisRound.Clear();
            var ranked = Players
                .Reverse()
                .Select((player, index) => (player.record, index))
                .GroupBy(p => -p.record.Scores)
                .Select(p => p.Select(p2 => p2.index).Reverse())
                .ToList();

            var sparePlayers = new List<int>();
            var byeTaken = Players.Length % 2 == 0;
            for(var i = 0; i < ranked.Count; i++)
            {
                var cohort = sparePlayers.Concat(ranked[i]).ToList();
                var graph = new Graph(cohort.Count + 1);
                var byeId = cohort.Count;
                for(var j = 0; j < cohort.Count; j++)
                {
                    if (!byeTaken && !History[cohort[j]].Opponents.Contains(-1))
                    {
                        graph.AddConnection(j, byeId);
                    }

                    for(var k = 0; k < cohort.Count; k++)
                    {
                        if (j != k && History[cohort[j]].CanPlay(History[cohort[k]]))
                            graph.AddConnection(j, k);
                    }
                }

                var matches = graph.FindMatches();
                foreach(var (a, b) in matches)
                {
                    if(a == byeId || b == byeId)
                    {
                        byeTaken = true;
                        var p = a == byeId ? b : a;
                        // When a player recieves a bye it's as if they win
                        Players[p].record.Wins++;
                        History[p].Opponents.Add(-1);
                        continue;
                    }
                    var (white, black) = SideDecider(cohort[a], cohort[b]);
                    ThisRound.Add((white, black));
                    History[white].AddPlay(true, black);
                    History[black].AddPlay(false, white);
                }
                sparePlayers = graph.Exposed();
                sparePlayers.Remove(byeId);
            }

            ThisRound.Reverse();
        }

        public (int, int) SideDecider(int a, int b)
        {
            if (History[a].NextSide().Mandatory() == false && History[b].NextSide().Mandatory() == true)
                return SideDecider(b, a);

            switch (History[a].NextSide())
            {
                case SideReq.WHITE_MAND:
                    return (a, b);
                case SideReq.BLACK_MAND:
                    return (b, a);
                case SideReq.WHITE_PREF:
                    return (a, b);
                case SideReq.BLACK_PREF:
                    return (b, a);
                default:
                    return (a, b);
            }
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

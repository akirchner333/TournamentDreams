using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Tournament
{
    // When applied to a number of players that aren't a power of 2, applies byes in the first round to eliminate down too the nearest power of two
    // As discussed here: https://en.wikipedia.org/wiki/Bye_(sports)#Elimination_tournaments
    // I've got other ideas for manners in which byes could be distributed, but this seems like a good method to start with

    // This will likely start acting weird if you put draws into it
    public class SingleElimination<Player> : AbstractTournament<Player>
    {
        private int _roundUp;
        private bool _squarePlayers;
        public List<int> RemainingPlayers = new List<int>();
        public SingleElimination(Player[] players) : base(players)
        {
            _squarePlayers = BitOperations.IsPow2(Players.Length);
            _roundUp = (int)(BitOperations.RoundUpToPowerOf2((uint)Players.Length));
            for(var i = 0; i < Players.Length; i++)
            {
                RemainingPlayers.Add(i);
            }
        }

        public override int TurnsInRound()
        {
            if(Round == 0 && !_squarePlayers)
            {
                return Players.Length - (_roundUp >> 1);
            }

            return _roundUp / (int)Math.Pow(2, Round + 1);
        }

        public override bool Finished()
        {
            return Round >= Math.Log(_roundUp, 2);
        }

        public override void SetupRound()
        {
            RemainingPlayers.RemoveAll(i => Players[i].record.Losses > 0);
        }

        public override List<(Player, Player)> CurrentRound()
        {
            var list = new List<(Player, Player)>();
            for(var i = 0; i < TurnsInRound(); i++)
            {
                var (a, b) = TurnIndexes(i);
                list.Add((
                    Players[a].player,
                    Players[b].player
                ));
            }

            return list;
        }

        public override (int, int) TurnIndexes(int turn)
        {
            var byes = RemainingPlayers.Count() - (TurnsInRound() * 2);
            return (RemainingPlayers.Skip(turn + byes).First(), RemainingPlayers.SkipLast(turn).Last());
        }
    }
}

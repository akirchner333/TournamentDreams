using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Series
{
    public class RepeatGames<Player> : AbstractTournament<Player>
    {
        public int Games { get; private set; }
        public RepeatGames(Player a, Player b, int games) : base(new Player[2] { a, b })
        {
            Games = games;
        }

        public RepeatGames((Player a, Player b) tuple, int games) : base(new Player[2] { tuple.a, tuple.b })
        {
            Games = games;
        }

        public override (int, int) TurnIndexes(int turn)
        {
            if (turn % 2 == 0)
                return (0, 1);
            return (1, 0);
        }

        public override List<(Player, Player)> CurrentRound()
        {
            var list = new List<(Player, Player)>();
            for (var i = 0; i < Games; i++)
            {
                if (i % 2 == 0)
                    list.Add((Players[0].player, Players[1].player));
                else
                    list.Add((Players[1].player, Players[0].player));
            }
            return list;
        }

        public override int TurnsInRound()
        {
            return Games;
        }

        public override bool Finished()
        {
            return Round > 0;
        }
    }
}

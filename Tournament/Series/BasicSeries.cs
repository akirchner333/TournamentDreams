using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Series
{
    // A series is a sequence of games between two players
    // So it covers best-of-n, n games in sequence, play until you don't get a draw, etc
    public class BasicSeries<Player>
    {
        public Player A { get; private set; }
        public Player B { get; private set; }

        public int Turn { get; set; } = 0;
        public BasicSeries(Player a, Player b)
        {
            A = a;
            B = b;
        }

        public (Player, Player) NextGame()
        {
            return (A, B);
        }

        public bool Finished()
        {
            return Turn > 0;
        }
    }
}

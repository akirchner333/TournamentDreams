using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Series
{
    public class BestOf<Player> : RepeatGames<Player>
    {
        public BestOf(Player a, Player b, int games) : base(a, b, games)
        {
            if (games % 2 != 1)
                throw new Exception($"The BestOf series must receive an odd number of games. {games} is not odd.");
        }

        public override bool Finished()
        {
            return Players.Any(p => p.record.Wins > Games / 2);
        }
    }
}
